using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationUnitPrice : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        string currency = string.Empty;

        [ObservableProperty]
        decimal netAmount;

        [ObservableProperty]
        decimal grossAmount;

        [ObservableProperty]
        long taxRatePercentage;
        #endregion
    }
}
