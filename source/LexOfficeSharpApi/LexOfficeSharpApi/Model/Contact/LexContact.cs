using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexContact
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("organizationId")]
        public Guid OrganizationId { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }

        [JsonProperty("roles")]
        public Dictionary<string, LexCustomerNumber> Roles { get; set; }

        [JsonProperty("company", NullValueHandling = NullValueHandling.Ignore)]
        public LexContactCompany Company { get; set; }

        [JsonProperty("person", NullValueHandling = NullValueHandling.Ignore)]
        public LexContactPerson Person { get; set; }

        [JsonProperty("addresses")]
        public Dictionary<string, List<LexContactAddress>> Addresses { get; set; }

        [JsonProperty("emailAddresses")]
        public Dictionary<string, List<string>> EmailAddresses { get; set; }

        [JsonProperty("phoneNumbers")]
        public Dictionary<string, List<string>> PhoneNumbers { get; set; }

        [JsonProperty("note", NullValueHandling = NullValueHandling.Ignore)]
        public string Note { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }
    }
}
