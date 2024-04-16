using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationTotalPrice : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        string currency = string.Empty;

        [ObservableProperty]
        double totalNetAmount;

        [ObservableProperty]
        double totalGrossAmount;

        [ObservableProperty]
        double totalTaxAmount;
        #endregion
    }
}
