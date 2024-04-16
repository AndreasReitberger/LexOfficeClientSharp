using Newtonsoft.Json;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationTotalPrice
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("totalNetAmount")]
        public double TotalNetAmount { get; set; }

        [JsonProperty("totalGrossAmount")]
        public double TotalGrossAmount { get; set; }

        [JsonProperty("totalTaxAmount")]
        public double TotalTaxAmount { get; set; }
    }
}
