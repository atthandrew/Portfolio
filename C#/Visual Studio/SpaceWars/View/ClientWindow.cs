///<author>
///Andrew Thompson & William Meldrum
///</author>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controller;
using Network;
using SpaceWars;

namespace SpaceWars
{

    /// <summary>
    /// A Form that handles the view for SpaceWars.
    /// </summary>
    public partial class ClientWindow : Form
    {
        private GameController gameController; //The associated controller for the view.

        DrawingPanel drawingPanel; //Used to draw the play area.
        HealthPanel healthBars; //The scoreboard.
        World theWorld; //The model.

        public ClientWindow(GameController gc)
        {
            InitializeComponent();
            gameController = gc;
            theWorld = gameController.GetWorld();

            ClientSize = new Size(700, 500); //Default size for the client.

            drawingPanel = new DrawingPanel(theWorld, gameController);
            healthBars = new HealthPanel(theWorld, gameController);

            //Creates the DrawingPanel and HealthPanel
            drawingPanel.Size = new Size(500, 500);
            healthBars.Size = new Size(200, 500);
            drawingPanel.Location = new Point(0, 50);
            healthBars.Location = new Point(drawingPanel.Width, 50);
            this.Controls.Add(drawingPanel);
            this.Controls.Add(healthBars);

            //Registers this Form's handlers.
            gameController.RedrawHandler += DrawFrame;
            gameController.ResizeHandler += ResizeForm;
            gameController.ErrorHandler += ConnectionError;
        }

        /// <summary>
        /// Calls the connect button handler in gameController, which begins the connection process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            //Disables the text boxes while connecting/connected, since the Network Protocol specifies we can only send our name/server
            //when initially connecting.
            connectButton.Enabled = false;
            serverTextBox.Enabled = false;
            nameTextBox.Enabled = false;

            gameController.ConnectButtonPressed(serverTextBox.Text, nameTextBox.Text);

            //Prepares to receive keyboard input.
            drawingPanel.Focus();
        }

        /// <summary>
        /// Handler for the redraw event. Invalidates the form and it's associated panels to redraw.
        /// </summary>
        public void DrawFrame()
        {
            if (!IsHandleCreated)
                return;

            MethodInvoker invalid = new MethodInvoker(() => this.Invalidate(true));
            try
            {
                this.Invoke(invalid);
            }
            catch (System.ObjectDisposedException)
            {
                //Prevents the ClientWindow from being accessed after closing the window
            }
        }

        /// <summary>
        /// Handler for the resize event. Called after receiving the new World dimensions.
        /// </summary>
        public void ResizeForm()
        {
            MethodInvoker invoker = new MethodInvoker(() =>
            {
                ClientSize = new Size(theWorld.dimensions + 200, theWorld.dimensions + 50); //+200 to accommodate the health bars, +50 for the offset of both panels
                drawingPanel.Size = new Size(theWorld.dimensions, theWorld.dimensions);
                healthBars.Location = new Point(drawingPanel.Width, 50);
                healthBars.Size = new Size(200, theWorld.dimensions);
            });

            this.Invoke(invoker);
        }

        /// <summary>
        /// Handler for errors encountered when trying to connect. Displays a yes/no message box that allows a user to try to reconnect if yes,
        /// and closes the Form if no.
        /// </summary>
        /// <param name="e"></param>
        public void ConnectionError(Exception e)
        {
            MethodInvoker invoker = new MethodInvoker(() =>
            {
                if(MessageBox.Show("Could not connect, received error message: " + e.Message + ".\n Do you want to reconnect?", "Connection error", MessageBoxButtons.YesNo)
                == DialogResult.Yes)
                {
                    connectButton.Enabled = true;
                    serverTextBox.Enabled = true;
                    nameTextBox.Enabled = true;
                }
                else
                {
                    this.Close();
                }
            });

            this.Invoke(invoker);
        }
    }
}
