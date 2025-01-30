using AndreasReitberger.API.LexOffice.Enum;
using AndreasReitberger.API.LexOffice.Struct;

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
        #region Documents

#if NETFRAMEWORK
        public async Task<LexQuotationFiles?> RenderDocumentAsync(Guid invoiceId)
        {
            string? jsonString = await BaseApiCallAsync<string>($"invoices/{invoiceId}/document", Method.Get);
            if (jsonString is null) return null;
            LexQuotationFiles? response = JsonConvert.DeserializeObject<LexQuotationFiles>(jsonString);
            return response;
        }

        public Task<byte[]?> GetFileAsync(Guid id) => BaseApiCallAsync<byte[]>($"files/{id}", Method.Get);

#else
        /// <summary>
        /// Render a document for a specific invoice id
        /// Docs: <seealso href="https://developers.lexoffice.io/docs/#invoices-endpoint-retrieve-an-invoice"/>
        /// </summary>
        /// <param name="invoiceId">The invoice id to be rendered</param>
        /// <returns><seealso cref="LexQuotationFiles"/></returns>
        public async Task<LexQuotationFiles?> RenderDocumentAsync(Guid invoiceId)
        {
            IRestApiRequestRespone? result = null;
            LexQuotationFiles? resultObject = null;
            try
            {
                string targetUri = $"invoices/{invoiceId}/document";
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
                resultObject = GetObjectFromJson<LexQuotationFiles>(result?.Result, NewtonsoftJsonSerializerSettings);
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        /// <summary>
        /// Gets the file of a rendered document by its id
        /// Docs: <seealso href="https://developers.lexoffice.io/docs/#files-endpoint-upload-a-file"/>
        /// </summary>
        /// <param name="id">The id of the rendered document</param>
        /// <param name="target">The requested file format, for instance a pdf. See <seealso cref="AcceptedFileHeaders"/></param>
        /// <returns>List of <seealso cref="byte"/></returns>
        public Task<byte[]?> GetFileAsync(Guid id, string target = AcceptedFileHeaders.Pdf) 
            => DownloadFileFromUriAsync($"files/{id}", AuthHeaders, headers: new() { { "Accept", target } });
#endif
        #endregion
    }
}
