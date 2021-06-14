///<author>
///Andrew Thompson & William Meldrum
/// </author>

using Newtonsoft.Json;
using SpaceWars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWars
{
    /// <summary>
    /// Represents a projectile object in Spacewars.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Projectile
    {
        [JsonProperty(PropertyName = "proj")]
        public int ID; //The ID of the projectile

        [JsonProperty]
        public Vector2D loc; //The location of the projectile

        [JsonProperty]
        public Vector2D dir; //The direction of the projectile

        [JsonProperty]
        public bool alive; //Indicates whether a projectile is alive or not.

        [JsonProperty]
        public int owner; //The player ID of the player who shot the projectile.

    }
}
