using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace NetworkControl
{
    public class MasterSocket
    {
        private static Socket Master;
        private byte[] buffer = new byte[8000];
        private StringBuilder unProcessedMessage;
        public delegate void Callback(MasterSocket tocallBack);

        public MasterSocket(Socket theMaster)
        {
            Master = theMaster;
            unProcessedMessage = new StringBuilder();
        }

        public Socket GetSocket { get { return Master; } }

        public byte[] GetBuffer { get { return buffer; } }

        public void AppendMessage(string toAppend)
        {
            lock (Master)
            {
                if (toAppend.Length > 0)
                {
                    unProcessedMessage.Append(toAppend);
                }
                else
                { 
                }                
            }
        }

        public StringBuilder GetFullMessage()
        {
            lock (Master)
            {
                if (unProcessedMessage == null)
                {
                    return new StringBuilder();
                }
                StringBuilder toprocess = new StringBuilder();
                toprocess.Append(unProcessedMessage);
                unProcessedMessage.Clear();

                return toprocess;
            }
        }

        public void DisconnectFromServer()
        {          
            Master.Shutdown(SocketShutdown.Both);
            System.Threading.Thread.Sleep(500);
            Master.Close();
        }
        
        
        
    }
}
