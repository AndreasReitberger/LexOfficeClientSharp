using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexShippingConditions : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial DateTimeOffset ShippingDate { get; set; }

        [ObservableProperty]
        public partial DateTimeOffset ShippingEndDate { get; set; }

        [ObservableProperty]
        public partial string ShippingType { get; set; } = string.Empty;
        #endregion
    }
}
