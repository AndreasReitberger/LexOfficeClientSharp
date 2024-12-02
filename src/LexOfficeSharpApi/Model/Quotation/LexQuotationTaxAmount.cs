using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationTaxAmount : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        long taxRatePercentage;

        [ObservableProperty]
        decimal taxAmount;

        [ObservableProperty]
        decimal netAmount;
        #endregion
    }
}
