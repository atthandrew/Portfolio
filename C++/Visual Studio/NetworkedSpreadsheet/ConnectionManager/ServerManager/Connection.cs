using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetworkControl;

namespace ServerManager
{
    public partial class Connection : Form
    {

        public Connection()
        {
            InitializeComponent();
            Networking Networking = new Networking();
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            Networking newConnection = new Networking();
            string connectionMessage = Networking.ConnectToServer(ipAddress.Text);
            checkConnection(connectionMessage, newConnection);
        }

        private void checkConnection(string connectionMessage, Networking tryConnection)
        {
            if (connectionMessage == "Connected to Server")
            {
                MessageBox.Show("Connected to Server");
                Networking.SendData("{\"type\": \"Managment\",\"name\": \"test.sprd\",\"username\": \"" + Username.Text + "\",\"password\": \"" + Password.Text +"\"}\n\n");
                this.FindForm().Close();
            }
            else if (connectionMessage == "Failed to Connect, would you like to retry?")
            {
                DialogResult dialogResult = MessageBox.Show("Server not found. Would you like to retry?", "Connection Failed", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    connectionMessage = Networking.ConnectToServer(ipAddress.Text);
                    checkConnection(connectionMessage, tryConnection);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //Do nothing
                }
            }
            else if (connectionMessage == "Please enter a valid Server Name")
            {
                MessageBox.Show("PLease Enter a valid Server Name");
            }
        }
    }
}
