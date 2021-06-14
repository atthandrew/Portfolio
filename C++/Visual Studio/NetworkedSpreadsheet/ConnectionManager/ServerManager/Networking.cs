using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace NetworkControl
{
    public class Networking
    {
        
        public static bool isConnected;
        public delegate string ProcessData(string inputProcess);
        public delegate void Callback(MasterSocket tocallBack);
        private static MasterSocket ServerConnected;

        public Networking()
        {
            isConnected = false;
        }

        private static void recieveInfo(IAsyncResult aResult)
        {
            MasterSocket master = (MasterSocket)aResult.AsyncState;
            int numBytes = master.GetSocket.EndReceive(aResult);
            if(numBytes > 0)
            {
                string message = ASCIIEncoding.UTF8.GetString(master.GetBuffer, 0, numBytes);
                if (message.Contains("\n"))
                {
                    master.AppendMessage(message);
                }
            }
            lock (GetMaster)
            {
                ServerConnected.GetSocket.BeginReceive(ServerConnected.GetBuffer, 0, ServerConnected.GetBuffer.Length, SocketFlags.None, recieveInfo, ServerConnected);
            }

        }

        private static void connectionMade(IAsyncResult aResult)
        {
            try
            {
                MasterSocket master = (MasterSocket)aResult.AsyncState;
                master.GetSocket.EndConnect(aResult);
                isConnected = true;
                master.GetSocket.BeginReceive(master.GetBuffer, 0, master.GetBuffer.Length, SocketFlags.None, recieveInfo, master);
            }
            catch
            {
                //Do nothing, this is handled by something else
            }

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

        public static string ConnectToServer(string serverID)
        {    
            try
            {
                IPAddress iP;
                Socket newSocket;
                MakeSocket(serverID, out newSocket,out iP);

                MasterSocket master = new MasterSocket(newSocket);

                master.GetSocket.BeginConnect(iP, 2112, connectionMade, master);
                ServerConnected = master;
                int timeWaited = 0;
                while (master.GetSocket.Connected == false && timeWaited < 10)
                {
                    System.Threading.Thread.Sleep(500);
                    timeWaited += 1;
                }
                if (master.GetSocket.Connected == false)
                {
                    return "Failed to Connect, would you like to retry?";
                }
                return "Connected to Server";
            }
            catch
            {
                return "Please enter a valid Server Name";
            }          
        }

        public static void GetData()
        {
            lock (GetMaster)
            {
                ServerConnected.GetSocket.BeginReceive(ServerConnected.GetBuffer, 0, ServerConnected.GetBuffer.Length, SocketFlags.None, recieveInfo, ServerConnected);
            }
        }

        /// <summary>
        /// Sends Data
        /// </summary>
        /// <param name="toSend"></param>
        public static void SendData(string toSend)
        {
            byte[] messagebytes = ASCIIEncoding.UTF8.GetBytes(toSend);
            ServerConnected.GetSocket.BeginSend(messagebytes, 0, messagebytes.Length, SocketFlags.None, SendCallBack, ServerConnected.GetSocket);
        }

        private static void SendCallBack(IAsyncResult aResult)
        {
            Socket newSocket = (Socket)aResult.AsyncState;
            newSocket.EndSend(aResult);
        }

        public static MasterSocket GetMaster { get { return ServerConnected; } }


    }
}
