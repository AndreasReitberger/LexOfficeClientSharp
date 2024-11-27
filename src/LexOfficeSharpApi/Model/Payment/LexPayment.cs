using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexPayment : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        string paymentItemType = string.Empty;

        [ObservableProperty]
        DateTimeOffset postingDate;

        [ObservableProperty]
        double amount;

        [ObservableProperty]
        string currency = string.Empty;
        #endregion
    }
}
