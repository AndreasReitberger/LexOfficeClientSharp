using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationTaxAmount : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial long TaxRatePercentage { get; set; }

        [ObservableProperty]
        public partial decimal TaxAmount { get; set; }

        [ObservableProperty]
        public partial decimal NetAmount { get; set; }
        #endregion
    }
}
