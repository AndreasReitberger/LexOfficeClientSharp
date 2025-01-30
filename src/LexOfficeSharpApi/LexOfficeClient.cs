#if !NETFRAMEWORK
using AndreasReitberger.API.REST;
using AndreasReitberger.API.REST.Enums;
#else
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
#endif
using AndreasReitberger.Core.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using RestSharp;
using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.RateLimiting;
using System.Threading.Tasks;
using System.Xml.Serialization;

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

        #region Instance
        static LexOfficeClient? _instance = null;
        static readonly object Lock = new();
#if NETFRAMEWORK
        public static LexOfficeClient Instance
#else
        public new static LexOfficeClient Instance
#endif
        {
            get
            {
                lock (Lock)
                {
                    _instance ??= new LexOfficeClient();
                }
                return _instance;
            }
            set
            {
                if (_instance == value) return;
                lock (Lock)
                {
                    _instance = value;
                }
            }
        }

        #endregion

        #region Static
        public static string HandlerName = "LexOffice";
        public static string HandlerLicenseUri = "https://www.lexoffice.de/public-api-lizenz-nutzungsbedingungen/";
        #endregion

        #region Properties

        #region SerializerSettings

#if NETFRAMEWORK
        [ObservableProperty]
        public partial JsonSerializerSettings NewtonsoftJsonSerializerSettings { get; set; } = new()
        {
            Formatting = Formatting.Indented,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy(),
            },
            DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffK"
        };
#endif
        #endregion

        #region General

#if !NETFRAMEWORK
        // A client can make up to 2 requests per second to the lexoffice API.
        public new static RateLimiter DefaultLimiter = new TokenBucketRateLimiter(new()
        {
            TokenLimit = 2,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = int.MaxValue,
            ReplenishmentPeriod = TimeSpan.FromSeconds(1.5),
            TokensPerPeriod = 2,
            AutoReplenishment = true,
        });
#endif

#if NETFRAMEWORK
        [ObservableProperty]
        [JsonIgnore, XmlIgnore]
        public partial string? AccessToken { get; set; } = null;
        partial void OnAccessTokenChanged(string? value) => VerifyAccessToken();
#endif
        #endregion

        #endregion

        #region EventHandlers
#if NETFRAMEWORK
        public event EventHandler? Error;
        protected virtual void OnError()
        {
            Error?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnError(UnhandledExceptionEventArgs e)
        {
            Error?.Invoke(this, e);
        }
#endif
        #endregion

        #region Constructor
        public LexOfficeClient() 
        {
            IsInitialized = false;
            ApiTargetPath ??= "https://api.lexoffice.io/";
            ApiVersion = "v1";
#if !NETFRAMEWORK
            Limiter ??= DefaultLimiter;
#endif
        }
#if NETFRAMEWORK
        public LexOfficeClient(string accessToken) : this()
        {
            AccessToken = accessToken;
            IsInitialized = true;
            Instance = this;
        }
#else
        public LexOfficeClient(string accessToken, string tokenName = "Authorization", string url = "https://api.lexoffice.io/", string version = "v1") : base(authHeader: new AuthenticationHeader()
        {
            Target = AuthenticationHeaderTarget.Header,
            Token = $"Bearer {accessToken}",
        }, tokenName: tokenName, url: url, version: version)
        {
#if !NETFRAMEWORK
            Limiter ??= DefaultLimiter;
#endif
            //AccessToken = accessToken;
            IsInitialized = true;
            Instance = this;
        }
#endif
#endregion

        #region Methods
#if NETFRAMEWORK
        [Obsolete("In the future, use `SendRestApiRequestAsync` from the `RestApiClient`!")]
        async Task<T?> BaseApiCallAsync<T>(string command, Method method = Method.Get, string body = "", CancellationTokenSource? cts = default) where T : class
        {
            if (cts == default)
            {
                cts = new(DefaultTimeout);
            }
            if (RestClient is null)
            {
                UpdateRestClientInstance();
            }

            RestRequest request = new(command, method)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddHeader("Authorization", $"Bearer {AccessToken}");
            if (!string.IsNullOrEmpty(body))
            {
                request.AddJsonBody(body);
            }
            if (RestClient is not null)
            {
                RestResponse? response = await RestClient.ExecuteAsync(request, cts.Token).ConfigureAwait(false);

                if ((response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created) &&
                    response.ResponseStatus == ResponseStatus.Completed)
                {
                    if (typeof(T) == typeof(byte[]))
                    {
                        return response.RawBytes as T;
                    }
                    else if (typeof(T) == typeof(string))
                    {
                        return response.Content as T;
                    }
                    else
                    {
                        throw new InvalidOperationException($"Unsupported return type: {typeof(T).Name}");
                    }
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return default;
                }
                else
                {
                    string errorMessage = $"Request failed with status code {(int)response.StatusCode} ({response.StatusCode}).";

                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        errorMessage += $" Response content: {response.Content}";
                    }

                    throw new HttpRequestException(errorMessage);
                }
            }
            return default;
        }

        void VerifyAccessToken()
        {
            try
            {
                IsAccessTokenValid = Regex.IsMatch(AccessToken ?? string.Empty, RegexHelper.LexOfficeAccessToken) && !string.IsNullOrEmpty(AccessToken);
            }
            catch (Exception exc)
            {
                IsAccessTokenValid = false;
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
#endif
#endregion
    }
}
