#if !NETFRAMEWORK
using AndreasReitberger.API.REST;
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

        #region Credit Notes

        public async Task<List<LexDocumentResponse>> GetCreditNotesAsync()
        {
            List<LexDocumentResponse> result = [];
            string? jsonString = await BaseApiCallAsync<string>("credit-notes", Method.Get) ?? string.Empty;
            result = JsonConvert.DeserializeObject<List<LexDocumentResponse>>(jsonString) ?? [];
            return result;
        }

        public async Task<LexDocumentResponse?> GetCreditNoteAsync(Guid id)
        {
            string? jsonString = await BaseApiCallAsync<string>($"credit-notes/{id}", Method.Get) ?? string.Empty;
            LexDocumentResponse? respone = JsonConvert.DeserializeObject<LexDocumentResponse>(jsonString);
            return respone;
        }

        public async Task<LexResponseDefault?> AddCreditNoteAsync(LexDocumentResponse lexQuotation, bool isFinalized = false)
        {
            var body = JsonConvert.SerializeObject(lexQuotation, JsonSerializerSettings);
            string? jsonString = await BaseApiCallAsync<string>($"credit-notes/?finalize={isFinalized}", Method.Post, body) ?? string.Empty;
            LexResponseDefault? response = JsonConvert.DeserializeObject<LexResponseDefault>(jsonString);
            return response;
        }
        #endregion

    }
}
