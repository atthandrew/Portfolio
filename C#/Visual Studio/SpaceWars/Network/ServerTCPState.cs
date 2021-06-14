///<author>
///Andrew Thompson & William Meldrum
///</author>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static Network.SocketState;

namespace Network
{
    /// <summary>
    /// Used to contain both the listener used by the server
    /// for all clients and callMe used to connect them.
    /// </summary>
    class ServerTCPState
    {
        public TcpListener tcpListener;
        public NetworkAction callMe;

        public ServerTCPState()
        {

        }
    }
}
