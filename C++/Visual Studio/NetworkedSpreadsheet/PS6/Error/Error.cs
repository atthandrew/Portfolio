using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Error
    {
        [JsonProperty(PropertyName = "type")]
        public string Type
        { get; set; }

        [JsonProperty(PropertyName = "code")]
        public int Code
        { get; set; }

        [JsonProperty(PropertyName = "source")]
        public string Source
        { get; set; }
    }
}
