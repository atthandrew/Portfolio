///<author>
///Andrew Thompson & William Meldrum
///</author>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using SpaceWars;
using Controller;

namespace SpaceWars
{
    /// <summary>
    /// A type of panel used by the view to draw the world of Spacewars.
    /// </summary>
    class DrawingPanel : Panel
    {
        private World theWorld;
        private GameController gameController;

        public DrawingPanel(World w, GameController gc)
        {            
            DoubleBuffered = true;
            theWorld = w;
            this.BackColor = Color.Black;
            this.gameController = gc;
        }

        /// <summary>
        /// Helper method for DrawObjectWithTransform
        /// </summary>
        /// <param name="size">The world (and image) size</param>
        /// <param name="w">The worldspace coordinate</param>
        /// <returns></returns>
        private static int WorldSpaceToImageSpace(int size, double w)
        {
            return (int)w + size / 2;
        }

        /// <summary>
        /// Draws a ship using an appropriate image from the Resources file.
        /// </summary>
        /// <param name="o">The ship as an object</param>
        /// <param name="e">The event causing the redraw</param>
        private void ShipDrawer(object o, PaintEventArgs e)
        {
            int shipWidth = 35;
            Ship s = o as Ship;

            if (s.hp > 0) //Only draws the ship if it is alive.
            {
                String shipSpriteFileName = "ship-coast-";

                switch (s.ID % 8)
                {
                    case 0:
                        shipSpriteFileName += "red.png";
                        break;
                    case 1:
                        shipSpriteFileName += "blue.png";
                        break;
                    case 2:
                        shipSpriteFileName += "green.png";
                        break;
                    case 3:
                        shipSpriteFileName += "yellow.png";
                        break;
                    case 4:
                        shipSpriteFileName += "violet.png";
                        break;
                    case 5:
                        shipSpriteFileName += "white.png";
                        break;
                    case 6:
                        shipSpriteFileName += "grey.png";
                        break;
                    case 7:
                        shipSpriteFileName += "brown.png";
                        break;
                }
                Image shipSprite = Image.FromFile("../../../Resources/Images/" + shipSpriteFileName);
                e.Graphics.DrawImage(shipSprite, -(shipWidth / 2), -(shipWidth / 2), shipWidth, shipWidth);
            }
        }

        /// <summary>
        /// Draws a ship using an image from the Resources file.
        /// </summary>
        /// <param name="o">The star as an object</param>
        /// <param name="e">The event causing the redraw</param>
        private void StarDrawer(object o, PaintEventArgs e)
        {
            int starWidth = 50;
            Star s = o as Star;

            Image shipSprite = Image.FromFile("../../../Resources/Images/star.jpg");
            Rectangle r = new Rectangle(new Point(-(starWidth /2) , -(starWidth /2) ), new Size(starWidth, starWidth));
            e.Graphics.DrawImage(shipSprite, r);
        }

        /// <summary>
        /// Draws a projectile using an image from the Resources file.
        /// </summary>
        /// <param name="o">The projectile as an object</param>
        /// <param name="e">The event causing the redraw</param>
        private void ProjectileDrawer(object o, PaintEventArgs e)
        {
            int projectileWidth = 25;

            Projectile p = o as Projectile;

            String projectileSpriteFileName = "shot-";

            switch (p.owner % 8)
            {
                case 0:
                    projectileSpriteFileName += "red.png";
                    break;
                case 1:
                    projectileSpriteFileName += "blue.png";
                    break;
                case 2:
                    projectileSpriteFileName += "green.png";
                    break;
                case 3:
                    projectileSpriteFileName += "yellow.png";
                    break;
                case 4:
                    projectileSpriteFileName += "violet.png";
                    break;
                case 5:
                    projectileSpriteFileName += "white.png";
                    break;
                case 6:
                    projectileSpriteFileName += "grey.png";
                    break;
                case 7:
                    projectileSpriteFileName += "brown.png";
                    break;
            }

            Image projectileSprite = Image.FromFile("../../../Resources/Images/" + projectileSpriteFileName);
            Rectangle r = new Rectangle(new Point(-(projectileWidth / 2), -(projectileWidth / 2)), new Size(projectileWidth, projectileWidth));
            e.Graphics.DrawImage(projectileSprite, r);
        }



