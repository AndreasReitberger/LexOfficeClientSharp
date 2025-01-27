#if !NETFRAMEWORK
using AndreasReitberger.API.REST;
using AndreasReitberger.API.REST.Interfaces;

#else
using CommunityToolkit.Mvvm.ComponentModel;
#endif
using Newtonsoft.Json;
using RestSharp;
using System;
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
        #region Payments

#if NETFRAMEWORK
        public async Task<LexPayments?> GetPaymentsAsync(Guid invoiceId)
        {
            string? jsonString = await BaseApiCallAsync<string>($"payments/{invoiceId}", Method.Get);
            if (jsonString is null) return null;
            LexPayments? response = JsonConvert.DeserializeObject<LexPayments>(jsonString);
            return response;
        }
#else

        public async Task<LexPayments?> GetPaymentsAsync(Guid invoiceId)
        {
            IRestApiRequestRespone? result = null;
            LexPayments? resultObject = null;
            try
            {
                string targetUri = $"payments/{invoiceId}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: null,
                       cts: default
                       )
                    .ConfigureAwait(false);
                resultObject = GetObjectFromJson<LexPayments>(result?.Result, base.NewtonsoftJsonSerializerSettings);
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
#endif
#endregion
    }
}
