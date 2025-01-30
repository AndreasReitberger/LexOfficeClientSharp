using AndreasReitberger.API.LexOffice.Enum;
#if !NETFRAMEWORK
using AndreasReitberger.API.REST;
using AndreasReitberger.API.REST.Events;
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

        #region Contacts

#if NETFRAMEWORK
        public async Task<List<LexContact>> GetContactsAsync(LexContactType type, int page = 0, int size = 25, int pages = -1, int cooldown = 0)
        {
            List<LexContact> result = [];
            string cmd = $"contacts?{(type == LexContactType.Customer ? "customer" : "vendor")}=true";
            cmd += $"&page={page}&size={size}";

            string? jsonString = await BaseApiCallAsync<string>(cmd, Method.Get) ?? string.Empty;
            LexContactsList? list = JsonConvert.DeserializeObject<LexContactsList>(jsonString);
            if (list != null)
            {
                if (list.TotalPages > 1 && page < list.TotalPages && (pages <= 0 || (pages - 1 > page && pages > 1)))
                {
                    result = new List<LexContact>(list.Content);
                    if (MinimumCooldown > 0 && cooldown > 0)
                        await Task.Delay(cooldown < MinimumCooldown ? MinimumCooldown : cooldown);
                    page++;
                    List<LexContact> append = await GetContactsAsync(type, page, size, pages, cooldown);
                    result = new List<LexContact>(result.Concat(append));
                    return result;
                }
                else
                    result = new List<LexContact>(list.Content);
            }
            return result;
        }

        public async Task<LexContact?> GetContactAsync(Guid id)
        {
            string? jsonString = await BaseApiCallAsync<string>($"contacts/{id}", Method.Get) ?? string.Empty;
            LexContact? contact = JsonConvert.DeserializeObject<LexContact>(jsonString);
            return contact;
        }

        public async Task<LexResponseDefault?> AddContactAsync(LexContact lexContact)
        {
            string? jsonString = await BaseApiCallAsync<string>($"contacts", Method.Post, JsonConvert.SerializeObject(lexContact, NewtonsoftJsonSerializerSettings)) ?? string.Empty;
            LexResponseDefault? response = JsonConvert.DeserializeObject<LexResponseDefault>(jsonString);
            return response;
        }

        public async Task<LexResponseDefault?> UpdateContactAsync(Guid contactId, LexContact lexContact)
        {
            string? jsonString = await BaseApiCallAsync<string>($"contacts/{contactId}", Method.Post, JsonConvert.SerializeObject(lexContact, NewtonsoftJsonSerializerSettings)) ?? string.Empty;
            LexResponseDefault? response = JsonConvert.DeserializeObject<LexResponseDefault>(jsonString);
            return response;
        }
#else
        public async Task<List<LexContact>> GetContactsAsync(LexContactType type, int page = 0, int size = 25, int pages = -1, int cooldown = 0)
        {
            IRestApiRequestRespone? result = null;
            List<LexContact> resultObject = [];
            try
            {
                string targetUri = $"contacts";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: new()
                       {
                           { type == LexContactType.Customer ? "customer" : "vendor", "true" },
                           { "page", $"{page}" },
                           { "size", $"{size}" },
                       },
                       cts: default
                       )
                    .ConfigureAwait(false);
                LexContactsList? list = GetObjectFromJson<LexContactsList>(result?.Result, NewtonsoftJsonSerializerSettings);
                if (list is not null)
                {
                    if (list.TotalPages > 1 && page < list.TotalPages && (pages <= 0 || (pages - 1 > page && pages > 1)))
                    {
                        resultObject = new List<LexContact>(list.Content);
                        if (MinimumCooldown > 0 && cooldown > 0)
                            await Task.Delay(cooldown < MinimumCooldown ? MinimumCooldown : cooldown);
                        page++;
                        List<LexContact> append = await GetContactsAsync(type, page, size, pages, cooldown);
                        resultObject = new List<LexContact>(resultObject.Concat(append));
                        return resultObject;
                    }
                    else
                        resultObject = new List<LexContact>(list.Content);
                }
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<LexContact?> GetContactAsync(Guid id)
        {
            IRestApiRequestRespone? result = null;
            LexContact? resultObject = null;
            try
            {
                string targetUri = $"contacts/{id}";
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
                resultObject = GetObjectFromJson<LexContact>(result?.Result, NewtonsoftJsonSerializerSettings);
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<LexResponseDefault?> AddContactAsync(LexContact lexContact)
        {
            IRestApiRequestRespone? result = null;
            LexResponseDefault? resultObject = null;
            try
            {
                string json = JsonConvert.SerializeObject(lexContact, NewtonsoftJsonSerializerSettings);
                string targetUri = $"contacts";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "",
                       jsonObject: json,
                       authHeaders: AuthHeaders,
                       urlSegments: null,
                       cts: default
                       )
                    .ConfigureAwait(false);
                resultObject = GetObjectFromJson<LexResponseDefault>(result?.Result, NewtonsoftJsonSerializerSettings);
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        public async Task<LexResponseDefault?> UpdateContactAsync(Guid contactId, LexContact lexContact)
        {
            IRestApiRequestRespone? result = null;
            LexResponseDefault? resultObject = null;
            try
            {
                string targetUri = $"contacts/{contactId}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: "",
                       jsonObject: lexContact,
                       authHeaders: AuthHeaders,
                       urlSegments: null,
                       cts: default
                       )
                    .ConfigureAwait(false);
                resultObject = GetObjectFromJson<LexResponseDefault>(result?.Result, NewtonsoftJsonSerializerSettings);
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
