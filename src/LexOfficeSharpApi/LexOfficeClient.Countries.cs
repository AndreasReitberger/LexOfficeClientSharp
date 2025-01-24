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
        #region Countries

        public async Task<List<LexCountry>> GetCountriesAsync()
        {
            List<LexCountry> result = [];
            string? jsonString = await BaseApiCallAsync<string>("countries", Method.Get) ?? string.Empty;
            result = JsonConvert.DeserializeObject<List<LexCountry>>(jsonString) ?? [];
            return result;
        }
        #endregion
    }
}
