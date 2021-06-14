///<author>
///Andrew Thompson & William Meldrum
/// </author>

using Network;
using SpaceWars;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using static Network.SocketState;

namespace ServerNS
{
    /// <summary>
    /// Represents a server for the Spacewars game. Handles all mechanics and computes
    /// interactions between clients and Spacewars objects. Connects/Disconnects clients as needed.
    /// </summary>
    public class Server
    {
        World serverWorld; //Holds model info for all clients.

        //These three variables are incremented to give each model instance a unique ID
        int uniqueShipID; 
        int uniqueStarID;
        int uniqueProjID;

        int currentFrame; //Counter that increments every time update is called.
        int msPerFrame; //Determines rate that update is called.
        int framesPerShot; //Frames between the firing of each projectile.
        int respawnRate; //The number of frames a ship must wait to respawn.
        int startingHP;
        int projectileSpeed; //The velocity constant for a projectile
        double engineStrength; //Determines the effect thrust has on acceleration
        int turningRate; //The degrees turned per frame if a turn input is received
        int shipSize; //The collision area for a ship
        int starSize; //The collision area for a star
        bool tagGameMode; //Turns on the tag game mode. See README for details.
        int isIt; //When playing tag, holds the ID of the ship that is "it"
        int wasIt; //When playing tag, holds the ID of the previous ship that was "it"

        Dictionary<int, SocketState> clients; //The list of clients connected to the server that receive updates
        Dictionary<int, Vector2D> shipVelocities; //The instantaneous velocities of each ship
        Stopwatch stopWatch; //Used to control the rate updates are sent.
        Dictionary<int, int> frameToRespawn; //Holds the frames at which dead ships will respawn.
        Dictionary<int, int> lastFrameFired; //Keeps track of the last frame a projectile was fired for each ship.
        Dictionary<int, int> lastFrameRotated; //Keeps track of the last frame each ship was rotated.
        Dictionary<int, int> lastPointFrame; //When playing tag, keeps track of the last frame points were awarded to those who weren't "it"
        List<int> disconnectedShips; //Temporarily holds the ships of disconnected clients before deletion.
        List<int> shipKillers; //Holds the ships that will be awarded points after getting a killing blow
        List<int> projToRemove; //Holds dead projectiles that need to be removed.
        List<int> clientsToRemove; //Holds disconnected clients

        static void Main(string[] args)
        {
            Server server = new Server();
            NetworkAction na = server.HandleNewClient;
            server.stopWatch = new Stopwatch();
            server.stopWatch.Start();
            NetworkController.ServerAwaitingClientLoop(na); //Starts the tcpListener loops that connect all clients
            while (true)
            {
                server.Update(); //Starts the loop that updates and sends the world to all clients
            }
        }

        public Server()
        {
            serverWorld = new World();
            currentFrame = 0;
            lastFrameFired = new Dictionary<int, int>();
            lastFrameRotated = new Dictionary<int, int>();
            lastPointFrame = new Dictionary<int, int>();
            frameToRespawn = new Dictionary<int, int>();
            clients = new Dictionary<int, SocketState>();
            shipVelocities = new Dictionary<int, Vector2D>();
            uniqueShipID = 0;
            uniqueStarID = 0;
            uniqueProjID = 0;
            shipKillers = new List<int>();
            projToRemove = new List<int>();
            clientsToRemove = new List<int>();
            disconnectedShips = new List<int>();
            readXMLFile(); //Reads in all starting variables from the settings XML file in Resources.
            if (tagGameMode)
            {
                startingHP = 1; //Ensures players will always be tagged in 1 hit
                wasIt = -1; //Makes sure no one "was it" before the game starts.
            }
        }

        /// <summary>
        /// The CallMe delegate callback that handles a new client connecting. 
        /// Prepares to receive name from client.
        /// </summary>
        /// <param name="ss">Holds client's socket and related info</param>
        public void HandleNewClient(SocketState ss)
        {
            ss.callMe = ReceiveName;
            NetworkController.GetData(ss);
        }

