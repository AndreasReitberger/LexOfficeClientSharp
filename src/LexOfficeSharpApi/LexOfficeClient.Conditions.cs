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
        #region Conditions

#if NETFRAMEWORK
        /// <summary>
        /// Get all available payment conditions
        /// Docs: <see href="https://developers.lexoffice.io/docs/#payment-conditions-endpoint-retrieve-list-of-payment-conditions"/>
        /// </summary>
        /// <returns>A list of <seealso cref="LexQuotationPaymentConditions"/></returns>
        public async Task<List<LexQuotationPaymentConditions>> GetPaymentConditionsAsync()
        {
            List<LexQuotationPaymentConditions> result = [];
            string? jsonString = await BaseApiCallAsync<string>($"payment-conditions", Method.Get) ?? string.Empty;
            result = JsonConvert.DeserializeObject<List<LexQuotationPaymentConditions>>(jsonString) ?? [];
            return result;
        }
#else
        /// <summary>
        /// Get all available payment conditions
        /// Docs: <see href="https://developers.lexoffice.io/docs/#payment-conditions-endpoint-retrieve-list-of-payment-conditions"/>
        /// </summary>
        /// <returns>A list of <seealso cref="LexQuotationPaymentConditions"/></returns>
        public async Task<List<LexQuotationPaymentConditions>> GetPaymentConditionsAsync()
        {
            IRestApiRequestRespone? result = null;
            List<LexQuotationPaymentConditions> resultObject = [];
            try
            {
                string targetUri = $"payment-conditions";
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
                resultObject = [.. GetObjectFromJson<List<LexQuotationPaymentConditions>>(result?.Result, NewtonsoftJsonSerializerSettings)];
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
