///<author>
///Andrew Thompson & William Meldrum
///</author>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Network.SocketState;

namespace Network
{
    /// <summary>
    /// An abstract class that handles network communication between a client and a server.
    /// </summary>
    public static class NetworkController
    {

        /// <summary>
        /// This method attempts to make a connection using the given NetworkAction delegate and IP(hostname).
        /// </summary>
        /// <param name="n">A delegate that uses the SocketState</param>
        /// <param name="hostname">The IP of the server</param>
        /// <returns>The Socket</returns>
        public static Socket ConnectToServer(NetworkAction n, string hostname)
        {
            MakeSocket(hostname, out Socket socket, out IPAddress addr);
            SocketState state = new SocketState();
            state.socket = socket;
            state.callMe = n;
            state.socket.BeginConnect(addr, 2112, ConnectedCallback, state);
            return socket;
        }


        /// <summary>
        /// Creates a Socket object for the given host string
        /// </summary>
        /// <param name="hostName">The host name or IP address</param>
        /// <param name="socket">The created Socket</param>
        /// <param name="ipAddress">The created IPAddress</param>
        public static void MakeSocket(string hostName, out Socket socket, out IPAddress ipAddress)
        {
            ipAddress = IPAddress.None;
            socket = null;
            try
            {
                // Establish the remote endpoint for the socket.
                IPHostEntry ipHostInfo;

                // Determine if the server address is a URL or an IP
                try
                {
                    ipHostInfo = Dns.GetHostEntry(hostName);
                    bool foundIPV4 = false;
                    foreach (IPAddress addr in ipHostInfo.AddressList)
                        if (addr.AddressFamily != AddressFamily.InterNetworkV6)
                        {
                            foundIPV4 = true;
                            ipAddress = addr;
                            break;
                        }
                    // Didn't find any IPV4 addresses
                    if (!foundIPV4)
                    {
                        System.Diagnostics.Debug.WriteLine("Invalid addres: " + hostName);
                        throw new ArgumentException("Invalid address");
                    }
                }
                catch (Exception)
                {
                    // see if host name is actually an ipaddress, i.e., 155.99.123.456
                    System.Diagnostics.Debug.WriteLine("using IP");
                    ipAddress = IPAddress.Parse(hostName);
                }

                // Create a TCP/IP socket.
                socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

                // Disable Nagle's algorithm - can speed things up for tiny messages, 
                // such as for a game
                socket.NoDelay = true;

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to create socket. Error occured: " + e);
                throw new ArgumentException("Invalid address");
            }
        }

        /// <summary>
        /// Finishes the connection process and calls the the next CallMe method.
        /// </summary>
        /// <param name="stateAsArObject">The SocketState from ConnectToServer as an IAsyncResult</param>
        private static void ConnectedCallback(IAsyncResult stateAsArObject)
        {
            SocketState state = (SocketState)stateAsArObject.AsyncState;
            state.socket.EndConnect(stateAsArObject);
            state.callMe(state);
        }

        /// <summary>
        /// Called when ready to receive data from the server.
        /// </summary>
        /// <param name="state">The SocketState containing the socket and the information used in the connection</param>
        public static void GetData(SocketState state)
        {
            state.socket.BeginReceive(state.byteBuffer, 0, 4096, SocketFlags.None, ReceiveCallback, state);
        }

        /// <summary>
        /// Called when new data arrives, and sends it using ProcessData. Then requests more data.
        /// </summary>
        /// <param name="stateAsArObject">The SocketState containing the socket and the information used in the connection</param>
        private static void ReceiveCallback(IAsyncResult stateAsArObject)
        {
            SocketState state = (SocketState)stateAsArObject.AsyncState;

            int bytes;

            try
            {
                bytes = state.socket.EndReceive(stateAsArObject);
            }
            catch (Exception)
            {
                return;
            }
            

            if (bytes > 0)
            {
                string message = Encoding.UTF8.GetString(state.byteBuffer, 0, bytes);



                // Check if sb contains a complete message
                lock (state)
                {
                    state.growableBuffer.Append(message);
                    ProcessData(state);
                    //Request more data
                    GetData(state);
                }

            }
            else
                state.socket.Close(); //If zero bytes were sent, the connection was closed.


        }

        /// <summary>
        /// Helper method that examines the strings in the StringBuilder, splits them whenever a \n is found,
        /// and uses the callMe to handle them if it's a complete part ending in \n. Removes the strings once
        /// they've been handled.
        /// </summary>
        /// <param name="ss"></param>
        private static void ProcessData(SocketState ss)
        {
            lock (ss)
            {
                string totaldata = ss.growableBuffer.ToString();

                string[] parts = Regex.Split(totaldata, @"(?<=[\n])");
                foreach (string p in parts)
                {
                    //ignore empty strings
                    if (p.Length == 0)
                        continue;

                    if (p[p.Length - 1] != '\n' && p[p.Length - 2] != '\n')
                        continue;

                    // process p

                    ss.currentMessage = p;
                    ss.callMe(ss);
                    ss.growableBuffer.Remove(0, p.Length);
                }
            }

        }

        /// <summary>
        /// This method is used to send data as bytes through the current socket.
        /// </summary>
        /// <param name="socket">The socket used in the current connection</param>
        /// <param name="data">The data to be sent</param>
        public static bool Send(Socket socket, String data)
        {
            if (!socket.Connected)
            {
                return false;
            }
            else
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                try
                {
                    socket.BeginSend(dataBytes, 0, dataBytes.Length, SocketFlags.None, SendCallback, socket);
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Called when the socket is finished sending the data from Send.
        /// </summary>
        /// <param name="ar"></param>
        private static void SendCallback(IAsyncResult ar)
        {
            Socket s = (Socket)ar.AsyncState;
            s.EndSend(ar);
        }

        //SERVER RELATED CODE

        /// <summary>
        /// Server begins listening for any IP address on 
        /// port 11000. Notifies the user that the server is 
        /// ready and waiting.
        /// </summary>
        /// <param name="na">the current callMe delegate</param>
        public static void ServerAwaitingClientLoop(NetworkAction na)
        {
            TcpListener lstn = new TcpListener(IPAddress.Any, 11000);
            lstn.Start();
            ServerTCPState tcpState = new ServerTCPState();
            tcpState.tcpListener = lstn;
            tcpState.callMe = na;
            Console.WriteLine("awaiting tcp connection");
            lstn.BeginAcceptSocket(AcceptNewClient, tcpState);
        }

        /// <summary>
        /// Creates a TCPState, which in turn creates and assigns
        /// values to the tcpListener and callMe delegate within.
        /// A socket state is then created as well to begin the event loop
        /// between the client and server.
        /// </summary>
        /// <param name="ar">contains the tcpState</param>
        public static void AcceptNewClient(IAsyncResult ar)
        {
            ServerTCPState tcpState = (ServerTCPState)ar.AsyncState;
            Socket socket = tcpState.tcpListener.EndAcceptSocket(ar);
            Console.WriteLine("New client connection");
            SocketState ss = new SocketState();
            ss.socket = socket;
            ss.callMe = tcpState.callMe;
            ss.callMe(ss);
            tcpState.tcpListener.BeginAcceptSocket(AcceptNewClient, tcpState);
        }

    }



}
