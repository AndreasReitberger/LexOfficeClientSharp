using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexShippingConditions : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        DateTimeOffset shippingDate;

        [ObservableProperty]
        DateTimeOffset shippingEndDate;

        [ObservableProperty]
        string shippingType = string.Empty;
        #endregion
    }
}
