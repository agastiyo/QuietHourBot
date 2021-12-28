using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace QuietHourBot {
    public struct Configjson {

        [JsonProperty("token")]
        public string Token { get; private set; }

        [JsonProperty("Prefix")]
        public string Prefix { get; private set; }
    }
}