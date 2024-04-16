using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexCustomerNumber : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        long number;
        #endregion
    }
}
