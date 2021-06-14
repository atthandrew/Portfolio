using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using NetworkControl;


namespace ServerManager
{
 
    public partial class Main : Form
    {

        public Main()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Runs the Server Code once connection is made
        /// </summary>
        private void runServer()
        {
            int x = 1;
            bool connected = false;
            
            //Every 15 seconds update the lists
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(resetLists);
            timer.Interval = 5000;
            timer.Enabled = true;
            timer.Start();

            while (x == 1)
            {
                if (Networking.isConnected == true)
                {
                    connected = true;
                    //Networking.GetData();
                    ServerStatus.Invoke(new MethodInvoker(delegate { ServerStatus.Text = "Server Status: Connected"; }));
                    try
                    {
                        proccessData();
                        
                        //Checks if the Socket is still alive
                        bool check1 = Networking.GetMaster.GetSocket.Poll(1000, SelectMode.SelectRead);
                        bool check2 = (Networking.GetMaster.GetSocket.Available == 0);
                        if (check1 && check2)
                            Networking.isConnected = false;
                        else
                            Networking.isConnected = true;
                    }
                    catch
                    {
                        ServerStatus.Invoke(new MethodInvoker(delegate { ServerStatus.Text = "Server Status: Not Connected"; }));
                        if (connected == true)
                        {
                            Application.Exit();
                        }
                    }
                }
                else
                {
                    try
                    {
                        ServerStatus.Invoke(new MethodInvoker(delegate { ServerStatus.Text = "Server Status: Not Connected"; }));
                        if (connected == true)
                        {
                            Application.Exit();
                        }
                    }
                    catch
                    {
                        //Do nothing
                    }
                }
            }
        }

        private void resetLists(object sender, ElapsedEventArgs e)
        {
            ServerStatus.Invoke(new MethodInvoker(delegate { Spreadsheets.Items.Clear(); }));
            ServerStatus.Invoke(new MethodInvoker(delegate { Users.Items.Clear(); }));
        }

        private void proccessData()
        {
            //Determines if this is a Spreadsheet or a User
            //Populates lists accordingly 
            try
            {
                StringBuilder dataIN;
                dataIN = Networking.GetMaster.GetFullMessage();
                if (dataIN.ToString() == "")
                {
                    //Do nothing
                }
                else
                {
                    string[] splitMessages = dataIN.ToString().Split('\n');
                    foreach (string x in splitMessages)
                    {
                        if (x[0] == 'S' && x[x.Length -1] == ';')
                        {
                            string[] listOfSheets = x.Split(',');
                            //ServerStatus.Invoke(new MethodInvoker(delegate { Spreadsheets.Items.Clear(); }));
                            foreach (string sheetIN in listOfSheets)
                            {
                                if (sheetIN == "S" || sheetIN == "" || sheetIN == ";" || sheetIN.Contains(".sprd") != true)
                                {
                                    //Don't add
                                }
                                else
                                {
                                    if (Spreadsheets.Items.Contains(sheetIN))
                                    {
                                        //Do nothing
                                    }
                                    else
                                    {
                                        ServerStatus.Invoke(new MethodInvoker(delegate { Spreadsheets.Items.Add(sheetIN); }));
                                    }
                                }
                            }
                            Spreadsheets.Sorted = true;
                        }
                        if (x[0] == 'U' && x[x.Length - 1] == ';')
                        {
                            string[] listOfUsers = x.Split(',');
                            //ServerStatus.Invoke(new MethodInvoker(delegate { Users.Items.Clear(); }));
                            foreach (string userIN in listOfUsers)
                            {
                                if (userIN == "U" || userIN == "" || userIN.Contains(".sprd") || userIN == ";")
                                {

                                }
                                else
                                {
                                    if (Users.Items.Contains(userIN))
                                    {
                                        //Do nothing
                                    }
                                    else
                                    {
                                        ServerStatus.Invoke(new MethodInvoker(delegate { Users.Items.Add(userIN); }));
                                    }
                                }
                            }
                            Users.Sorted = true;
                        }
                        else
                        {
                            //Do nothing, we don't like these messages
                        }
                    }
                }
            }
            catch
            {
                //Do nothing
            }
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            ServerManager.Connection newConnection = new Connection();
            newConnection.Show();
            System.Threading.Thread run = new System.Threading.Thread(runServer);
            run.Start();
        }

        /// <summary>
        /// Creates new user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewUser_Click(object sender, EventArgs e)
        {
            if (ServerStatus.Text == "Server Status: Not Connected")
            {
                DialogResult dialogResult = MessageBox.Show("Not Connected to Server", "Error", MessageBoxButtons.OK);
            }
            else
            {
                NewUser newUser = new NewUser();
                newUser.Show();
            }
        }

        /// <summary>
        /// Creates a new Spreadsheet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewSpreadsheet_Click(object sender, EventArgs e)
        {
            if (ServerStatus.Text == "Server Status: Not Connected")
            {
                DialogResult dialogResult = MessageBox.Show("Not Connected to Server", "Error", MessageBoxButtons.OK);
            }
            else
            {
                NewSpreadsheet newSheet = new NewSpreadsheet();
                newSheet.Show();
            }
        }

        /// <summary>
        /// Turns off Server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TurnOffServer_Click(object sender, EventArgs e)
        {
            if (ServerStatus.Text == "Server Status: Not Connected")
            {
                DialogResult dialogResult = MessageBox.Show("Not Connected to Server", "Error", MessageBoxButtons.OK);
            }
            else
            {
                Networking.SendData("TurnOffServer\n\n");
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void Users_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string UserName = Users.SelectedItem.ToString();
                ServerManager.ModifyUser newModify = new ModifyUser(UserName);
                newModify.Show();
            }
            catch
            {

            }
        }
    }
}
