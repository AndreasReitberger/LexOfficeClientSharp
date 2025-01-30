using AndreasReitberger.API.LexOffice.Enum;
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
        #region Quotations

#if NETFRAMEWORK
        public async Task<List<VoucherListContent>> GetQuotationListAsync(LexVoucherStatus status, bool archived = false, int page = 0, int size = 25)
        {
            List<VoucherListContent> result = [];
            string cmd = $"voucherlist?voucherType={LexVoucherType.Quotation.ToString().ToLower()}" +
                $"&voucherStatus={status.ToString().ToLower()}" +
                $"&archived={archived}"
                ;
            cmd += $"&page={page}&size={size}";

            string? jsonString = await BaseApiCallAsync<string>(cmd, Method.Get) ?? string.Empty;
            LexVoucherList? list = JsonConvert.DeserializeObject<LexVoucherList>(jsonString);
            if (list is not null)
            {
                if (page < list.TotalPages - 1)
                {
                    page++;
                    result = new List<VoucherListContent>(list.Content);
                    List<VoucherListContent> append = await GetQuotationListAsync(status, archived, page, size);
                    result = new List<VoucherListContent>(result.Concat(append));
                    return result;
                }
            }
            return result;
        }

        public async Task<List<LexDocumentResponse>> GetQuotationsAsync(List<Guid> ids)
        {
            List<LexDocumentResponse> result = [];
            foreach (Guid Id in ids)
            {
                LexDocumentResponse? quote = await GetQuotationAsync(Id);
                if (quote is not null)
                    result.Add(quote);
            }
            return result;
        }

        public async Task<List<LexDocumentResponse>> GetQuotationsAsync(List<VoucherListContent> voucherList)
        {
            List<Guid> ids = [.. voucherList.Select(id => id.Id)];
            return await GetQuotationsAsync(ids);
        }

        public async Task<LexDocumentResponse?> GetQuotationAsync(Guid id)
        {
            string? jsonString = await BaseApiCallAsync<string>($"quotations/{id}", Method.Get);
            if (jsonString is null) return null;
            LexDocumentResponse? response = JsonConvert.DeserializeObject<LexDocumentResponse>(jsonString);
            return response;
        }
#else
        public async Task<List<VoucherListContent>> GetQuotationListAsync(LexVoucherStatus status, bool archived = false, int page = 0, int size = 25, int pages = -1, int cooldown = 0)
        {
            IRestApiRequestRespone? result = null;
            List<VoucherListContent> resultObject = [];
            try
            {
                string targetUri = $"voucherlist";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: new()
                       {
                           { "voucherType", LexVoucherType.Quotation.ToString().ToLower() },
                           { "voucherStatus", status.ToString().ToLower() },
                           { "archived", $"{archived}" },
                           { "page", $"{page}" },
                           { "size", $"{size}" },
                       },
                       cts: default
                       )
                    .ConfigureAwait(false);
                LexVoucherList? list = GetObjectFromJson<LexVoucherList>(result?.Result, NewtonsoftJsonSerializerSettings);
                if (list is not null)
                {
                    if (list.TotalPages > 1 && page < list.TotalPages && (pages <= 0 || (pages - 1 > page && pages > 1)))
                    {
                        resultObject = [.. list.Content];
                        if (MinimumCooldown > 0 && cooldown > 0)
                            await Task.Delay(cooldown < MinimumCooldown ? MinimumCooldown : cooldown);
                        page++;
                        List<VoucherListContent> append = await GetQuotationListAsync(status, archived, page, size, pages, cooldown);
                        resultObject = [.. resultObject.Concat(append)];
                        return resultObject;
                    }
                    else
                        resultObject = [.. list.Content];
                }
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }
        public async Task<List<LexDocumentResponse>> GetQuotationsAsync(List<Guid> ids, int cooldown = 50)
        {
            List<LexDocumentResponse> result = [];
            foreach (Guid Id in ids)
            {
                LexDocumentResponse? quote = await GetQuotationAsync(Id);
                if (quote is not null)
                    result.Add(quote);
                if (cooldown > 0)
                    await Task.Delay(50);
            }
            return result;
        }

        public async Task<List<LexDocumentResponse>> GetQuotationsAsync(List<VoucherListContent> voucherList)
        {
            List<Guid> ids = [.. voucherList.Select(id => id.Id)];
            return await GetQuotationsAsync(ids);
        }

        public async Task<LexDocumentResponse?> GetQuotationAsync(Guid id)
        {
            IRestApiRequestRespone? result = null;
            LexDocumentResponse? resultObject = null;
            try
            {
                string targetUri = $"quotations/{id}";
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
                resultObject = GetObjectFromJson<LexDocumentResponse>(result?.Result, NewtonsoftJsonSerializerSettings);
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
