using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationItem : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        Guid? id;

        [ObservableProperty]
        string type = string.Empty;

        [ObservableProperty]
        string name = string.Empty;

        [ObservableProperty]
        string description = string.Empty;

        [ObservableProperty]
        long quantity;

        [ObservableProperty]
        string unitName = string.Empty;

        [ObservableProperty]
        LexQuotationUnitPrice? unitPrice;

        [ObservableProperty]
        long discountPercentage;

        [ObservableProperty]
        double lineItemAmount;

        [ObservableProperty]
        List<LexQuotationItem> subItems = [];

        [ObservableProperty]
        bool alternative;

        [ObservableProperty]
        bool optional;
        #endregion
    }
}
