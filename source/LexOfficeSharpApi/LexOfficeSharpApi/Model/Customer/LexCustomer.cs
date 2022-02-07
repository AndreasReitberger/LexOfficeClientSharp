using Newtonsoft.Json;

namespace LexOfficeSharpApi
{
    public partial class LexCustomer
    {
        [JsonProperty("number")]
        public long Number { get; set; }
    }
}
