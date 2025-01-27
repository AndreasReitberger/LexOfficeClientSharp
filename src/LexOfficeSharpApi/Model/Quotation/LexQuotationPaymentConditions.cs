using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationPaymentConditions : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial Guid? Id { get; set; }

        [ObservableProperty]
        public partial string PaymentTermLabel { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string PaymentTermLabelTemplate { get; set; } = string.Empty;

        [ObservableProperty]
        public partial long PaymentTermDuration { get; set; }

        [ObservableProperty]
        public partial LexQuotationDiscountCondition? PaymentDiscountConditions { get; set; }

        [ObservableProperty]
        public partial bool OrganizationDefault { get; set; }
        #endregion
    }
}