        /// <summary>
        /// A CallMe that handles the client's name once received.
        /// Creates a ship for the client, adds it to needed dictionaries.
        /// Sends the client a unique ID and the world dimensions.
        /// Prepares to receive keyboard inputs indefinitely.
        /// </summary>
        /// <param name="ss">Holds client's socket and related info</param>
        public void ReceiveName(SocketState ss)
        {
            Ship s = new Ship();
            s.name = ss.currentMessage.Trim(); //Trims off newline characters
            s.ID = uniqueShipID;
            ss.playerID = s.ID;
            uniqueShipID++; //increment for next unique ID
            s.hp = startingHP;
            s.dir = GenerateNormalizedVector();
            s.loc = CreateSpawn();
            s.score = 0;
            s.thrust = false;
            serverWorld.shipDictionary.Add(s.ID, s);
            shipVelocities.Add(s.ID, new Vector2D(0, 0));
            lastFrameFired.Add(s.ID, 0);
            lastFrameRotated.Add(s.ID, 0);
            lastPointFrame.Add(s.ID, 0);

            ss.callMe = HandleClientData;

            //Stops the loop if sending fails. Client/ship eventually removed in Update.
            if (NetworkController.Send(ss.socket, s.ID + "\n" + serverWorld.dimensions + "\n"))
            {
                lock (clients)
                {
                    clients.Add(s.ID, ss);
                }

                NetworkController.GetData(ss);
            }

        }

        /// <summary>
        /// CallMe delegate that handles keyboard inputs received from each client.
        /// </summary>
        /// <param name="ss">Holds client's socket and related info</param>
        public void HandleClientData(SocketState ss)
        {
            lock (serverWorld)
            {
                if (ss.currentMessage.Contains("T"))
                {
                    serverWorld.shipDictionary[ss.playerID].thrust = true;
                }

                //This if statement makes sure dead ships can't rotate or fire.
                if (serverWorld.shipDictionary[ss.playerID].hp > 0)
                {
                    if (ss.currentMessage.Contains("F"))
                    {
                        //If playing tag, only allow the player who is "it" to fire.
                        if (tagGameMode && isIt != ss.playerID)
                            goto DONTFIRE;

                        //Waits framesPerShot until firing again.
                        if (currentFrame > lastFrameFired[ss.playerID] + framesPerShot)
                        {
                            Projectile p = new Projectile();
                            serverWorld.projectileDictionary.Add(uniqueProjID, p);
                            p.ID = uniqueProjID++;
                            p.owner = ss.playerID;
                            p.dir = new Vector2D(serverWorld.shipDictionary[ss.playerID].dir);
                            p.loc = new Vector2D(serverWorld.shipDictionary[ss.playerID].loc);
                            p.alive = true;
                            lastFrameFired[ss.playerID] = currentFrame;
                        }
                    DONTFIRE: { };
                    }

                    //Waits 1 frame until rotating again.
                    if (ss.currentMessage.Contains("L") && currentFrame > lastFrameRotated[ss.playerID] + 1)
                    {
                        serverWorld.shipDictionary[ss.playerID].dir.Rotate(-turningRate);
                        lastFrameRotated[ss.playerID] = currentFrame;
                    }

                    //Waits 1 frame until rotating again.
                    if (ss.currentMessage.Contains("R") && currentFrame > lastFrameRotated[ss.playerID] + 1)
                    {
                        serverWorld.shipDictionary[ss.playerID].dir.Rotate(turningRate);
                        lastFrameRotated[ss.playerID] = currentFrame;
                    }
                }

            }
        }

        /// <summary>
        /// Updates the locations, directions, motion, and status of all Spacewars objects.
        /// Sends the updated world data to all connected clients.
        /// </summary>
        public void Update()
        {
            StringBuilder sb = new StringBuilder(); //Holds the message to be sent to all clients.
            while (stopWatch.ElapsedMilliseconds < msPerFrame)
            {
                // Wait to update until msPerFrame time has passed.
            }
            stopWatch.Reset();


            lock (serverWorld)
            {
                updateShips(sb);
                removeDisconnectedShips(disconnectedShips); //Removes the list of ships created from removeDisconnectedClients
                disconnectedShips.Clear();

                foreach (Star star in serverWorld.starDictionary.Values)
                {
                    sb.Append(JsonConvert.SerializeObject(star) + "\n");
                }

                updateProjectiles(sb);
                projCleanup(projToRemove, shipKillers);
                shipKillers.Clear();
                projToRemove.Clear();

                string message = sb.ToString(); //Creates the final message to be sent
                lock (clients)
                {
                    foreach (SocketState ss in clients.Values)
                    {
                        //If cannot send to client, it has been disconnected. Prepares to remove client.
                        if(!NetworkController.Send(ss.socket, message))
                        {
                            clientsToRemove.Add(ss.playerID);
                        }
                        
                    }
                }
                removeDisconnectedClients(clientsToRemove); //Removes client and its associated data.
                clientsToRemove.Clear();

                //Sets thrust back to false, so only another thrust input will make it true again.
                foreach(Ship s in serverWorld.shipDictionary.Values)
                {
                    s.thrust = false;
                }

                currentFrame++;
                stopWatch.Start();
            }
        }

