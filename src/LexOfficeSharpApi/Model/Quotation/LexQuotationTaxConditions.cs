using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationTaxConditions : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        string taxType = string.Empty;

        #endregion
    }
}
