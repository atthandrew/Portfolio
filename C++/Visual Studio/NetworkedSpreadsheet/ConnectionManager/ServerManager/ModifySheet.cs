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
    public partial class ModifySheet : Form
    {
        private string name;

        public ModifySheet(string nameIN)
        {
            name = nameIN;
            InitializeComponent();
            label2.Text = nameIN;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Delete_Click(object sender, EventArgs e)
        {
            Networking.SendData("X" + name + "\n\n");
            this.FindForm().Close();
        }
    }
}
