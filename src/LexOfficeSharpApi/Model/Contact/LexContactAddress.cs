using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexContactAddress : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        string name = string.Empty;

        [ObservableProperty]
        string supplement = string.Empty;

        [ObservableProperty]
        string street = string.Empty;

        [ObservableProperty]
        string zip = string.Empty;

        [ObservableProperty]
        string city = string.Empty;

        [ObservableProperty]
        string countryCode = string.Empty;
        #endregion
    }
}
