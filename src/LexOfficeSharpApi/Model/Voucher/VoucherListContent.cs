using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.LexOffice
{
    public partial class VoucherListContent : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial Guid Id { get; set; }

        [ObservableProperty]
        public partial string VoucherType { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string VoucherStatus { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string VoucherNumber { get; set; } = string.Empty;

        [ObservableProperty]
        public partial DateTimeOffset VoucherDate { get; set; }

        [ObservableProperty]
        public partial DateTimeOffset UpdatedDate { get; set; }

        [ObservableProperty]
        public partial DateTimeOffset DueDate { get; set; }

        [ObservableProperty]
        public partial string ContactName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial decimal TotalAmount { get; set; }

        [ObservableProperty]
        public partial string Currency { get; set; } = string.Empty;

        [ObservableProperty]
        public partial bool Archived { get; set; }
        #endregion
    }
}
