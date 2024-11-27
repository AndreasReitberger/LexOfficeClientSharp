using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationTotalPrice : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        string currency = string.Empty;

        [ObservableProperty]
        decimal totalNetAmount;

        [ObservableProperty]
        decimal totalGrossAmount;

        [ObservableProperty]
        decimal totalTaxAmount;
        #endregion
    }
}
