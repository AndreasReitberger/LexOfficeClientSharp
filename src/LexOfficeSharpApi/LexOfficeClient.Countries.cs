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
using System.Linq;
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
        #region Countries

#if NETFRAMEWORK
        public async Task<List<LexCountry>> GetCountriesAsync()
        {
            List<LexCountry> result = [];
            string? jsonString = await BaseApiCallAsync<string>("countries", Method.Get) ?? string.Empty;
            result = JsonConvert.DeserializeObject<List<LexCountry>>(jsonString) ?? [];
            return result;
        }
#else

        public async Task<List<LexCountry>> GetCountriesAsync()
        {
            IRestApiRequestRespone? result = null;
            List<LexCountry> resultObject = [];
            try
            {
                string targetUri = $"countries";
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
                resultObject = [.. GetObjectFromJson<List<LexCountry>>(result?.Result, NewtonsoftJsonSerializerSettings)];
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
