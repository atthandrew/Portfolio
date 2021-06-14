///<author>
///Andrew Thompson & William Meldrum
///</author>

using Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpaceWars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Network.SocketState;
using System.Windows.Forms;
using System.Net.Sockets;

namespace SpaceWars
{

    /// <summary>
    /// This class handles information received from the server, and sends information/events to the model and view.
    /// </summary>
    public class GameController
    {
        //Types of handlers used when passing an event to ClientWindow.
        public delegate void handler();
        public delegate void errorHandler(Exception e);
        public event handler ResizeHandler;
        public event handler RedrawHandler;
        public event errorHandler ErrorHandler;

        public string nameOfPlayer;
        
        private Socket theServer; //The socket used for the connection to the server.
        private bool RSHasBeenCalled; //Keeps track of whether we have read one or both parts of the startup message from the server.
        private World theWorld; //The World object that contains all 

        //These booleans keep track of when a key is pressed down in the DrawingPanel.
        private bool leftKey;
        private bool rightKey;
        private bool thrustKey;
        private bool fireKey;


        public GameController()
        {
            RSHasBeenCalled = false;
            theWorld = new World();
        }

        /// <summary>
        /// Getter method for the World used by the current GameController
        /// </summary>
        /// <returns>The World used by GameController</returns>
        public World GetWorld()
        {
            return theWorld;
        }

        /// <summary>
        /// Handler for ConnectButton that attempts to connect to the specified server.
        /// </summary>
        /// <param name="ip">The ip of the server</param>
        /// <param name="playerName">The name of the player</param>
        public void ConnectButtonPressed(String ip, String playerName)
        {
            nameOfPlayer = playerName + "\n";

            NetworkAction na = HandleFirstContact;
            try
            {
                theServer = NetworkController.ConnectToServer(na, ip);
            }
            catch (Exception e)
            {
                HandleError(e);
            }
            
        }

        /// <summary>
        /// After a connection is made, prepares to receive startup information and sends the socket and player name.
        /// </summary>
        /// <param name="state">The SocketState containing the socket and information used in the connection</param>
        public void HandleFirstContact(SocketState state)
        {
            state.callMe = ReceiveStartup;
            NetworkController.Send(state.socket, nameOfPlayer);
            NetworkController.GetData(state);
        }

        /// <summary>
        /// Receives and saves the startup information, and prepares to start drawing frames.
        /// </summary>
        /// <param name="ss">The SocketState containing the socket and information used in the connection</param>
        public void ReceiveStartup(SocketState ss)
        {

            if (int.TryParse(ss.currentMessage, out int result))
            {
                //Sets the player's ID in the SocketState.
                if (!RSHasBeenCalled)
                {
                    ss.playerID = result;
                }
                //Sets the World dimensions and, prepares to ReceiveWorld, and resizes the client/DrawingPanel.
                else
                {
                    theWorld.dimensions = result;

                    ss.callMe = ReceiveWorld;
                    Resize();
                }
                RSHasBeenCalled = true;
            }

        }

        /// <summary>
        /// Parses JSON objects received by the server and saves them into the appropriate dictionaries in the World.
        /// </summary>
        /// <param name="ss">The SocketState containing the socket and information used in the connection</param>
        public void ReceiveWorld(SocketState ss)
        {
            JObject obj = JObject.Parse(ss.currentMessage);

            JToken token = obj["ship"];

            lock (theWorld)
            {
                if (token != null)
                {
                    //obj is a ship
                    Ship ship = JsonConvert.DeserializeObject<Ship>(ss.currentMessage);
                    if (!theWorld.shipDictionary.ContainsKey(ship.ID))
                    {
                        theWorld.shipDictionary.Add(ship.ID, ship);
                    }
                    else
                    {
                        theWorld.shipDictionary[ship.ID] = ship;
                    }
                }

                token = obj["star"];

                if (token != null)
                {
                    //obj is a star
                    Star star = JsonConvert.DeserializeObject<Star>(ss.currentMessage);
                    if (!theWorld.starDictionary.ContainsKey(star.ID))
                    {
                        theWorld.starDictionary.Add(star.ID, star);
                    }
                    else
                        theWorld.starDictionary[star.ID] = star;
                }

                token = obj["proj"];

                if (token != null)
                {
                    //obj is a proj
                    Projectile proj = JsonConvert.DeserializeObject<Projectile>(ss.currentMessage);

                    if (!theWorld.projectileDictionary.ContainsKey(proj.ID))
                    {
                        //This if statement handles projectiles shot directly into a ship/star (that are therefore dead on creation).
                        if (!proj.alive)
                        {
                            theWorld.projectileDictionary.Remove(proj.ID);
                        }
                        else
                            theWorld.projectileDictionary.Add(proj.ID, proj);
                    }
                    else
                    {
                        //This if statement removes dead projectiles from the dictionary.
                        if (!proj.alive)
                        {
                            theWorld.projectileDictionary.Remove(proj.ID);
                        }
                        else
                            theWorld.projectileDictionary[proj.ID] = proj;
                    }

                }
            }

            SendToView(); //Redraws the DrawingPanel
            NetworkController.Send(theServer, BuildKeyMessage()); //Sends any key inputs detected during this time.

        }

        /// <summary>
        /// Calls all RedrawHandlers in ClientWindow
        /// </summary>
        private void SendToView()
        {
            RedrawHandler();
        }

        /// <summary>
        /// Handler for the OnKeyDown event in DrawingPanel
        /// </summary>
        /// <param name="e">The Keys pressed down</param>
        public void SendKeyToServer(KeyEventArgs e)
        {

            if (e.KeyData == Keys.Left)
            {
                leftKey = true;
            }

            if (e.KeyData == Keys.Right)
            {
                rightKey = true;
            }

            if (e.KeyData == Keys.Up)
            {
                thrustKey = true;
            }

            if (e.KeyData == Keys.Space)
            {
                fireKey = true;
            }

        }

        /// <summary>
        /// Helper method that builds a string in the form "(inputs)/n" that will be sent to the server. 
        /// </summary>
        /// <returns>A string in the form "(inputs)/n"</returns>
        private string BuildKeyMessage()
        {
            string inputMessage = "(";

            if (leftKey == true)
                inputMessage += "L";
            if (rightKey == true)
                inputMessage += "R";
            if (thrustKey == true)
                inputMessage += "T";
            if (fireKey == true)
                inputMessage += "F";
            inputMessage += ")\n";

            return inputMessage;
        }

        /// <summary>
        /// Handler for the OnKeyUp event in DrawingPanel
        /// </summary>
        /// <param name="e">The Keys not being pressed</param>
        public void KeyReleaseHandler(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Left)
            {
                leftKey = false;
            }

            if (e.KeyData == Keys.Right)
            {
                rightKey = false;
            }

            if (e.KeyData == Keys.Up)
            {
                thrustKey = false;
            }

            if (e.KeyData == Keys.Space)
            {
                fireKey = false;
            }
        }

        /// <summary>
        /// Calls all ResizeHandlers in ClientWindow
        /// </summary>
        private void Resize()
        {
            ResizeHandler();
        }

        /// <summary>
        /// Calls all ErrorHandlers in ClientWindow
        /// </summary>
        /// <param name="e">The exceptions to be handled</param>
        private void HandleError(Exception e)
        {
            ErrorHandler(e);
        }
    }


}

