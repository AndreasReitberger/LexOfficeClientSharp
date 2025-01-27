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
        #region Invoices

#if NETFRAMEWORK
        public async Task<List<VoucherListContent>> GetInvoiceListAsync(LexVoucherStatus status, bool archived = false, int page = 0, int size = 25, int pages = -1, int cooldown = 0)
        {
            List<VoucherListContent> result = [];
            string cmd = $"voucherlist?voucherType={LexVoucherType.Invoice.ToString().ToLower()}" +
                $"&voucherStatus={status.ToString().ToLower()}" +
                $"&archived={archived}"
                ;
            cmd += $"&page={page}&size={size}";

            string? jsonString = await BaseApiCallAsync<string>(cmd, Method.Get) ?? string.Empty;
            LexVoucherList? list = JsonConvert.DeserializeObject<LexVoucherList>(jsonString);
            if (list is not null)
            {
                if (list.TotalPages > 1 && page < list.TotalPages && (pages <= 0 || (pages - 1 > page && pages > 1)))
                {
                    result = new List<VoucherListContent>(list.Content);
                    if (MinimumCooldown > 0 && cooldown > 0)
                        await Task.Delay(cooldown < MinimumCooldown ? MinimumCooldown : cooldown);
                    page++;
                    List<VoucherListContent> append = await GetInvoiceListAsync(status, archived, page, size, pages, cooldown);
                    result = new List<VoucherListContent>(result.Concat(append));
                    return result;
                }
                else
                    result = new List<VoucherListContent>(list.Content);
            }
            return result;
        }

        public async Task<List<LexDocumentResponse>> GetInvoicesAsync(List<Guid> ids, int cooldown = 0)
        {
            List<LexDocumentResponse> result = [];
            foreach (Guid id in ids)
            {
                LexDocumentResponse? quote = await GetInvoiceAsync(id);
                if (quote is not null)
                    result.Add(quote);
                await Task.Delay(cooldown < MinimumCooldown ? MinimumCooldown : cooldown);
            }
            return result;
        }

        public async Task<List<LexDocumentResponse>> GetInvoicesAsync(List<VoucherListContent> voucherList)
        {
            List<Guid> ids = voucherList.Select(id => id.Id).ToList();
            return await GetInvoicesAsync(ids);
        }

        public async Task<LexDocumentResponse?> GetInvoiceAsync(Guid id)
        {
            string? jsonString = await BaseApiCallAsync<string>($"invoices/{id}", Method.Get) ?? string.Empty;
            LexDocumentResponse? response = JsonConvert.DeserializeObject<LexDocumentResponse>(jsonString);
            return response;
        }

        public async Task<LexResponseDefault?> AddInvoiceAsync(LexDocumentResponse lexQuotation, bool isFinalized = false)
        {
            string? jsonString = await BaseApiCallAsync<string>($"invoices?finalize={isFinalized}", Method.Post, JsonConvert.SerializeObject(lexQuotation, NewtonsoftJsonSerializerSettings)) ?? string.Empty;
            LexResponseDefault? response = JsonConvert.DeserializeObject<LexResponseDefault>(jsonString);
            return response;
        }
#else
        public async Task<List<VoucherListContent>> GetInvoiceListAsync(LexVoucherStatus status, bool archived = false, int page = 0, int size = 25, int pages = -1, int cooldown = 0)
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
                           { "voucherType", LexVoucherType.Invoice.ToString().ToLower() },
                           { "voucherStatus", status.ToString().ToLower() },
                           { "archived", $"{archived}" },
                           { "page", $"{page}" },
                           { "size", $"{size}" },
                       },
                       cts: default
                       )
                    .ConfigureAwait(false);
                LexVoucherList? list = GetObjectFromJson<LexVoucherList>(result?.Result, base.NewtonsoftJsonSerializerSettings);
                if (list is not null)
                {
                    if (list.TotalPages > 1 && page < list.TotalPages && (pages <= 0 || (pages - 1 > page && pages > 1)))
                    {
                        resultObject = [.. list.Content];
                        if (MinimumCooldown > 0 && cooldown > 0)
                            await Task.Delay(cooldown < MinimumCooldown ? MinimumCooldown : cooldown);
                        page++;
                        List<VoucherListContent> append = await GetInvoiceListAsync(status, archived, page, size, pages, cooldown);
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

        public async Task<List<LexDocumentResponse>> GetInvoicesAsync(List<Guid> ids, int cooldown = 0)
        {
            List<LexDocumentResponse> result = [];
            foreach (Guid id in ids)
            {
                LexDocumentResponse? quote = await GetInvoiceAsync(id);
                if (quote is not null)
                    result.Add(quote);
                await Task.Delay(cooldown < MinimumCooldown ? MinimumCooldown : cooldown);
            }
            return result;
        }

        public async Task<List<LexDocumentResponse>> GetInvoicesAsync(List<VoucherListContent> voucherList)
        {
            List<Guid> ids = voucherList.Select(id => id.Id).ToList();
            return await GetInvoicesAsync(ids);
        }

        public async Task<LexDocumentResponse?> GetInvoiceAsync(Guid id)
        {
            IRestApiRequestRespone? result = null;
            LexDocumentResponse? resultObject = null;
            try
            {
                string targetUri = $"invoices/{id}";
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

        public async Task<LexResponseDefault?> AddInvoiceAsync(LexDocumentResponse lexQuotation, bool isFinalized = false)
        {
            IRestApiRequestRespone? result = null;
            LexResponseDefault? resultObject = null;
            try
            {
                string json = JsonConvert.SerializeObject(lexQuotation, NewtonsoftJsonSerializerSettings);
                string targetUri = $"invoices";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "",
                       jsonObject: json,
                       authHeaders: AuthHeaders,
                       urlSegments: new()
                       {
                           { "finalize", $"{isFinalized}" }
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
