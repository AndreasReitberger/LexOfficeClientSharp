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
        Uri? resourceUri;

        [ObservableProperty]
        DateTimeOffset createdDate;

        [ObservableProperty]
        DateTimeOffset updatedDate;

        [ObservableProperty]
        DateTimeOffset eventDate;

        [ObservableProperty]
        string eventType;

        [ObservableProperty]
        string callbackUrl;

        [ObservableProperty]
        long version;
    }
}
