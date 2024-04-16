using Newtonsoft.Json;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationTaxConditions
    {
        [JsonProperty("taxType")]
        public string TaxType { get; set; }
    }
}
