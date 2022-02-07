using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LexOfficeSharpApi
{
    public partial class LexQuotationItem
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("unitName")]
        public string UnitName { get; set; }

        [JsonProperty("unitPrice")]
        public LexQuotationUnitPrice UnitPrice { get; set; }

        [JsonProperty("discountPercentage")]
        public long DiscountPercentage { get; set; }

        [JsonProperty("lineItemAmount")]
        public double LineItemAmount { get; set; }

        [JsonProperty("subItems", NullValueHandling = NullValueHandling.Ignore)]
        public List<LexQuotationItem> SubItems { get; set; }

        [JsonProperty("alternative")]
        public bool Alternative { get; set; }

        [JsonProperty("optional")]
        public bool Optional { get; set; }
    }
}