        /// <summary>
        /// Generates a random normalized vector.
        /// Used for giving a random direction to ships on respawn.
        /// </summary>
        /// <returns>Random normalized vector</returns>
        private Vector2D GenerateNormalizedVector()
        {
            var rand = new System.Random();

            double min = -1;
            double max = 1;
            double ranX = min + (rand.NextDouble() * (max - min)); //Gives a number between -1 and 1
            double ranY = min + (rand.NextDouble() * (max - min));

            Vector2D vector2d = new Vector2D(ranX, ranY);
            vector2d.Normalize();
            return vector2d;
        }

        /// <summary>
        /// Generates a random location within the world dimensions to respawn a ship at.
        /// Ensures spawn points are at least 20 units away from a star. 
        /// </summary>
        /// <returns>A random vector location within world bounds</returns>
        private Vector2D CreateSpawn()
        {
            var rand = new System.Random();

            int ranX = rand.Next(-serverWorld.dimensions / 2, serverWorld.dimensions / 2);
            int ranY = rand.Next(-serverWorld.dimensions / 2, serverWorld.dimensions / 2);

            //Ensures we don't get a vector of <0,0>
            if (ranX == 0 && ranY == 0)
                ranX = 1;

            Vector2D spawnPoint = new Vector2D(ranX, ranY);

            foreach (Star star in serverWorld.starDictionary.Values)
            {
                //Moves spawn points that are within stars to be just outside the star size and then some.
                if ((spawnPoint - star.loc).Length() < starSize + 20)
                {
                    spawnPoint.Normalize();
                    spawnPoint *= starSize + 20;
                }

            }
            return spawnPoint;
        }

        /// <summary>
        /// Helper method that removes clients that have been found to be disconnected.
        /// Used in conjunction with Update to send the dead ships/projectiles one last time.
        /// </summary>
        /// <param name="clientIDs"></param>
        private void removeDisconnectedClients(List<int> clientIDs)
        {
            foreach (int ID in clientIDs)
            {
                clients.Remove(ID);
                serverWorld.shipDictionary[ID].hp = 0; //Kills the client's ship

                //Kills the client's projectiles
                foreach(Projectile proj in serverWorld.projectileDictionary.Values)
                {
                    if(proj.owner == ID)
                    {
                        proj.alive = false;
                    }
                }
                
                //Removes ID from all related dictionaries
                shipVelocities.Remove(ID);
                lastFrameFired.Remove(ID);
                lastFrameRotated.Remove(ID);

                //Prepares ship to be removed after its dead state is sent to the clients.
                disconnectedShips.Add(ID);

                if (tagGameMode)
                    lastPointFrame.Remove(ID); //Prevents update from attempting to award further points.
            }
        }

        /// <summary>
        /// Removes disconnected ships that have already sent their dead state to the clients.
        /// </summary>
        /// <param name="shipIDs"></param>
        private void removeDisconnectedShips(List<int> shipIDs)
        {
            foreach(int ID in shipIDs)
            {
                serverWorld.shipDictionary.Remove(ID);
            }
        }

        /// <summary>
        /// Removes dead projectiles and awards points to the killers of other ships.
        /// </summary>
        /// <param name="projIDs">Dead projectiles to be removed</param>
        /// <param name="killerIDs">Ships to be awarded points</param>
        private void projCleanup(List<int> projIDs, List<int> killerIDs)
        {
            foreach(int ID in killerIDs)
            {
                serverWorld.shipDictionary[ID].score++;
            }

            foreach (int ID in projIDs)
            {
                serverWorld.projectileDictionary.Remove(ID);
            }
        }

        /// <summary>
        /// Checks to see if a dead ship's respawn countdown has expired.
        /// If it has, respawns the ship.
        /// </summary>
        /// <param name="ship">The dead ship to be checked</param>
        private void SpawnCheck(Ship ship)
        {
            if (frameToRespawn[ship.ID] == currentFrame)
            {
                shipVelocities[ship.ID] = new Vector2D(0,0);
                frameToRespawn.Remove(ship.ID);
                ship.hp = startingHP;
                ship.loc = CreateSpawn();
                ship.dir = GenerateNormalizedVector();
            }
            
        }

