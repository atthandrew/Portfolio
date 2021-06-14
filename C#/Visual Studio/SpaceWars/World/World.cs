///<author>
///Andrew Thompson & William Meldrum
///</author>

using SpaceWars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWars
{
    /// <summary>
    /// Main part of the model for SpaceWars. Stores all ships, projectiles, and stars in dictionaries with their IDs as keys.
    /// Also keeps track of the dimensions at which the DrawingPanel should be set
    /// </summary>
    public class World
    {
        public Dictionary<int, Ship> shipDictionary = new Dictionary<int, Ship>();
        public Dictionary<int, Star> starDictionary = new Dictionary<int, Star>();
        public Dictionary<int, Projectile> projectileDictionary = new Dictionary<int, Projectile>();
        public int dimensions;
        public World()
        {
            dimensions = 500;
        }
    }
}
