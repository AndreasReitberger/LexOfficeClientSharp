using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationFiles
    {
        [JsonProperty("documentFileId")]
        public Guid DocumentFileId { get; set; }
    }
}
