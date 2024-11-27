using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexPayments : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        double openAmount;

        [ObservableProperty]
        string paymentStatus = string.Empty;

        [ObservableProperty]
        string currency = string.Empty;

        [ObservableProperty]
        string voucherType = string.Empty;

        [ObservableProperty]
        string voucherStatus = string.Empty;

        [ObservableProperty]
        DateTimeOffset paidDate;

        [ObservableProperty]
        List<LexPayment> paymentItems = [];
        #endregion
    }
}