        // A delegate for DrawObjectWithTransform
        // Methods matching this delegate can draw whatever they want using e  
        public delegate void ObjectDrawer(object o, PaintEventArgs e);


        /// <summary>
        /// This method performs a translation and rotation to drawn an object in the world.
        /// </summary>
        /// <param name="e">PaintEventArgs to access the graphics (for drawing)</param>
        /// <param name="o">The object to draw</param>
        /// <param name="worldSize">The size of one edge of the world (assuming the world is square)</param>
        /// <param name="worldX">The X coordinate of the object in world space</param>
        /// <param name="worldY">The Y coordinate of the object in world space</param>
        /// <param name="angle">The orientation of the objec, measured in degrees clockwise from "up"</param>
        /// <param name="drawer">The drawer delegate. After the transformation is applied, the delegate is invoked to draw whatever it wants</param>
        private void DrawObjectWithTransform(PaintEventArgs e, object o, int worldSize, double worldX, double worldY, double angle, ObjectDrawer drawer)
        {
            // Perform the transformation
            int x = WorldSpaceToImageSpace(worldSize, worldX);
            int y = WorldSpaceToImageSpace(worldSize, worldY);
            e.Graphics.TranslateTransform(x, y);
            e.Graphics.RotateTransform((float)angle);
            // Draw the object 
            drawer(o, e);
            // Then undo the transformation
            e.Graphics.ResetTransform();
        }


        /// <summary>
        /// This event handler redraws all ships, stars, and projectiles in the World.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            lock (theWorld)
            {
                foreach (Star star in theWorld.starDictionary.Values)
                {
                    DrawObjectWithTransform(e, star, theWorld.dimensions, star.loc.GetX(), star.loc.GetY(), 0, StarDrawer);
                }

                foreach (Projectile p in theWorld.projectileDictionary.Values)
                {
                    DrawObjectWithTransform(e, p, theWorld.dimensions, p.loc.GetX(), p.loc.GetY(), p.dir.ToAngle(), ProjectileDrawer);
                }

                foreach (Ship s in theWorld.shipDictionary.Values)
                {
                    DrawObjectWithTransform(e, s, theWorld.dimensions, s.loc.GetX(), s.loc.GetY(), s.dir.ToAngle(), ShipDrawer);
                }
            }

            base.OnPaint(e);
        }

        /// <summary>
        /// Allows the left, up, right, and space keys to be used as input.
        /// </summary>
        /// <param name="keyData">The Key being examined</param>
        /// <returns>True if they key is accepted as input</returns>
        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Right || keyData == Keys.Left || keyData == Keys.Up || keyData == Keys.Space)
            {
                return true;
            }
            else
            {
                return base.IsInputKey(keyData);
            }
        }

        /// <summary>
        /// Monitors when the up, left, down, and right keys are pressed, telling the GameController when they are.
        /// </summary>
        /// <param name="e">Event telling when keys are down</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Right || e.KeyData == Keys.Left || e.KeyData == Keys.Up || e.KeyData == Keys.Space)
            {
                gameController.SendKeyToServer(e);
            }
            else
            {
                base.OnKeyDown(e);
            }

        }

        /// <summary>
        /// Monitors when the up, left, down, and right keys are not being pressed, telling the GameController when they are not.
        /// </summary>
        /// <param name="e">Event telling when keys are up</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Right || e.KeyData == Keys.Left || e.KeyData == Keys.Up || e.KeyData == Keys.Space)
            {
                gameController.KeyReleaseHandler(e);
            }
            else
            {
                base.OnKeyUp(e);
            }

        }

    }
}
