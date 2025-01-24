#if !NETFRAMEWORK
using AndreasReitberger.API.REST;
#else
using CommunityToolkit.Mvvm.ComponentModel;
#endif
using Newtonsoft.Json;
using RestSharp;
using System;
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
        #region Payments
        public async Task<LexPayments?> GetPaymentsAsync(Guid invoiceId)
        {
            string? jsonString = await BaseApiCallAsync<string>($"payments/{invoiceId}", Method.Get);
            if (jsonString is null) return null;
            LexPayments? response = JsonConvert.DeserializeObject<LexPayments>(jsonString);
            return response;
        }
        #endregion
    }
}
