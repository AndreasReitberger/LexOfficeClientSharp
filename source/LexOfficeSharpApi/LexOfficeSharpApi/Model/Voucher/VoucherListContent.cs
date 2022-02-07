using Newtonsoft.Json;
using System;

namespace LexOfficeSharpApi
{
    public partial class VoucherListContent
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("voucherType")]
        public string VoucherType { get; set; }

        [JsonProperty("voucherStatus")]
        public string VoucherStatus { get; set; }

        [JsonProperty("voucherNumber")]
        public string VoucherNumber { get; set; }

        [JsonProperty("voucherDate")]
        public DateTimeOffset VoucherDate { get; set; }

        [JsonProperty("updatedDate")]
        public DateTimeOffset UpdatedDate { get; set; }

        [JsonProperty("dueDate")]
        public DateTimeOffset DueDate { get; set; }

        [JsonProperty("contactName")]
        public string ContactName { get; set; }

        [JsonProperty("totalAmount")]
        public double TotalAmount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }
    }
}
