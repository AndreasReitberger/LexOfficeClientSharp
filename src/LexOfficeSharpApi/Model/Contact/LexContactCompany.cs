using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexContactCompany : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string TaxNumber { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string VatRegistrationId { get; set; } = string.Empty;

        [ObservableProperty]
        public partial bool AllowTaxFreeInvoices { get; set; }

        [ObservableProperty]
        public partial List<LexContactPerson> ContactPersons { get; set; } = [];
        #endregion
    }
}
