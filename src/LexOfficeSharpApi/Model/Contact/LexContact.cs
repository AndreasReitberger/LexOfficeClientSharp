using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexContact : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        Guid id;

        [ObservableProperty]
        Guid organizationId;

        [ObservableProperty]
        long version;

        [ObservableProperty]
        Dictionary<string, LexCustomerNumber> roles = [];

        [ObservableProperty]
        LexContactCompany? company;

        [ObservableProperty]
        LexContactPerson? person;

        [ObservableProperty]
        Dictionary<string, List<LexContactAddress>> addresses = [];

        [ObservableProperty]
        Dictionary<string, List<string>> emailAddresses = [];

        [ObservableProperty]
        Dictionary<string, List<string>> phoneNumbers = [];

        [ObservableProperty]
        string note = string.Empty;

        [ObservableProperty]
        bool archived;
        #endregion
    }
}
