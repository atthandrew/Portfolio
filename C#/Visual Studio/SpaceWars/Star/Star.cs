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
    /// Represents a star object in Spacewars.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Star
    {
        [JsonProperty(PropertyName = "star")]
        public int ID; //The Star ID

        [JsonProperty]
        public Vector2D loc; //The location of the Star

        [JsonProperty]
        public double mass; //The mass of the Star


    }
}