        /// <summary>
        /// Helper method for Update. Updates the ships in the serverWorld shipDictionary.
        /// Appends the updated ships to the stringbuilder to be sent to all clients.
        /// </summary>
        /// <param name="sb">The StringBuilder used throughout update</param>
       private void updateShips(StringBuilder sb)
       {
            lock (serverWorld)
            {
                foreach (Ship ship in serverWorld.shipDictionary.Values)
                {
                    //When playing tag, checks all connected ships (which are in lastPointFrame, see removeDisconnectedClients)
                    if (tagGameMode && lastPointFrame.ContainsKey(ship.ID))
                    {
                        //When starting the game, assigns the first ship to be "it."
                        //If the ship that is "it" disconnects, arbitrarily assigns another ship to be "it."
                        if (serverWorld.shipDictionary.Count == 1 || !serverWorld.shipDictionary.ContainsKey(isIt))
                        {
                            wasIt = -1; //Ensures ships that were it can still be chosen again.
                            isIt = ship.ID;
                        }

                        //Increases the score of ships that are not it by 1 every 60 frames.
                        if (ship.ID != isIt && currentFrame > lastPointFrame[ship.ID] + 60)
                        {
                            ship.score++;
                            lastPointFrame[ship.ID] = currentFrame; //Resets countdown until the next point.
                        }
                    }

                    if (ship.hp > 0)
                    {
                        // compute gravity
                        Vector2D g = new Vector2D(0, 0); //acceleration caused by all stars
                        foreach (Star star in serverWorld.starDictionary.Values)
                        {
                            //Detects if the ship collides with a star
                            if ((ship.loc - star.loc).Length() < starSize)
                            {
                                //Kills the ship and starts a respawn countdown.
                                ship.hp = 0;
                                frameToRespawn.Add(ship.ID, currentFrame + respawnRate);

                                //When playing tag, ships that die to the star become "it".
                                if (tagGameMode)
                                {
                                    isIt = ship.ID;
                                }
                                goto END;
                            }
                            Vector2D gPartial = new Vector2D();
                            gPartial = star.loc - ship.loc;
                            gPartial.Normalize();
                            gPartial *= star.mass;

                            g += gPartial;
                        }

                        // compute thrust
                        Vector2D t = new Vector2D(ship.dir);
                        if (ship.thrust)
                        {
                            t *= engineStrength;
                        }
                        else
                        {
                            t *= 0;
                        }

                        // acceleration = sum of all stars + thrust
                        Vector2D a = (g + t);

                        // velocity += acceleration
                        shipVelocities[ship.ID] += a;

                        // position += velocity
                        //The if statement handles wraparound, putting ships on the opposite side if they go out of bounds.
                        if (Math.Abs((ship.loc + shipVelocities[ship.ID]).GetY()) > serverWorld.dimensions / 2
                            || Math.Abs((ship.loc + shipVelocities[ship.ID]).GetX()) > serverWorld.dimensions / 2)
                        {
                            if (Math.Abs((ship.loc + shipVelocities[ship.ID]).GetX()) > serverWorld.dimensions / 2)
                            {
                                Vector2D wrapVector = new Vector2D(ship.loc.GetX() * -1, ship.loc.GetY());
                                ship.loc = wrapVector;
                                ship.loc += shipVelocities[ship.ID];
                            }
                            if (Math.Abs((ship.loc + shipVelocities[ship.ID]).GetY()) > serverWorld.dimensions / 2)
                            {
                                Vector2D wrapVector = new Vector2D(ship.loc.GetX(), ship.loc.GetY() * -1);
                                ship.loc = wrapVector;
                                ship.loc += shipVelocities[ship.ID];
                            }
                        }
                        else
                        {
                            ship.loc += shipVelocities[ship.ID];
                        }

                    END: { }
                    }

                    else
                    {
                        //Checks dead ships to see if they are ready to respawn.
                        //Ignores disconnected ships (which are not in frameToRespawn.)
                        if (frameToRespawn.ContainsKey(ship.ID))
                            SpawnCheck(ship);
                    }

                    sb.Append(JsonConvert.SerializeObject(ship) + "\n");
                }
            }
        }


