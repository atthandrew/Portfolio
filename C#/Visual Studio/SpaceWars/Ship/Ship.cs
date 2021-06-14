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
    /// Represents a ship object in Spacewars.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Ship
    {
        [JsonProperty(PropertyName = "ship")]
        public int ID; //The ID of the ship

        [JsonProperty]
        public string name; //The player name

        [JsonProperty]
        public Vector2D loc; //Ship location

        [JsonProperty]
        public Vector2D dir; //Ship direction

        [JsonProperty]
        public bool thrust; //Whether or not the ship has thrust engaged

        [JsonProperty]
        public int hp; //The health of the ship

        [JsonProperty]
        public int score; //The player score
    }
}
