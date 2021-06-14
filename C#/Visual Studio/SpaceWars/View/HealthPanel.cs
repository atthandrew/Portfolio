///<author>
///Andrew Thompson & William Meldrum
///</author>

using Controller;
using SpaceWars;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceWars
{
    /// <summary>
    /// A type of DrawingPanel that represents player's names, scores, and current HP.
    /// </summary>
    class HealthPanel : Panel
    {
        private GameController gameController;
        private World theWorld;
        private SolidBrush textBrush;
        private SolidBrush healthBrush;
        private Pen borderPen;

        public HealthPanel(World w, GameController gc)
        {
            gameController = gc;
            this.BackColor = Color.White;
            theWorld = w;
            textBrush = new SolidBrush(Color.Black);
            healthBrush = new SolidBrush(Color.SeaGreen);
            borderPen = new Pen(Color.Black, 2);
            borderPen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset; //Used to draw borders on the rectangles.
            DoubleBuffered = true;
            this.Font = new Font("Comic Sans MS", 14); //Comic Sans is critical to our implementation.
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            lock (theWorld)
            {
                int barHeight = theWorld.dimensions / 30; //Allows the health bars and scores of the top 15 players to be drawn.
                int i = 0; //Used to indicate the position of the rectangle in the panel (0 on top, 30 on bottom).

                //These two methods sort the ships in the dictionary into a list based on their score.
                List<Ship> SortedShips = theWorld.shipDictionary.Values.ToList();
                SortedShips = SortedShips.OrderByDescending(s => s.score).ToList();

                foreach (Ship s in SortedShips)
                {
                    //Draws the name and score of the current ship.
                    Rectangle textRectangle = new Rectangle(0, i * barHeight, this.Width, barHeight);
                    e.Graphics.DrawString(s.name + ": " + s.score, this.Font, textBrush, textRectangle);
                    i++;

                    //Draws the health bar and its border for the current ship.
                    Rectangle healthRectangle = new Rectangle(0, i * barHeight, (int) (this.Width * (s.hp / 5.0)), barHeight);
                    healthColorSelector(s.hp);
                    e.Graphics.FillRectangle(healthBrush, healthRectangle);
                    Rectangle borderRectangle = new Rectangle(0, i * barHeight, this.Width, barHeight);
                    e.Graphics.DrawRectangle(borderPen, borderRectangle);
                    i++;
                }
            }
        }

        private void healthColorSelector(int health)
        {
            switch (health)
            {
                case 1:
                    healthBrush.Color = Color.Red;
                    break;
                case 2:
                    healthBrush.Color = Color.Orange;
                    break;
                case 3:
                    healthBrush.Color = Color.Yellow;
                    break;
                case 4:
                    healthBrush.Color = Color.YellowGreen;
                    break;
                case 5:
                    healthBrush.Color = Color.ForestGreen;
                    break;
            }
        }


    }
}
