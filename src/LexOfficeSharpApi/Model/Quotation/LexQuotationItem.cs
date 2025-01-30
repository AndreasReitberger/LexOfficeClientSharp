using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationItem : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [field: JsonIgnore]
        [JsonIgnore]
        public partial Guid? Id { get; set; }

        [ObservableProperty]
        public partial string Type { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Description { get; set; } = string.Empty;

        [ObservableProperty]
        public partial decimal Quantity { get; set; }

        [ObservableProperty]
        public partial string UnitName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial LexQuotationUnitPrice? UnitPrice { get; set; }

        [ObservableProperty]
        public partial long DiscountPercentage { get; set; }

        [ObservableProperty]
        public partial double LineItemAmount { get; set; }

        [ObservableProperty]
        public partial List<LexQuotationItem> SubItems { get; set; } = [];

        [ObservableProperty]
        public partial bool Alternative { get; set; }

        [ObservableProperty]
        public partial bool Optional { get; set; }
        #endregion
    }
}
