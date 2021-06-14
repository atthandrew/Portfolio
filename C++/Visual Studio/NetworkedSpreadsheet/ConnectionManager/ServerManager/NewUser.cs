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
    public partial class NewUser : Form
    {
        public NewUser()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sends new User Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNew_Click(object sender, EventArgs e)
        {
            Networking.SendData("U" + Username.Text + "," + Password.Text + "\n\n");
            this.FindForm().Close();
        }
    }
}
