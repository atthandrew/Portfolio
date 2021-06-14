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
    public partial class ModifyUser : Form
    {
        private string name;
        public ModifyUser(string NameIN)
        {
            name = NameIN;
            InitializeComponent();
            UserName.Text = NameIN;
        }

        private void CreateNew_Click(object sender, EventArgs e)
        {
            Networking.SendData("U" + name + "," + Password.Text + "\n\n");
            this.FindForm().Close();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            Networking.SendData("D" + name + "\n\n");
            this.FindForm().Close();
        }
    }
}
