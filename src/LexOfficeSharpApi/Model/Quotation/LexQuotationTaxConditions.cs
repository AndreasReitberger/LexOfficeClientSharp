using AndreasReitberger.API.LexOffice.Enum;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexQuotationTaxConditions : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TaxTypeString))]
        LexQuotationTaxType taxType;

        [JsonIgnore]
        public string TaxTypeString => $"{TaxType}";

        #endregion
    }
}
