using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexDocumentResponse : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        Guid id;

        [ObservableProperty]
        Guid organizationId;

        [ObservableProperty]
        DateTimeOffset createdDate;

        [ObservableProperty]
        DateTimeOffset updatedDate;

        [ObservableProperty]
        long version;

        [ObservableProperty]
        string language = string.Empty;

        [ObservableProperty]
        bool archived;

        [ObservableProperty]
        string voucherStatus = string.Empty;

        [ObservableProperty]
        string voucherNumber = string.Empty;

        [ObservableProperty]
        DateTimeOffset voucherDate;

        [ObservableProperty]
        DateTimeOffset expirationDate;

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

        [ObservableProperty]
        string eventType = string.Empty;

        [ObservableProperty]
        string callbackUrl = string.Empty;
        #endregion
    }
}
