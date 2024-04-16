using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexVoucherSort : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        string property = string.Empty;

        [ObservableProperty]
        string direction = string.Empty;

        [ObservableProperty]
        bool? ignoreCase;

        [ObservableProperty]
        string nullHandling = string.Empty;

        [ObservableProperty]
        bool? ascending;
        #endregion
    }
}
