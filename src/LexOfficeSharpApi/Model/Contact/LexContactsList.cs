using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexContactsList : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        List<LexContact> content = [];

        [ObservableProperty]
        bool first;

        [ObservableProperty]
        bool last;

        [ObservableProperty]
        long totalPages;

        [ObservableProperty]
        long totalElements;

        [ObservableProperty]
        long numberOfElements;

        [ObservableProperty]
        long size;

        [ObservableProperty]
        long number;

        [ObservableProperty]
        List<LexVoucherSort> sort = [];
        #endregion
    }
}
