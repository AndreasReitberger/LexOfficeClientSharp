using Newtonsoft.Json;

namespace LexOfficeSharpApi
{
    public partial class LexCustomerNumber
    {
        [JsonProperty("number")]
        public long Number { get; set; }
    }
}
