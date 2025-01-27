using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexContactsList : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial List<LexContact> Content { get; set; } = [];

        [ObservableProperty]
        public partial bool First { get; set; }

        [ObservableProperty]
        public partial bool Last { get; set; }

        [ObservableProperty]
        public partial long TotalPages { get; set; }

        [ObservableProperty]
        public partial long TotalElements { get; set; }

        [ObservableProperty]
        public partial long NumberOfElements { get; set; }

        [ObservableProperty]
        public partial long Size { get; set; }

        [ObservableProperty]
        public partial long Number { get; set; }

        [ObservableProperty]
        public partial List<LexVoucherSort> Sort { get; set; } = [];
        #endregion
    }
}
