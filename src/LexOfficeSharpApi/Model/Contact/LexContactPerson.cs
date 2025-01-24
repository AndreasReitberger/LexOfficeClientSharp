using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexContactPerson : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial string Salutation { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string FirstName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string LastName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial bool Primary { get; set; }

        [ObservableProperty]
        public partial string EmailAddress { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string PhoneNumber { get; set; } = string.Empty;
        #endregion
    }
}
