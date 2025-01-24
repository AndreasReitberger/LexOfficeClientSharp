
#if NETFRAMEWORK
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Xml.Serialization;

namespace AndreasReitberger.API.LexOffice
{
    /*  .NET Framework is not supported by the RestApiClient, so keep this properties for know in a separat file.
     *  In the future, the support of .NET Framework will be dropped.
     */
    // https://developers.lexoffice.io/docs/#lexoffice-api-documentation/?cid=1766
    public partial class LexOfficeClient : ObservableObject
    {

        #region Properties

        [ObservableProperty]
        public partial bool IsActive { get; set; } = false;

        [ObservableProperty]
        public partial bool IsInitialized { get; set; } = false;

        [ObservableProperty]
        [JsonIgnore, XmlIgnore]
        public partial RestClient? RestClient { get; set; }

        [ObservableProperty]
        [JsonIgnore, XmlIgnore]
        public partial HttpClient? HttpClient { get; set; }

        [ObservableProperty]
        public partial bool UpdatingClients { get; set; } = false;

        [ObservableProperty]
        public partial string ApiTargetPath { get; set; } = "https://api.lexoffice.io/";
        partial void OnApiTargetPathChanged(string value) => UpdateRestClientInstance();

        [ObservableProperty]
        public partial string ApiVersion { get; set; } = "v1";
        partial void OnApiVersionChanged(string value) => UpdateRestClientInstance();

        [ObservableProperty]
        [JsonIgnore, XmlIgnore]
        public partial bool IsConnecting { get; set; } = false;

        [field: JsonIgnore, XmlIgnore]

        [ObservableProperty]
        [JsonIgnore, XmlIgnore]
        public partial bool IsOnline { get; set; } = false;

        [ObservableProperty]
        [JsonIgnore, XmlIgnore]
        public partial bool IsAccessTokenValid { get; set; } = false;

        [ObservableProperty]
        public partial int DefaultTimeout { get; set; } = 10000;

        [ObservableProperty]
        public partial int MinimumCooldown { get; set; } = 0;
        #endregion

        #region Methods
        public async Task CheckOnlineAsync(int timeout = 10000)
        {
            if (IsConnecting) return; // Avoid multiple calls
            IsConnecting = true;
            bool isReachable = false;
            try
            {
                // Cancel after timeout
                CancellationTokenSource cts = new(new TimeSpan(0, 0, 0, 0, timeout));
                string uriString = $"{ApiTargetPath}";
                try
                {
                    if (HttpClient is not null)
                    {
                        HttpResponseMessage? response = await HttpClient.GetAsync(uriString, cts.Token).ConfigureAwait(false);
                        response.EnsureSuccessStatusCode();
                        if (response != null)
                        {
                            isReachable = response.IsSuccessStatusCode;
                        }
                    }
                }
                catch (InvalidOperationException iexc)
                {
                    OnError(new UnhandledExceptionEventArgs(iexc, false));
                }
                catch (HttpRequestException rexc)
                {
                    OnError(new UnhandledExceptionEventArgs(rexc, false));
                }
                catch (TaskCanceledException)
                {
                    // Throws exception on timeout, not actually an error
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
            IsConnecting = false;
            IsOnline = isReachable;
            //return isReachable;
        }

        void UpdateRestClientInstance()
        {
            if (string.IsNullOrEmpty(ApiTargetPath) || string.IsNullOrEmpty(ApiVersion) || UpdatingClients)
            {
                return;
            }
            UpdatingClients = true;
            RestClientOptions options = new($"{ApiTargetPath}{ApiVersion}/")
            {
                ThrowOnAnyError = false,
                Timeout = TimeSpan.FromSeconds(DefaultTimeout / 1000),
            };
            if (EnableProxy && !string.IsNullOrEmpty(ProxyAddress))
            {
                HttpClientHandler httpHandler = new()
                {
                    UseProxy = true,
                    Proxy = GetCurrentProxy(),
                    AllowAutoRedirect = true,
                };

                HttpClient = new(handler: httpHandler, disposeHandler: true);
            }
            else
            {
                HttpClient = new();
            }
            RestClient = new(httpClient: HttpClient, options: options);
            UpdatingClients = false;
        }
        #endregion
    }
}
#endif