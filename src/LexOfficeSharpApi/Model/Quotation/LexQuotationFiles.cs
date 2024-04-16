using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationFiles : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        Guid documentFileId;

        #endregion
    }
}
