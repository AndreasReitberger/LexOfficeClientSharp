using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexCustomer : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        long number;
        #endregion
    }
}
