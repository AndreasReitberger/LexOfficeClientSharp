using Newtonsoft.Json;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexCustomer
    {
        [JsonProperty("number")]
        public long Number { get; set; }
    }
}
