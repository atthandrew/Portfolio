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
    public partial class NewSpreadsheet : Form
    {
        public NewSpreadsheet()
        {
            InitializeComponent();
        }

        private void CreateNew_Click(object sender, EventArgs e)
        {
            Networking.SendData("S" + NameSp.Text + "\n\n");
            this.FindForm().Close();
        }
    }
}
