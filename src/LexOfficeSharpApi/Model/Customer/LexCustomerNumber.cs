using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexCustomerNumber : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial long Number { get; set; }
        #endregion
    }
}
