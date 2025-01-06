using AndreasReitberger.API.LexOffice.Enum;
using AndreasReitberger.API.LexOffice.Model.Response;
using AndreasReitberger.API.LexOffice.Utilities;
using AndreasReitberger.Core.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class LexOfficeClient : ObservableObject
    {

        #region Instance
        static LexOfficeClient? _instance = null;
        static readonly object Lock = new();
        public static LexOfficeClient Instance
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

        [ObservableProperty]
        bool isActive = false;

        [ObservableProperty]
        bool isInitialized = false;

        #endregion

        #region Static
        public static string HandlerName = "LexOffice";
        public static string HandlerLicenseUri = "https://www.lexoffice.de/public-api-lizenz-nutzungsbedingungen/";
        #endregion

        #region Properties

        #region Clients

        [ObservableProperty]
        [property: JsonIgnore, XmlIgnore]
        RestClient? restClient;
        //partial void OnRestClientChanged(RestClient? value) => UpdateRestClientInstance();

        [ObservableProperty]
        [property: JsonIgnore, XmlIgnore]
        HttpClient? httpClient;
        //partial void OnHttpClientChanged(HttpClient? value) => UpdateRestClientInstance();

#if !NETFRAMEWORK
        [ObservableProperty]
        [property: JsonIgnore, XmlIgnore]
        RateLimitedHandler? rateLimitedHandler;

        public static RateLimiter DefaultLimiter = new TokenBucketRateLimiter(new()
        {
            TokenLimit = 2,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = int.MaxValue,
            ReplenishmentPeriod = TimeSpan.FromSeconds(1),
            TokensPerPeriod = 1,
            AutoReplenishment = true,
        });

        [ObservableProperty]
        [property: JsonIgnore, XmlIgnore]
        RateLimiter? limiter;
        partial void OnLimiterChanged(RateLimiter? value) => UpdateRestClientInstance();
#endif
        [ObservableProperty]
        bool updatingClients = false;

        [ObservableProperty]
        string appBaseUrl = "https://api.lexoffice.io/";
        partial void OnAppBaseUrlChanged(string value) => UpdateRestClientInstance();

        [ObservableProperty]
        string apiVersion = "v1";
        partial void OnApiVersionChanged(string value) => UpdateRestClientInstance();
        #endregion

        #region SerializerSettings

        [ObservableProperty]
        JsonSerializerSettings jsonSerializerSettings = new()
        {
            Formatting = Formatting.Indented,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy(),
            },
            DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffK"
        };

        #endregion

        #region General

        [ObservableProperty]
        [property: JsonIgnore, XmlIgnore]
        string? accessToken = null;
        partial void OnAccessTokenChanged(string? value) => VerifyAccessToken();

        [ObservableProperty]
        [property: JsonIgnore, XmlIgnore]
        bool isConnecting = false;
        [JsonIgnore, XmlIgnore]

        [ObservableProperty]
        [property: JsonIgnore, XmlIgnore]
        bool isOnline = false;

        [ObservableProperty]
        [property: JsonIgnore, XmlIgnore]
        bool isAccessTokenValid = false;

        [ObservableProperty]
        int defaultTimeout = 10000;

        [ObservableProperty]
        int minimumCooldown = 0;

        #endregion

        #region Proxy

        [ObservableProperty]
        bool enableProxy = false;
        partial void OnEnableProxyChanged(bool value) => UpdateRestClientInstance();

        [ObservableProperty]
        bool proxyUseDefaultCredentials = true;
        partial void OnProxyUseDefaultCredentialsChanged(bool value) => UpdateRestClientInstance();

        [ObservableProperty]
        bool secureProxyConnection = true;
        partial void OnSecureProxyConnectionChanged(bool value) => UpdateRestClientInstance();

        [ObservableProperty]
        string proxyAddress = string.Empty;
        partial void OnProxyAddressChanged(string value) => UpdateRestClientInstance();

        [ObservableProperty]
        int proxyPort = 443;
        partial void OnProxyPortChanged(int value) => UpdateRestClientInstance();

        [ObservableProperty]
        string proxyUser = string.Empty;
        partial void OnProxyUserChanged(string value) => UpdateRestClientInstance();

        [ObservableProperty]
        [property: JsonIgnore, XmlIgnore]
        string? proxyPassword;
        partial void OnProxyPasswordChanged(string? value) => UpdateRestClientInstance();

        #endregion

        #endregion

        #region EventHandlers
        public event EventHandler? Error;
        protected virtual void OnError()
        {
            Error?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnError(UnhandledExceptionEventArgs e)
        {
            Error?.Invoke(this, e);
        }
        #endregion

        #region Constructor
        public LexOfficeClient()
        {
            IsInitialized = false;
        }
        public LexOfficeClient(string accessToken)
        {
            AccessToken = accessToken;
            IsInitialized = true;
            Instance = this;
        }
        #endregion

        #region Methods
        void UpdateRestClientInstance()
        {
            if (string.IsNullOrEmpty(AppBaseUrl) || string.IsNullOrEmpty(ApiVersion) || UpdatingClients)
            {
                return;
            }
            UpdatingClients = true;
#if !NETFRAMEWORK
            Limiter ??= DefaultLimiter;
#endif
            RestClientOptions options = new($"{AppBaseUrl}{ApiVersion}/")
            {
                ThrowOnAnyError = true,
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
                //RestClient = new(httpClient: HttpClient, options: options);
            }
            else
            {
                HttpClient =
#if !NETFRAMEWORK
                    new(new RateLimitedHandler(Limiter));
#else
                    new();
#endif
                //RestClient = new(baseUrl: $"{AppBaseUrl}{ApiVersion}/");
            }
            RestClient = new(httpClient: HttpClient, options: options);
            UpdatingClients = false;
        }

        async Task<T?> BaseApiCallAsync<T>(string command, Method method = Method.Get, string body = "", CancellationTokenSource? cts = default)  where T : class
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
            catch(Exception exc)
            {
                IsAccessTokenValid = false;
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
#endregion

        #region Public Methods

        #region Proxy
        Uri GetProxyUri() => 
            ProxyAddress.StartsWith("http://") || ProxyAddress.StartsWith("https://") ? new Uri($"{ProxyAddress}:{ProxyPort}") : new Uri($"{(SecureProxyConnection ? "https" : "http")}://{ProxyAddress}:{ProxyPort}");
        
        WebProxy GetCurrentProxy()
        {
            WebProxy proxy = new()
            {
                Address = GetProxyUri(),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = ProxyUseDefaultCredentials,
            };
            if (ProxyUseDefaultCredentials && !string.IsNullOrEmpty(ProxyUser))
            {
                proxy.Credentials = new NetworkCredential(ProxyUser, ProxyPassword);
            }
            else
            {
                proxy.UseDefaultCredentials = ProxyUseDefaultCredentials;
            }
            return proxy;
        }
        #endregion

        #region SetAccessToken
        public void SetAccessToken(string token)
        {
            AccessToken = token;
            IsInitialized = true;
        }
        #endregion

        #region OnlineCheck
        public async Task CheckOnlineAsync(int timeout = 10000)
        {
            if (IsConnecting) return; // Avoid multiple calls
            IsConnecting = true;
            bool isReachable = false;
            try
            {
                // Cancel after timeout
                CancellationTokenSource cts = new(new TimeSpan(0, 0, 0, 0, timeout));
                string uriString = $"{AppBaseUrl}";
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
        #endregion

        #region Contacts
        public async Task<List<LexContact>> GetContactsAsync(LexContactType type, int page = 0, int size = 25, int pages = -1, int cooldown = 0)
        {
            List<LexContact> result = [];
            string cmd = $"contacts?{(type == LexContactType.Customer ? "customer" : "vendor")}=true";
            cmd += $"&page={page}&size={size}";

            string? jsonString = await BaseApiCallAsync<string>(cmd, Method.Get) ?? string.Empty;
            LexContactsList? list = JsonConvert.DeserializeObject<LexContactsList>(jsonString);
            if (list != null)
            {
                if (list.TotalPages > 1 && page < list.TotalPages &&  (pages <= 0 || (pages - 1 > page && pages > 1)))
                {
                    result = new List<LexContact>(list.Content);
                    if(MinimumCooldown > 0 && cooldown > 0)
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
            string? jsonString = await BaseApiCallAsync<string>($"contacts", Method.Post, JsonConvert.SerializeObject(lexContact, JsonSerializerSettings)) ?? string.Empty;
            LexResponseDefault? response = JsonConvert.DeserializeObject<LexResponseDefault>(jsonString);
            return response;
        }

        public async Task<LexResponseDefault?> UpdateContactAsync(Guid contactId, LexContact lexContact)
        {
            string? jsonString = await BaseApiCallAsync<string>($"contacts/{contactId}", Method.Post, JsonConvert.SerializeObject(lexContact, JsonSerializerSettings)) ?? string.Empty;
            LexResponseDefault? response = JsonConvert.DeserializeObject<LexResponseDefault>(jsonString);
            return response;
        }
        #endregion

        #region Countries

        public async Task<List<LexCountry>> GetCountriesAsync()
        {
            List<LexCountry> result = [];
            string? jsonString = await BaseApiCallAsync<string>("countries", Method.Get) ?? string.Empty;
            result = JsonConvert.DeserializeObject<List<LexCountry>>(jsonString) ?? [];
            return result;
        }
        #endregion

        #region Credit Notes

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
            var body = JsonConvert.SerializeObject(lexQuotation, JsonSerializerSettings);
            string? jsonString = await BaseApiCallAsync<string>($"credit-notes/?finalize={isFinalized}", Method.Post, body) ?? string.Empty;
            LexResponseDefault? response = JsonConvert.DeserializeObject<LexResponseDefault>(jsonString);
            return response;
        }
        #endregion

        #region Conditions

        public async Task<List<LexQuotationPaymentConditions>> GetPaymentConditionsAsync()
        {
            List<LexQuotationPaymentConditions> result = [];
            string? jsonString = await BaseApiCallAsync<string>($"payment-conditions", Method.Get) ?? string.Empty;
            result = JsonConvert.DeserializeObject<List<LexQuotationPaymentConditions>>(jsonString) ?? [];
            return result;
        }
        #endregion

        #region Invoices
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
            string? jsonString = await BaseApiCallAsync<string>($"invoices?finalize={isFinalized}", Method.Post, JsonConvert.SerializeObject(lexQuotation, JsonSerializerSettings)) ?? string.Empty;
            LexResponseDefault? response = JsonConvert.DeserializeObject<LexResponseDefault>(jsonString);
            return response;
        }
        #endregion

        public async Task<LexResponseDefault?> AddEventSubscriptionAsync(LexDocumentResponse lexQuotation)
        {
            var json = JsonConvert.SerializeObject(lexQuotation, JsonSerializerSettings) ?? string.Empty;
            string? jsonString = await BaseApiCallAsync<string>($"event-subscriptions", Method.Post, json);
            LexResponseDefault? response = JsonConvert.DeserializeObject<LexResponseDefault>(jsonString);
            return response;
        }

        public async Task<LexResponseDefault?> GetEventSubscriptionAsync(Guid? id)
        {
            string? jsonString = await BaseApiCallAsync<string>($"event-subscriptions/{id}");
            LexResponseDefault? response = JsonConvert.DeserializeObject<LexResponseDefault>(jsonString);
            return response;
        }

        public async Task<List<LexResponseDefault>?> GetAllEventSubscriptionsAsync()
        {
            string? jsonString = await BaseApiCallAsync<string>($"event-subscriptions", Method.Get);
            List<LexResponseDefault>? response = JsonConvert.DeserializeObject<LexResponseWrapper>(jsonString).Content;
            return response;
        }

        public async Task DeleteEventSubscriptionAsync(Guid? id)
        {
            await BaseApiCallAsync<string>($"event-subscriptions/{id}", Method.Delete);
        }

        #region Payments
        public async Task<LexPayments?> GetPaymentsAsync(Guid invoiceId)
        {
            string? jsonString = await BaseApiCallAsync<string>($"payments/{invoiceId}", Method.Get) ?? string.Empty;
            LexPayments? response = JsonConvert.DeserializeObject<LexPayments>(jsonString);
            return response;
        }
        #endregion

        #region Quotations
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
            List<Guid> ids = voucherList.Select(id => id.Id).ToList();
            return await GetQuotationsAsync(ids);
        }

        public async Task<LexDocumentResponse?> GetQuotationAsync(Guid id)
        {
            string? jsonString = await BaseApiCallAsync<string>($"quotations/{id}", Method.Get) ?? string.Empty;
            LexDocumentResponse? response = JsonConvert.DeserializeObject<LexDocumentResponse>(jsonString);
            return response;
        }
        #endregion

        #region Documents

        public async Task<LexQuotationFiles?> RenderDocumentAsync(Guid invoiceId)
        {
            string? jsonString = await BaseApiCallAsync<string>($"invoices/{invoiceId}/document", Method.Get) ?? string.Empty;
            LexQuotationFiles? response = JsonConvert.DeserializeObject<LexQuotationFiles>(jsonString);
            return response;
        }

        public async Task<byte[]> GetFileAsync(Guid id)
        {
            byte[] response = await BaseApiCallAsync<byte[]>($"files/{id}", Method.Get) ?? [];
            return response;
        }

        #endregion
        #endregion
    }
}
