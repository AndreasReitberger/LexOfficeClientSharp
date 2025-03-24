using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexContactAddress : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial string ContactId { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Supplement { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Street { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Zip { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string City { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string CountryCode { get; set; } = string.Empty;
        #endregion
    }
}
