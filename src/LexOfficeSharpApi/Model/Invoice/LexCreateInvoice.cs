using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    [Obsolete("Use `LexDocumentRespone` instead. This is a common file for holding all information")]
    public partial class LexCreateInvoice : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial bool Archived { get; set; }

        [ObservableProperty]
        public partial DateTimeOffset VoucherDate { get; set; }

        [ObservableProperty]
        public partial LexContactAddress? Address { get; set; }

        [ObservableProperty]
        public partial List<LexQuotationItem> LineItems { get; set; } = [];

        [ObservableProperty]
        public partial LexQuotationTotalPrice? TotalPrice { get; set; }

        [ObservableProperty]
        public partial List<LexQuotationTaxAmount> TaxAmounts { get; set; } = [];

        [ObservableProperty]
        public partial LexQuotationTaxConditions? TaxConditions { get; set; }

        [ObservableProperty]
        public partial LexQuotationPaymentConditions? PaymentConditions { get; set; }

        [ObservableProperty]
        public partial LexShippingConditions? ShippingConditions { get; set; }

        [ObservableProperty]
        public partial string Introduction { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Remark { get; set; } = string.Empty;

        [ObservableProperty]
        public partial LexQuotationFiles? Files { get; set; }

        [ObservableProperty]
        public partial string Title { get; set; } = string.Empty;
        #endregion
    }
}
