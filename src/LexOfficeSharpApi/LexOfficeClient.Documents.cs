using AndreasReitberger.API.LexOffice.Enum;
#if !NETFRAMEWORK
using AndreasReitberger.API.REST;
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
        #region Documents

        public async Task<LexQuotationFiles?> RenderDocumentAsync(Guid invoiceId)
        {
            string? jsonString = await BaseApiCallAsync<string>($"invoices/{invoiceId}/document", Method.Get);
            if (jsonString is null) return null;
            LexQuotationFiles? response = JsonConvert.DeserializeObject<LexQuotationFiles>(jsonString);
            return response;
        }

        public Task<byte[]?> GetFileAsync(Guid id) => BaseApiCallAsync<byte[]>($"files/{id}", Method.Get);

        #endregion
    }
}
