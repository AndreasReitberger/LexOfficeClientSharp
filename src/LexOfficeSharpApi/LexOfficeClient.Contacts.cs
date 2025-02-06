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
        /// <summary>
        /// Gets a list of all available contacts.
        /// Docs: <see href="https://developers.lexoffice.io/docs/#contacts-endpoint-purpose"/>
        /// </summary>
        /// <param name="type">The type, either <c>Customer</c> or <c>Vendor</c></param>
        /// <param name="page">The page to start with</param>
        /// <param name="size">The amount of contacts for each page</param>
        /// <param name="pages">The end page, take all pages with <c>-1</c></param>
        /// <param name="cooldown">A wait between each query (might be needed to avoid rejections from the api server)</param>
        /// <returns>List of <seealso cref="LexContact"/></returns>
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
        
        /// <summary>
        /// Gets a single contact by its id.
        /// Docs: <see href="https://developers.lexoffice.io/docs/#contacts-endpoint-retrieve-a-contact"/>
        /// </summary>
        /// <param name="id">The id of the contact</param>
        /// <returns><seealso cref="LexContact"/></returns>
        public async Task<LexContact?> GetContactAsync(Guid id)
        {
            string? jsonString = await BaseApiCallAsync<string>($"contacts/{id}", Method.Get) ?? string.Empty;
            LexContact? contact = JsonConvert.DeserializeObject<LexContact>(jsonString);
            return contact;
        }
        
        /// <summary>
        /// Adds a new contact to the lexoffice api. See also <seealso cref="LexContact"/>
        /// Docs: <see href="https://developers.lexoffice.io/docs/#contacts-endpoint-create-a-contact"/>
        /// </summary>
        /// <param name="lexContact">The contact to be added as <c>LexContact</c></param>
        /// <returns><seealso cref="LexResponseDefault"/></returns>
        public async Task<LexResponseDefault?> AddContactAsync(LexContact lexContact)
        {
            string? jsonString = await BaseApiCallAsync<string>($"contacts", Method.Post, JsonConvert.SerializeObject(lexContact, NewtonsoftJsonSerializerSettings)) ?? string.Empty;
            LexResponseDefault? response = JsonConvert.DeserializeObject<LexResponseDefault>(jsonString);
            return response;
        }
        
        /// <summary>
        /// Updates an existing contact in the lexoffice api. See also <seealso cref="LexContact"/>
        /// </summary>
        /// <param name="contactId">The id of the contact</param>
        /// <param name="lexContact">The updated contact as <seealso cref="LexContact"/></param>
        /// <returns><seealso cref="LexResponseDefault"/></returns>
        public async Task<LexResponseDefault?> UpdateContactAsync(Guid contactId, LexContact lexContact)
        {
            string? jsonString = await BaseApiCallAsync<string>($"contacts/{contactId}", Method.Post, JsonConvert.SerializeObject(lexContact, NewtonsoftJsonSerializerSettings)) ?? string.Empty;
            LexResponseDefault? response = JsonConvert.DeserializeObject<LexResponseDefault>(jsonString);
            return response;
        }
#else
        /// <summary>
        /// Gets a list of all available contacts.
        /// Docs: <see href="https://developers.lexoffice.io/docs/#contacts-endpoint-purpose"/>
        /// </summary>
        /// <param name="type">The type, either <c>Customer</c> or <c>Vendor</c></param>
        /// <param name="page">The page to start with</param>
        /// <param name="size">The amount of contacts for each page</param>
        /// <param name="pages">The end page, take all pages with <c>-1</c></param>
        /// <param name="cooldown">A wait between each query (might be needed to avoid rejections from the api server)</param>
        /// <returns>List of <seealso cref="LexContact"/></returns>
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

        /// <summary>
        /// Gets a single contact by its id.
        /// Docs: <see href="https://developers.lexoffice.io/docs/#contacts-endpoint-retrieve-a-contact"/>
        /// </summary>
        /// <param name="id">The id of the contact</param>
        /// <returns><seealso cref="LexContact"/></returns>
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

        /// <summary>
        /// Adds a new contact to the lexoffice api. See also <seealso cref="LexContact"/>
        /// Docs: <see href="https://developers.lexoffice.io/docs/#contacts-endpoint-create-a-contact"/>
        /// </summary>
        /// <param name="lexContact">The contact to be added as <c>LexContact</c></param>
        /// <returns><seealso cref="LexResponseDefault"/></returns>
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

        /// <summary>
        /// Updates an existing contact in the lexoffice api. See also <seealso cref="LexContact"/>
        /// </summary>
        /// <param name="contactId">The id of the contact</param>
        /// <param name="lexContact">The updated contact as <seealso cref="LexContact"/></param>
        /// <returns><seealso cref="LexResponseDefault"/></returns>
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
