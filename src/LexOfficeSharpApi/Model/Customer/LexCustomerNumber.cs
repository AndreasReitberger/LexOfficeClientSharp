using Newtonsoft.Json;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexCustomerNumber
    {
        [JsonProperty("number")]
        public long Number { get; set; }
    }
}
