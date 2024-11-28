using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationDiscountCondition : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        double discountPercentage;

        [ObservableProperty]
        long discountRange;

        #endregion
    }
}
