using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationTotalPrice : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial string Currency { get; set; } = string.Empty;

        [ObservableProperty]
        public partial decimal TotalNetAmount { get; set; }

        [ObservableProperty]
        public partial decimal TotalGrossAmount { get; set; }

        [ObservableProperty]
        public partial decimal TotalTaxAmount { get; set; }
        #endregion
    }
}
