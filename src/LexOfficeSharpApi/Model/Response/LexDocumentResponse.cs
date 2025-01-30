using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexDocumentResponse : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial Guid Id { get; set; }

        [ObservableProperty]
        public partial Guid OrganizationId { get; set; }

        [ObservableProperty]
        public partial DateTimeOffset CreatedDate { get; set; }

        [ObservableProperty]
        public partial DateTimeOffset UpdatedDate { get; set; }

        [ObservableProperty]
        public partial long Version { get; set; }

        [ObservableProperty]
        public partial string Language { get; set; } = string.Empty;

        [ObservableProperty]
        public partial bool Archived { get; set; }

        [ObservableProperty]
        public partial string VoucherStatus { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string VoucherNumber { get; set; } = string.Empty;

        [ObservableProperty]
        public partial DateTimeOffset VoucherDate { get; set; }

        [ObservableProperty]
        public partial DateTimeOffset ExpirationDate { get; set; }

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
