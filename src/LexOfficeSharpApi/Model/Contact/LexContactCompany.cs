using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexContactCompany : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        string name = string.Empty;

        [ObservableProperty]
        string taxNumber = string.Empty;

        [ObservableProperty]
        string vatRegistrationId = string.Empty;

        [ObservableProperty]
        bool allowTaxFreeInvoices;

        [ObservableProperty]
        List<LexContactPerson> contactPersons = [];
        #endregion
    }
}
