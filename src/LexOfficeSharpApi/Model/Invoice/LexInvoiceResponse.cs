﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace AndreasReitberger.API.LexOffice
{
    [Obsolete("Use `LexResponseDefault` instead")]
    public partial class LexInvoiceResponse : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        Guid id;

        [ObservableProperty]
        string resourceUri = string.Empty;

        [ObservableProperty]
        DateTimeOffset createdDate;

        [ObservableProperty]
        DateTimeOffset updatedDate;

        [ObservableProperty]
        long version;
        #endregion
    }
}
