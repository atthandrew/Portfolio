using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    [JsonObject(MemberSerialization.OptIn)]
    public class OpenMessage
    {
        [JsonProperty(PropertyName = "type")]
        public string Type
        { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name
        { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username
        { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password
        { get; set; }
    }
}
