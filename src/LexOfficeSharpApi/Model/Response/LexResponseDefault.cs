using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexResponseDefault : ObservableObject
    {
        [ObservableProperty]
        Guid? id;

        [ObservableProperty]
        Guid subscriptionId;

        [ObservableProperty]
        Guid organizationId;

        [ObservableProperty]
        Guid resourceId;

        [ObservableProperty]
        Uri? resourceUri;

        [ObservableProperty]
        DateTimeOffset createdDate;

        [ObservableProperty]
        DateTimeOffset updatedDate;

        [ObservableProperty]
        DateTimeOffset eventDate;

        [ObservableProperty]
        string eventType = string.Empty;

        [ObservableProperty]
        string callbackUrl = string.Empty;

        [ObservableProperty]
        long version;
    }
}
