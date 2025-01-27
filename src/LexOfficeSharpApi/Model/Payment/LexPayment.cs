using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexPayment : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial string PaymentItemType { get; set; } = string.Empty;

        [ObservableProperty]
        public partial DateTimeOffset PostingDate { get; set; }

        [ObservableProperty]
        public partial decimal Amount { get; set; }

        [ObservableProperty]
        public partial string Currency { get; set; } = string.Empty;
        #endregion
    }
}
