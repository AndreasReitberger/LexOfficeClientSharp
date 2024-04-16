using Newtonsoft.Json;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationTaxAmount
    {
        [JsonProperty("taxRatePercentage")]
        public long TaxRatePercentage { get; set; }

        [JsonProperty("taxAmount")]
        public double TaxAmountTaxAmount { get; set; }

        [JsonProperty("netAmount")]
        public double NetAmount { get; set; }
    }
}
