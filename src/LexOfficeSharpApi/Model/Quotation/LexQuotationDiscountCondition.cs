using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationDiscountCondition : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial decimal DiscountPercentage { get; set; }

        [ObservableProperty]
        public partial long DiscountRange { get; set; }

        #endregion
    }
}
