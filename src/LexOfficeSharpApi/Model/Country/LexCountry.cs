using AndreasReitberger.API.LexOffice.Enum;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexCountry : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        string countryCode = string.Empty;

        [ObservableProperty]
        string countryNameDE = string.Empty;

        [ObservableProperty]
        string countryNameEN = string.Empty;

        [ObservableProperty]
        LexTaxClassifications taxClassification;
        //string taxClassification = string.Empty;
        #endregion
    }
}
