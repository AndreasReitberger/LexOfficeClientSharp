using Newtonsoft.Json;
using System;

namespace LexOfficeSharpApi
{
    public partial class LexQuotationFiles
    {
        [JsonProperty("documentFileId")]
        public Guid DocumentFileId { get; set; }
    }
}
