Authors: Andrew Thompson u0879848 and William Meldrum u0934535

Extra Features:
	- Health Bar changes colors based on health remaining
	- Players/Health Bars sorted by score
	- Yes/No dialog box for connection errors
	- Comic Sans ;D

11/11/2018
Implemented all NetworkController methods. We came to the conclusion that GetData in the ReceiveStartup 
block of the Networking Diagram represents calling GetData after processing the data from our buffer. This 
coincides with slide 14 of lecture 19, stating the we call BeginReceive/GetData from within ReceiveCallback 
after processing the data. 

11/12/2018
Implemented client library/GameController methods. The view now has an event listener DrawFrame(). It is
yet to be implemented. This listener will be called whenever the view needs to be updated, triggered by new
information received in GameController.
-We decided to store the dimensions of the game windows in World
-The next message to processed are stored on the SocketState

11/14/2018
We were able to partially implememnt the methods concerning world/image space in our DrawingPanel. We are unable to see if they are
properly implemented yet. 

11/16/2018
Finished implementing all provided method stubs. Connection cannot be established yet. It appears there is some cross-threading
issues. These will have to be handled next session.

11/17/2018
Fixed all cross-threading issues. We needed to lock any code that was iterating through the World's dictionaries for Ships, Projectiles,
and stars. This results in 3 lock statements total in the following classes.
-GameController > ReceiveWorld
-DrawingPanel > OnPaint
-HealthPanel > OnPaint
It looks like race codition was raised because we were modifying these dictionaries and drawing from them at the same time. In addition,
we were able to address bugs connecting to the SpaceWars server. We are now able to connect.Our DrawFrame() listener seems to be firing as intended. 
The images don't seem to be drawn correctly. This will have to be addressed next session.


11/18/2018
Our issues drawing objects in DrawingPanel have been resolved. We were not using the correct offset for these objects. Objects are now an appropriate size
and are accurate with the collision. 
The drawingPanel and ClientWindow now adjust with the size of the receive world dimensions. We were able to do so by resizing these
elements to our World dimensions within OnPaint in ClientWindow. If possible, we would like to resize these elements once after receive the
size of the World. 
We are still unable to send our input to the server. Currently we are listening for key presses in our ClientWindow, which then fires off a handler in our gamecontroller.
It looks like our key events are not firing at all. Will need to consult a TA.


11/19/2018
Finally got the client to send input messages to the server.  We have decided to "listen" for the inputs in our DrawingPanel, which then fire off handler events in
our Gamecontroller. The GameController has booleans for all of our control keys (up, down, space, etc.). The message is then sent at the very end of our ReceiveWorld
method. This is to make sure we are sending data at the same rate we are receiving it. 
We have implemented the healthbars as well. They appear in the right side in a 200 pixel margin.  We split this margin into 30 rows. Each row holds the player name and score or
their health. This allows us to see up to 15 players at once. 

We found a bug in which projectiles would "linger" after death. This would only happen when the projectile was fired on top of the star or an enemy ship. The projectile did not have 
a hitbox, so we were able to conlcude this was a client issue. We discovered that we were drawing a "dead" object, so we added code to check if the object was alive right before drawing it.
If the object was already considered dead, we would not draw it and remove it from its respective dictionary.

11/20/2018
We used another event to handle connection errors in GameController and ClientWindow, which are displayed with a Yes/No dialog box. We added in the rest of the extra features listed above.
There was one last unhandled error being thrown when attempting to close the window while it was drawing, (System.ObjectDisposedException), which we handled using a Try-Catch block.

PS8 LOGS

11/28/2018

Built skeleton of required classes and projects. Added the Server project. Added both ServerAwaitinClientLoop() and AcceptNewClient() to our NetworkController. Both of these SHOULD be working, but our server still needs to be built out to make a connection. Our server is able to read in an XML file just fine. We have implemented error-checking with our XML as well. We use and XMLReader and use a switch statement to see if the current element is relevant, if so, we assign it to our server fields. 

We’ve realized all objects will need some kind of identification. We only have ship IDs at the moment but will expand when ready. 

11/30/2018

