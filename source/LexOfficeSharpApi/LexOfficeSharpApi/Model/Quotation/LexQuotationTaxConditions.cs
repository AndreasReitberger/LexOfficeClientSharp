using Newtonsoft.Json;

namespace LexOfficeSharpApi
{
    public partial class LexQuotationTaxConditions
    {
        [JsonProperty("taxType")]
        public string TaxType { get; set; }
    }
}
