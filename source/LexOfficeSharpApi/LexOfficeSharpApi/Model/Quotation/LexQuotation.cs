using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotation
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("organizationId")]
        public Guid OrganizationId { get; set; }

        [JsonProperty("createdDate")]
        public DateTimeOffset CreatedDate { get; set; }

        [JsonProperty("updatedDate")]
        public DateTimeOffset UpdatedDate { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("voucherStatus")]
        public string VoucherStatus { get; set; }

        [JsonProperty("voucherNumber")]
        public string VoucherNumber { get; set; }

        [JsonProperty("voucherDate")]
        public DateTimeOffset VoucherDate { get; set; }

        [JsonProperty("expirationDate")]
        public DateTimeOffset ExpirationDate { get; set; }

        [JsonProperty("address")]
        public LexContactAddress Address { get; set; }

        [JsonProperty("lineItems")]
        public List<LexQuotationItem> LineItems { get; set; }

        [JsonProperty("totalPrice")]
        public LexQuotationTotalPrice TotalPrice { get; set; }

        [JsonProperty("taxAmounts")]
        public List<LexQuotationTaxAmount> TaxAmounts { get; set; }

        [JsonProperty("taxConditions")]
        public LexQuotationTaxConditions TaxConditions { get; set; }
        //public Dictionary<string, string> TaxConditions { get; set; }

        [JsonProperty("paymentConditions")]
        public LexQuotationPaymentConditions PaymentConditions { get; set; }

        [JsonProperty("introduction")]
        public string Introduction { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("files")]
        public LexQuotationFiles Files { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    } 
}
