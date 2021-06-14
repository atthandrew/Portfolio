using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SpreadsheetList
    {
        [JsonProperty(PropertyName = "type")]
        public string Type
        { get; set; }

        [JsonProperty(PropertyName = "spreadsheets")]
        public List<string> Spreadsheets
        { get; set; }
    }
}
