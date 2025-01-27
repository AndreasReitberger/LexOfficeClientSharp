using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace AndreasReitberger.API.LexOffice
{
    [Obsolete("Use `LexResponseDefault` instead")]
    public partial class LexInvoiceResponse : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial Guid Id { get; set; }

        [ObservableProperty]
        public partial string ResourceUri { get; set; } = string.Empty;

        [ObservableProperty]
        public partial DateTimeOffset CreatedDate { get; set; }

        [ObservableProperty]
        public partial DateTimeOffset UpdatedDate { get; set; }

        [ObservableProperty]
        public partial long Version { get; set; }
        #endregion
    }
}
