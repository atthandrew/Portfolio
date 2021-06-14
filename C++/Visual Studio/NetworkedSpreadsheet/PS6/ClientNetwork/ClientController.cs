using Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpreadsheetGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static Network.SocketState;

namespace Controller
{
    public class ClientController
    {
        private Socket theServer;
        private SocketState socketState;
        public string username;
        public List<string> ssList;
        public delegate void handler();
        public event handler connected;
        public event handler receivedLists;
        public event handler receivedPWError;
        public event handler receivedCells;
        public event handler receivedCircularError;
        public event handler noServerResponse;
        public Dictionary<string, string> cellValues;

        public void ConnectButtonPressed(string user, string password, string server, string filename)
        {
            username = user;
            NetworkAction na = HandleFirstContact;


            try
            {
                theServer = NetworkController.ConnectToServer(na, server);

            }
            catch (Exception)
            {

            }
        }

        public void OpenButtonPressed(string user, string password, string filename)
        {
            string messageToServer;

            OpenMessage message = new OpenMessage();
            message.Type = "open";
            message.Name = filename;
            message.Password = password;
            message.Username = user;

            messageToServer = JsonConvert.SerializeObject(message) + "\n" + "\n";
            SendAndCheck(theServer, messageToServer);
        }

        public void HandleFirstContact(SocketState state)
        {
            state.callMe = ReceiveStartup;
            theServer = state.socket;
            ConnectedToServer();
            NetworkController.GetData(state);
        }

        public void ReceiveStartup(SocketState ss)
        {
            JObject obj = JObject.Parse(ss.currentMessage);

            JToken token = obj["spreadsheets"];

            if (token != null)
            {
                SpreadsheetList list = JsonConvert.DeserializeObject<SpreadsheetList>(ss.currentMessage);
                ssList = list.Spreadsheets;
                SendListsToView();
            }

            Console.WriteLine(ss.currentMessage.ToString());
            ss.callMe = ReceiveSpreadsheet;
            //socketState = ss;
        }

        private void SendListsToView()
        {
            receivedLists();
        }

        public void ReceiveSpreadsheet(SocketState ss)
        {
            JObject obj = JObject.Parse(ss.currentMessage);

            JToken errorToken = obj["code"];
            JToken spreadsheetToken = obj["spreadsheet"];

            //if pwerror
            if (errorToken != null)
            {
                Error error = JsonConvert.DeserializeObject<Error>(ss.currentMessage);

                //1 is password error
                if (error.Code == 1)
                {
                    SendPWErrorToView();
                }
                //2 is circular dependency
                else if (error.Code == 2)
                {
                    ReceivedCircularError();
                }

            }
            //if full send
            if (spreadsheetToken != null)
            {
                Console.WriteLine("Found a spreadsheet");
                FullSend fullSend = JsonConvert.DeserializeObject<FullSend>(ss.currentMessage);
                cellValues = fullSend.Spreadsheet;
                SendNewCellsToView();
            }
        }

        private void SendPWErrorToView()
        {
            receivedPWError();
        }

        private void SendNewCellsToView()
        {
            receivedCells();
        }

        public void EditMade(string cellName, string value, List<string> dependencies)
        {
            string messageToServer;

            Edit message = new Edit();
            message.Type = "edit";
            message.cell = cellName;
            message.Value = value;
            message.Dependencies = dependencies;

            messageToServer = JsonConvert.SerializeObject(message) + "\n" + "\n";
            SendAndCheck(theServer, messageToServer);
        }

        public void UndoMade()
        {
            string messageToServer;

            Undo message = new Undo();
            message.Type = "undo";

            messageToServer = JsonConvert.SerializeObject(message) + "\n" + "\n";
            SendAndCheck(theServer, messageToServer);
        }

        public void RevertMade(string cellName)
        {
            string messageToServer;

            Revert message = new Revert();
            message.Type = "revert";
            message.Cell = cellName;

            messageToServer = JsonConvert.SerializeObject(message) + "\n" + "\n";
            SendAndCheck(theServer, messageToServer);
        }

        public void SendAndCheck(Socket socket, string message)
        {
            bool connected = NetworkController.Send(socket, message);
            if (!connected)
            {
                NoResponseFromServer();
            }
        }

        public void ReceivedCircularError()
        {
            receivedCircularError();
        }

        public void NoResponseFromServer()
        {
            noServerResponse();
        }

        public void ConnectedToServer()
        {
            connected();
        }
    }
}
