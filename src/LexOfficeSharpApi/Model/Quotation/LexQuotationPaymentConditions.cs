using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationPaymentConditions
    {
        [JsonProperty("paymentTermLabel")]
        public string PaymentTermLabel { get; set; }

        [JsonProperty("paymentTermDuration")]
        public long PaymentTermDuration { get; set; }

        [JsonProperty("paymentDiscountConditions")]
        public Dictionary<string, long> PaymentDiscountConditions { get; set; }
    }
}
