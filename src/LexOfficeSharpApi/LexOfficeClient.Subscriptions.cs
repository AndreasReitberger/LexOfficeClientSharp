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
        #region Subscription
#if NETFRAMEWORK
        /// <summary>
        /// Adds a new event subscription to lexoffice
        /// Docs: <seealso href="https://developers.lexoffice.io/docs/#event-subscriptions-endpoint-create-an-event-subscription"/>
        /// </summary>
        /// <param name="lexQuotation">The quotation as <seealso cref="LexResponseDefault"/></param>
        /// <returns><seealso cref="LexResponseDefault"/></returns>
        public async Task<LexResponseDefault?> AddEventSubscriptionAsync(LexResponseDefault lexQuotation)
        {
            string json = JsonConvert.SerializeObject(lexQuotation, NewtonsoftJsonSerializerSettings) ?? string.Empty;
            string? jsonString = await BaseApiCallAsync<string>($"event-subscriptions", Method.Post, json);
            if (jsonString is null) return null;
            LexResponseDefault? response = JsonConvert.DeserializeObject<LexResponseDefault>(jsonString);
            return response;
        }
        
        /// <summary>
        /// Gets a single event subscription by its id
        /// Docs: <seealso href="https://developers.lexoffice.io/docs/#event-subscriptions-endpoint-retrieve-an-event-subscription"/>
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns><seealso cref="LexResponseDefault"/></returns>
        public async Task<LexResponseDefault?> GetEventSubscriptionAsync(Guid? id)
        {
            string? jsonString = await BaseApiCallAsync<string>($"event-subscriptions/{id}");
            if (jsonString is null) return null;
            LexResponseDefault? response = JsonConvert.DeserializeObject<LexResponseDefault>(jsonString);
            return response;
        }
        
        /// <summary>
        /// Gets a list of all event subscriptions.
        /// Docs: <seealso href="https://developers.lexoffice.io/docs/#event-subscriptions-endpoint-retrieve-all-event-subscriptions"/>
        /// </summary>
        /// <returns>List of <seealso cref="LexResponseDefault"/></returns>
        public async Task<List<LexResponseDefault>?> GetAllEventSubscriptionsAsync()
        {
            string? jsonString = await BaseApiCallAsync<string>($"event-subscriptions", Method.Get);
            if (jsonString is null) return null;
            List<LexResponseDefault>? response = JsonConvert.DeserializeObject<LexResponseWrapper>(jsonString)?.Content;
            return response;
        }
        
        /// <summary>
        /// Deletes a single event subscription by its id
        /// Docs: <seealso href="https://developers.lexoffice.io/docs/#event-subscriptions-endpoint-delete-an-event-subscription"/>
        /// </summary>
        /// <param name="id">The id of the event to be deleted</param>
        /// <returns><seealso cref="LexResponseDefault"/></returns>
        public Task DeleteEventSubscriptionAsync(Guid? id) => BaseApiCallAsync<string>($"event-subscriptions/{id}", Method.Delete);
#else

        /// <summary>
        /// Adds a new event subscription to lexoffice
        /// Docs: <seealso href="https://developers.lexoffice.io/docs/#event-subscriptions-endpoint-create-an-event-subscription"/>
        /// </summary>
        /// <param name="lexQuotation">The quotation as <seealso cref="LexResponseDefault"/></param>
        /// <returns><seealso cref="LexResponseDefault"/></returns>
        public async Task<LexResponseDefault?> AddEventSubscriptionAsync(LexResponseDefault lexQuotation)
        {
            IRestApiRequestRespone? result = null;
            LexResponseDefault? resultObject = null;
            try
            {
                string json = JsonConvert.SerializeObject(lexQuotation, NewtonsoftJsonSerializerSettings);
                string targetUri = $"event-subscriptions";
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
        /// Gets a single event subscription by its id
        /// Docs: <seealso href="https://developers.lexoffice.io/docs/#event-subscriptions-endpoint-retrieve-an-event-subscription"/>
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns><seealso cref="LexResponseDefault"/></returns>
        public async Task<LexResponseDefault?> GetEventSubscriptionAsync(Guid? id)
        {
            IRestApiRequestRespone? result = null;
            LexResponseDefault? resultObject = null;
            try
            {
                string targetUri = $"event-subscriptions/{id}";
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
        /// Gets a list of all event subscriptions.
        /// Docs: <seealso href="https://developers.lexoffice.io/docs/#event-subscriptions-endpoint-retrieve-all-event-subscriptions"/>
        /// </summary>
        /// <returns>List of <seealso cref="LexResponseDefault"/></returns>
        public async Task<List<LexResponseDefault>?> GetAllEventSubscriptionsAsync()
        {
            IRestApiRequestRespone? result = null;
            List<LexResponseDefault>? resultObject = [];
            try
            {
                string targetUri = $"event-subscriptions";
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
                resultObject = GetObjectFromJson<LexResponseWrapper>(result?.Result, NewtonsoftJsonSerializerSettings)?.Content;
                return resultObject;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return resultObject;
            }
        }

        /// <summary>
        /// Deletes a single event subscription by its id
        /// Docs: <seealso href="https://developers.lexoffice.io/docs/#event-subscriptions-endpoint-delete-an-event-subscription"/>
        /// </summary>
        /// <param name="id">The id of the event to be deleted</param>
        /// <returns><seealso cref="LexResponseDefault"/></returns>
        public async Task<LexResponseDefault?> DeleteEventSubscriptionAsync(Guid id)
        {
            IRestApiRequestRespone? result = null;
            LexResponseDefault? resultObject = null;
            try
            {
                string targetUri = $"event-subscriptions/{id}";
                result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Delete,
                       command: "",
                       jsonObject: null,
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
