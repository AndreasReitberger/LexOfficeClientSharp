using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexResponseDefault : ObservableObject
    {
        [ObservableProperty]
        public partial Guid? Id { get; set; }

        [ObservableProperty]
        public partial Guid SubscriptionId { get; set; }

        [ObservableProperty]
        public partial Guid OrganizationId { get; set; }

        [ObservableProperty]
        public partial Guid ResourceId { get; set; }

        [ObservableProperty]
        public partial Uri? ResourceUri { get; set; }

        [ObservableProperty]
        public partial DateTimeOffset CreatedDate { get; set; }

        [ObservableProperty]
        public partial DateTimeOffset UpdatedDate { get; set; }

        [ObservableProperty]
        public partial DateTimeOffset EventDate { get; set; }

        [ObservableProperty]
        public partial string EventType { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string CallbackUrl { get; set; } = string.Empty;

        [ObservableProperty]
        public partial long Version { get; set; }
    }
}
