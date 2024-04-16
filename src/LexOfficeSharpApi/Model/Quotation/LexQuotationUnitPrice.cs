using Newtonsoft.Json;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationUnitPrice
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("netAmount")]
        public double NetAmount { get; set; }

        [JsonProperty("grossAmount")]
        public double GrossAmount { get; set; }

        [JsonProperty("taxRatePercentage")]
        public long TaxRatePercentage { get; set; }
    }
}
