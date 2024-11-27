using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationPaymentConditions : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        Guid? id;

        [ObservableProperty]
        string paymentTermLabel = string.Empty;

        [ObservableProperty]
        string paymentTermLabelTemplate = string.Empty;

        [ObservableProperty]
        long paymentTermDuration;

        [ObservableProperty]
        LexQuotationDiscountCondition? paymentDiscountConditions;

        [ObservableProperty]
        bool organizationDefault;
        #endregion
    }
}
