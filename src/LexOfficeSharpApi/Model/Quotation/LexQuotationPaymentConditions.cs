using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationPaymentConditions : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        string paymentTermLabel = string.Empty;

        [ObservableProperty]
        long paymentTermDuration;

        [ObservableProperty]
        Dictionary<string, long> paymentDiscountConditions = [];
        #endregion
    }
}
