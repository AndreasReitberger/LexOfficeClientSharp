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
        #region Subscription
        public async Task<LexResponseDefault?> AddEventSubscriptionAsync(LexResponseDefault lexQuotation)
        {
            string json = JsonConvert.SerializeObject(lexQuotation, JsonSerializerSettings) ?? string.Empty;
            string? jsonString = await BaseApiCallAsync<string>($"event-subscriptions", Method.Post, json);
            if (jsonString is null) return null;
            LexResponseDefault? response = JsonConvert.DeserializeObject<LexResponseDefault>(jsonString);
            return response;
        }

        public async Task<LexResponseDefault?> GetEventSubscriptionAsync(Guid? id)
        {
            string? jsonString = await BaseApiCallAsync<string>($"event-subscriptions/{id}");
            if (jsonString is null) return null;
            LexResponseDefault? response = JsonConvert.DeserializeObject<LexResponseDefault>(jsonString);
            return response;
        }

        public async Task<List<LexResponseDefault>?> GetAllEventSubscriptionsAsync()
        {
            string? jsonString = await BaseApiCallAsync<string>($"event-subscriptions", Method.Get);
            if (jsonString is null) return null;
            List<LexResponseDefault>? response = JsonConvert.DeserializeObject<LexResponseWrapper>(jsonString)?.Content;
            return response;
        }

        public Task DeleteEventSubscriptionAsync(Guid? id) => BaseApiCallAsync<string>($"event-subscriptions/{id}", Method.Delete);
        #endregion
    }
}
