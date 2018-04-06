using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSharing.File.IO
{
    /// <summary>
    /// Container class for storing 
    /// </summary>
    public class FileIOJson
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("expiry")]
        public string Expiry { get; set; }

        public static FileIOJson FromJson(string json) => JsonConvert.DeserializeObject<FileIOJson>(json);

    }

}
