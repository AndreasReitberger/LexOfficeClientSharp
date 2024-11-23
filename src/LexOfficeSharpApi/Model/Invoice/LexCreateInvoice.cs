using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexCreateInvoice : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        DateTimeOffset voucherDate;

        [ObservableProperty]
        LexContactAddress? address;

        [ObservableProperty]
        List<LexQuotationItem> lineItems = [];

        [ObservableProperty]
        LexQuotationTotalPrice? totalPrice;

        [ObservableProperty]
        List<LexQuotationTaxAmount> taxAmounts = [];

        [ObservableProperty]
        LexQuotationTaxConditions? taxConditions;

        [ObservableProperty]
        LexQuotationPaymentConditions? paymentConditions;
        
        [ObservableProperty]
        LexShippingConditions? shippingConditions;

        [ObservableProperty]
        string introduction = string.Empty;

        [ObservableProperty]
        string remark = string.Empty;

        [ObservableProperty]
        LexQuotationFiles? files;

        [ObservableProperty]
        string title = string.Empty;
        #endregion
    }
}
