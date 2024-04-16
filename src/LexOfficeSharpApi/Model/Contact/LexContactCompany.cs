using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexContactCompany
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("taxNumber")]
        public string TaxNumber { get; set; }

        [JsonProperty("vatRegistrationId")]
        public string VatRegistrationId { get; set; }

        [JsonProperty("allowTaxFreeInvoices")]
        public bool AllowTaxFreeInvoices { get; set; }

        [JsonProperty("contactPersons")]
        public List<LexContactPerson> ContactPersons { get; set; }
    }
}
