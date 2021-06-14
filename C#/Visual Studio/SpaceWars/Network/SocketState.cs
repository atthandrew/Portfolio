///<author>
///Andrew Thompson & William Meldrum
///</author>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    /// <summary>
    /// This is a wrapper class for the socket used in a connection, a data buffer, a growable buffer, a delegate used to call methods,
    /// and a string representing a message being examined.
    /// </summary>
    public class SocketState
    {
        public Socket socket;
        public StringBuilder growableBuffer = new StringBuilder();
        public byte[] byteBuffer = new byte[4096]; //4 kilobytes, as allowed in lecture.
        public int playerID;
        public delegate void NetworkAction(SocketState s);
        public NetworkAction callMe;

        public string currentMessage; //A complete message being examined in the NetworkController.
    }

    
}
