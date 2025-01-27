using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexPayments : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial decimal OpenAmount { get; set; }

        [ObservableProperty]
        public partial string PaymentStatus { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Currency { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string VoucherType { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string VoucherStatus { get; set; } = string.Empty;

        [ObservableProperty]
        public partial DateTimeOffset PaidDate { get; set; }

        [ObservableProperty]
        public partial List<LexPayment> PaymentItems { get; set; } = [];
        #endregion
    }
}
