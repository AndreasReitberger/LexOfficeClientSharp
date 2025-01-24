using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexVoucherSort : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial string Property { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Direction { get; set; } = string.Empty;

        [ObservableProperty]
        public partial bool? IgnoreCase { get; set; }

        [ObservableProperty]
        public partial string NullHandling { get; set; } = string.Empty;

        [ObservableProperty]
        public partial bool? Ascending { get; set; }
        #endregion
    }
}
