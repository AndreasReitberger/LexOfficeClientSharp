using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexContact : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial Guid Id { get; set; }

        [ObservableProperty]
        public partial Guid OrganizationId { get; set; }

        [ObservableProperty]
        public partial long Version { get; set; }

        [ObservableProperty]
        public partial Dictionary<string, LexCustomerNumber> Roles { get; set; } = [];

        [ObservableProperty]
        public partial LexContactCompany? Company { get; set; }

        [ObservableProperty]
        public partial LexContactPerson? Person { get; set; }

        [ObservableProperty]
        public partial Dictionary<string, List<LexContactAddress>> Addresses { get; set; } = [];

        [ObservableProperty]
        public partial Dictionary<string, List<string>> EmailAddresses { get; set; } = [];

        [ObservableProperty]
        public partial Dictionary<string, List<string>> PhoneNumbers { get; set; } = [];

        [ObservableProperty]
        public partial string Note { get; set; } = string.Empty;

        [ObservableProperty]
        public partial bool Archived { get; set; }
        #endregion
    }
}
