using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexVoucherList
    {
        [JsonProperty("content")]
        public List<VoucherListContent> Content { get; set; }

        [JsonProperty("first")]
        public bool First { get; set; }

        [JsonProperty("last")]
        public bool Last { get; set; }

        [JsonProperty("totalPages")]
        public long TotalPages { get; set; }

        [JsonProperty("totalElements")]
        public long TotalElements { get; set; }

        [JsonProperty("numberOfElements")]
        public long NumberOfElements { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("number")]
        public long Number { get; set; }

        [JsonProperty("sort")]
        public List<LexVoucherSort> Sort { get; set; }
    }
}
