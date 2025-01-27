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

        #region Credit Notes

#if NETFRAMEWORK
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
            var body = JsonConvert.SerializeObject(lexQuotation, NewtonsoftJsonSerializerSettings);
            string? jsonString = await BaseApiCallAsync<string>($"credit-notes/?finalize={isFinalized}", Method.Post, body) ?? string.Empty;
            LexResponseDefault? response = JsonConvert.DeserializeObject<LexResponseDefault>(jsonString);
            return response;
        }
#else

        public async Task<List<LexDocumentResponse>> GetCreditNotesAsync()
        {
            IRestApiRequestRespone? result = null;
            List<LexDocumentResponse> resultObject = [];
            try
            {
                string targetUri = $"credit-notes";
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
                resultObject = [.. GetObjectFromJson<List<LexDocumentResponse>>(result?.Result, base.NewtonsoftJsonSerializerSettings)];
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<LexDocumentResponse?> GetCreditNoteAsync(Guid id)
        {
            IRestApiRequestRespone? result = null;
            LexDocumentResponse? resultObject = null;
            try
            {
                string targetUri = $"credit-notes/{id}";
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
                resultObject = GetObjectFromJson<LexDocumentResponse>(result?.Result, base.NewtonsoftJsonSerializerSettings);
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<LexResponseDefault?> AddCreditNoteAsync(LexDocumentResponse lexQuotation, bool isFinalized = false)
        {
            IRestApiRequestRespone? result = null;
            LexResponseDefault? resultObject = null;
            try
            {
                string json = JsonConvert.SerializeObject(lexQuotation, NewtonsoftJsonSerializerSettings);
                string targetUri = $"credit-notes";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "",
                       jsonObject: json,
                       authHeaders: AuthHeaders,
                       urlSegments: new()
                       {
                           { "finalize", $"{isFinalized}"   }
                       },
                       cts: default
                       )
                    .ConfigureAwait(false);
                resultObject = GetObjectFromJson<LexResponseDefault>(result?.Result, base.NewtonsoftJsonSerializerSettings);
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