        /// <summary>
        /// Helper method for Update. Updates the projectiles in the serverWorld shipDictionary, detecting collisions and awarding points.
        /// Appends the updated projectiles to the stringbuilder to be sent to all clients.
        /// </summary>
        /// <param name="sb">The StringBuilder used throughout update</param>
        private void updateProjectiles(StringBuilder sb)
        {
            lock (serverWorld)
            {
                foreach (Projectile proj in serverWorld.projectileDictionary.Values)
                {
                    if (tagGameMode && proj.owner == wasIt)
                    {
                        proj.alive = false; //Immediately kills wasIt's projectiles when another ship becomes it.
                    }

                    if (proj.alive == false)
                    {
                        projToRemove.Add(proj.ID);
                    }

                    else
                    {
                        //Kills projectiles that leave the world bounds.
                        if (Math.Abs(proj.loc.GetX()) > serverWorld.dimensions / 2
                            || Math.Abs(proj.loc.GetY()) > serverWorld.dimensions / 2)
                        {
                            proj.alive = false;
                            continue;
                        }

                        proj.loc += proj.dir * projectileSpeed; //calculates the new location

                        foreach (Star star in serverWorld.starDictionary.Values)
                        {
                            //Kills projectiles that hit stars.
                            if ((proj.loc - star.loc).Length() < starSize)
                            {
                                proj.alive = false;
                                continue;
                            }
                        }

                        foreach (Ship s in serverWorld.shipDictionary.Values)
                        {
                            //Prevents projectiles from colliding with owner.
                            if (s.ID == proj.owner || s.hp == 0)
                            {
                                continue;
                            }

                            //Detects projectile collisions with enemy ships.
                            if ((proj.loc - s.loc).Length() < shipSize)
                            {
                                proj.alive = false;
                                s.hp--; //Reduces the hp of the ship that was hit by 1.

                                //If the projectile kills the target . . .
                                if (s.hp == 0)
                                {
                                    //Prepares the killed ship to respawn, and marks the killer to receive a point.
                                    frameToRespawn.Add(s.ID, currentFrame + respawnRate);
                                    shipKillers.Add(proj.owner);

                                    if (tagGameMode)
                                    {
                                        //Update the "it" conditions based on who was tagged.
                                        wasIt = isIt;
                                        isIt = s.ID;
                                    }
                                }
                            }
                        }
                    }
                    sb.Append(JsonConvert.SerializeObject(proj) + "\n");
                }
            }
        }

        /// <summary>
        /// Reads in an XML file named settings.xml from the Resources folder.
        /// File must contain parameters for all of the cases within the switch-case block.
        /// </summary>
        private void readXMLFile()
        {
            using (XmlReader reader = XmlReader.Create("../../../Resources/settings.xml"))
            {
                try {
                    reader.ReadToFollowing("SpaceSettings");

                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "UniverseSize":
                                    reader.Read();
                                    serverWorld.dimensions = int.Parse(reader.Value);
                                    break;
                                case "MSPerFrame":
                                    reader.Read();
                                    msPerFrame = int.Parse(reader.Value);
                                    break;
                                case "FramesPerShot":
                                    reader.Read();
                                    framesPerShot = int.Parse(reader.Value);
                                    break;

                                case "RespawnRate":
                                    reader.Read();
                                    respawnRate = int.Parse(reader.Value);
                                    break;

                                case "Star":
                                    Star st = new Star();
                                    reader.ReadToFollowing("x");
                                    reader.Read();
                                    int x = int.Parse(reader.Value);

                                    reader.ReadToFollowing("y");
                                    reader.Read();
                                    int y = int.Parse(reader.Value);

                                    reader.ReadToFollowing("mass");
                                    reader.Read();
                                    double mass = double.Parse(reader.Value);
                                    st.loc = new Vector2D(x, y);
                                    st.mass = mass;
                                    st.ID = uniqueStarID++;
                                    serverWorld.starDictionary.Add(st.ID, st);
                                    break;

                                case "HP":
                                    reader.Read();
                                    startingHP = int.Parse(reader.Value);
                                    break;
                                case "ProjectileSpeed":
                                    reader.Read();
                                    projectileSpeed = int.Parse(reader.Value);
                                    break;
                                case "EngineStrength":
                                    reader.Read();
                                    engineStrength = double.Parse(reader.Value);
                                    break;
                                case "TurningRate":
                                    reader.Read();
                                    turningRate = int.Parse(reader.Value);
                                    break;
                                case "ShipSize":
                                    reader.Read();
                                    shipSize = int.Parse(reader.Value);
                                    break;
                                case "StarSize":
                                    reader.Read();
                                    starSize = int.Parse(reader.Value);
                                    break;
                                case "TagGameMode":
                                    reader.Read();
                                    if (reader.Value.ToLower() == "true")
                                        tagGameMode = true;
                                    else
                                        tagGameMode = false;
                                    break;

                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("Error while reading XML document. Error message: " + e.Message);
                    Console.Read();
                }
            }

            if (msPerFrame == 0 || framesPerShot == 0 || respawnRate == 0 || startingHP == 0 || projectileSpeed == 0 
                || engineStrength == 0 || turningRate == 0 || shipSize == 0 || starSize == 0)
            {
                throw new Exception("Not all starting values initialized, or one of them is equal to zero");
            }
        }
    }
}
