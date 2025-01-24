using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationUnitPrice : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial string Currency { get; set; } = string.Empty;

        [ObservableProperty]
        public partial decimal NetAmount { get; set; }

        [ObservableProperty]
        public partial decimal GrossAmount { get; set; }

        [ObservableProperty]
        public partial long TaxRatePercentage { get; set; }
        #endregion
    }
}
