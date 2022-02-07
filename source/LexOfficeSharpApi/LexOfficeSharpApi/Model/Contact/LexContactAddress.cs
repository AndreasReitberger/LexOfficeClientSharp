using Newtonsoft.Json;

namespace LexOfficeSharpApi
{
    public partial class LexContactAddress
    {
        [JsonProperty("supplement")]
        public string Supplement { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("zip")]
        public string Zip { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }
    }
}
