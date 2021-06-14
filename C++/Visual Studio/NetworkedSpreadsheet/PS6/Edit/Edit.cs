using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Edit
    {
        [JsonProperty(PropertyName = "type")]
        public string Type
        { get; set; }

        [JsonProperty(PropertyName = "cell")]
        public string cell
        { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value
        { get; set; }

        [JsonProperty(PropertyName = "dependencies")]
        public List<string> Dependencies
        { get; set; }
    }
}