We are seeing how the Server will have to track many things at once. We have decided to sort client objects/connections into dictionaries with their IDs at the key and the desired object as the value (i.e. velocity). 

We have ran into our first lock statement. Our client dictionary needs to be locked in ReceiveName so we can add the new client to the game. 

Our update method can be considered our “tick” function. All relevant data is/will be (re)calculated and sent out to our clients. For now, we’ve just have some basic code for ship gravity, but we should first make a proper connection between our client and server. 

We have implemented our CreateSpawn method. This method will choose a random x and y values within the play area. We then have logic to make sure the coordinates are not shared with a star. If they overlap, the method will recursively call itself until it finds an valid location. This may be more taxing on the CPU than desired -- will investigate further. (EDIT: WE HAVE NOW USED STAR RADIUS INSTEAD OF A RECURSIVE METHOD FOR COLLISION)

12/1/2018

We have now made contact with our server! Our client window is resizing/receiving the world dimensions. Nothing is drawn yet….

...Looks like we had 1 too many newline characters when appending/parsing our JSON string. Our window is resizing but no Objects are being drawn yet.

We have decided to use a stopwatch in our update method. We use a busy loop that runs x milliseconds until it is > than the number of milliseconds per frame, afterwards update will run. 

We have placed our second lock statement in the update method since it accesses ships, stars, and projectiles being shared in multiple threads. 

Objects are now being drawn! Kinda…. Star is not centered and our ship/ai ships appear on screen no more than a frame or two. Very sporadic. 

Turns out we were using panel coordinates rather than world coordinates with the center being (0,0). Still no controls, and gravity seems to be off. 

12/2/2018
Trouble strikes again! After running our server for ~5 seconds, ProcessData() throws an IndexOutOfBoundsException. We can replicate this problem even faster with more clients, but it is inconsistent. This one has stumped us. Not as much progress as expected.  

12/3/2018
BUGSKUAWSH! It appears the message stored in our socket was being accessed by multiple threads. We have placed lock statements when it is being accessed. This allows ProcessData() to remove the correct amount of characters from our message in our socket. 

Gravity has now been properly implemented, we had some arithmetic issues. Inputs are being read. We are flying.
All is well so far.

12/4/2018
Projectiles! We have implemented projectiles, but not collision/death. We use another stopwatch to make sure our rate of fire is consistent with our xml input. We also did this for left and right rotations. 

Wraparound is working as well! We had troubles thinking of how to implement this, but realized we can just get the absolute value of the x or y coordinate. If either other these are > half of the world width. We then times that coordinate by -1 and add the current velocity to the ship to guarantee it is in bounds. 


12/5/2018
We have now properly implement collision! We were able to find out to calculate the radius from a point instead of using squares. Our collision with stars and projectiles looks much cleaner. 

Handling disconnected clients has been more difficult than anticipated AND we have ran into a bug in which the class client will continue to thrust after it is released. It will only stop when a non-thrust key is pressed. 
We are having troubles making sure our clients receive the dead ship after our client has disconnected. 

We have replaced all of our stopwatches with an int that increments each time update() is called. This elimnates the potential margin of error used with stopwatches. 

12/6/2018
Andrew had the idea to use one frame of “preparation” when the client disconnects. The ship’s health is set to 0 on the collision frame and is taken out of the world space but is NOT removed from our dictionary of clients until the following frame. This way the clients receive the dead ship AND THEN the client/ship is completely removed from their respective dictionaries. 

TAG MODE
Like tag, one player is "it". Whoever is "it" can shoot while the others run away. Those who are not "it" will 
receive points over time. Whoever dies by projectile or star is then "it". Set the tag mode element in 
settings.xml to "true" to play or "false" otherwise. If a player disconnects while "it", the server will arbitrarily pick another player to be "it".


EXTRA IMPROVEMENTS
-Code handles multiple stars
-All "Basic Data" are settings within our xml file
-Cleans up list of connections as soon as a send or receive fails/Graceful disconnect
-ship faces random direction when spawns
-ship location is random when spawns and never intersects with stars
-All functionality listed in the Model section of PS8 has been implemented.