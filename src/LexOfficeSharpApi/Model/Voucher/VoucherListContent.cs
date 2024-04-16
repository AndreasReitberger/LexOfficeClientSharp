using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.LexOffice
{
    public partial class VoucherListContent : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        Guid id;

        [ObservableProperty]
        string voucherType = string.Empty;

        [ObservableProperty]
        string voucherStatus = string.Empty;

        [ObservableProperty]
        string voucherNumber = string.Empty;

        [ObservableProperty]
        DateTimeOffset voucherDate;

        [ObservableProperty]
        DateTimeOffset updatedDate;

        [ObservableProperty]
        DateTimeOffset dueDate;

        [ObservableProperty]
        string contactName = string.Empty;

        [ObservableProperty]
        double totalAmount;

        [ObservableProperty]
        string currency = string.Empty;

        [ObservableProperty]
        bool archived;
        #endregion
    }
}
