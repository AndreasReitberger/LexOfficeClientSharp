using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexContactPerson : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        string salutation = string.Empty;

        [ObservableProperty]
        string firstName = string.Empty;

        [ObservableProperty]
        string lastName = string.Empty;

        [ObservableProperty]
        bool primary;

        [ObservableProperty]
        string emailAddress = string.Empty;

        [ObservableProperty]
        string phoneNumber = string.Empty;
        #endregion
    }
}
