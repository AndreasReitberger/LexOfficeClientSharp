#if !NETFRAMEWORK
using AndreasReitberger.API.REST;
#else
using CommunityToolkit.Mvvm.ComponentModel;
#endif
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreasReitberger.API.LexOffice
{
    // https://developers.lexoffice.io/docs/#lexoffice-api-documentation/?cid=1766
    public partial class LexOfficeClient
#if NETFRAMEWORK
       : ObservableObject
#else
       : RestApiClient
#endif
    {
        #region Conditions

        public async Task<List<LexQuotationPaymentConditions>> GetPaymentConditionsAsync()
        {
            List<LexQuotationPaymentConditions> result = [];
            string? jsonString = await BaseApiCallAsync<string>($"payment-conditions", Method.Get) ?? string.Empty;
            result = JsonConvert.DeserializeObject<List<LexQuotationPaymentConditions>>(jsonString) ?? [];
            return result;
        }
        #endregion

    }
}
