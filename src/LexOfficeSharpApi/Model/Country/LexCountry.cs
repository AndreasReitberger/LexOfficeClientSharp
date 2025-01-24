using AndreasReitberger.API.LexOffice.Enum;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexCountry : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        public partial string CountryCode { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string CountryNameDE { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string CountryNameEN { get; set; } = string.Empty;

        [ObservableProperty]
        public partial LexTaxClassifications TaxClassification { get; set; }
        //string taxClassification = string.Empty;
        #endregion
    }
}
