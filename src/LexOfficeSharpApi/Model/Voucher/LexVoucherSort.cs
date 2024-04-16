using Newtonsoft.Json;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexVoucherSort
    {
        [JsonProperty("property", NullValueHandling = NullValueHandling.Ignore)]
        public string Property { get; set; }

        [JsonProperty("direction", NullValueHandling = NullValueHandling.Ignore)]
        public string Direction { get; set; }

        [JsonProperty("ignoreCase", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IgnoreCase { get; set; }

        [JsonProperty("nullHandling", NullValueHandling = NullValueHandling.Ignore)]
        public string NullHandling { get; set; }

        [JsonProperty("ascending", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Ascending { get; set; }
    }
}
