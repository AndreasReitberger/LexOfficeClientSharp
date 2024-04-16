using AndreasReitberger.Core.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading;
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

        #region Variable
        RestClient? restClient;
        HttpClient? httpClient;

        const string _appBaseUrl = "https://api.lexoffice.io/";
        const string _apiVersion = "v1";
        #endregion

        #region Properties

        #region General

        [ObservableProperty]
        SecureString? accessToken = null;
        partial void OnAccessTokenChanged(SecureString? value) => VerifyAccessToken();

        [ObservableProperty]
        bool isConnecting = false;
        [JsonIgnore, XmlIgnore]

        [ObservableProperty]
        bool isOnline = false;

        [ObservableProperty]
        bool isAccessTokenValid = false;

        [ObservableProperty]
        int defaultTimeout = 10000;

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
        SecureString? proxyPassword;
        partial void OnProxyPasswordChanged(SecureString? value) => UpdateRestClientInstance();

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
        public LexOfficeClient(SecureString accessToken)
        {
            AccessToken = accessToken;
            IsInitialized = true;
            Instance = this;
        }
        #endregion

        #region Methods
        void UpdateRestClientInstance()
        {
            if (string.IsNullOrEmpty(_appBaseUrl) || string.IsNullOrEmpty(_apiVersion))
            {
                return;
            }
            if (EnableProxy && !string.IsNullOrEmpty(ProxyAddress))
            {
                RestClientOptions options = new($"{_appBaseUrl}{_apiVersion}/")
                {
                    ThrowOnAnyError = true,
                    MaxTimeout = 10000,
                };
                HttpClientHandler httpHandler = new()
                {
                    UseProxy = true,
                    Proxy = GetCurrentProxy(),
                    AllowAutoRedirect = true,
                };

                httpClient = new(handler: httpHandler, disposeHandler: true);
                restClient = new(httpClient: httpClient, options: options);
            }
            else
            {
                httpClient = null;
                restClient = new(baseUrl: $"{_appBaseUrl}{_apiVersion}/");
            }
        }

        async Task<string?> BaseApiCallAsync(string command, Method method = Method.Get, CancellationTokenSource? cts = default)
        {
            string? result = string.Empty;
            if (cts == default)
            {
                cts = new(DefaultTimeout);
            }
            if (restClient is null)
            {
                UpdateRestClientInstance();
            }

            RestRequest request = new(command, method)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddHeader("Authorization", $"Bearer {SecureStringHelper.ConvertToString(AccessToken)}");
            if (restClient is not null)
            {
                RestResponse? respone = await restClient.ExecuteAsync(request, cts.Token).ConfigureAwait(false);
                if (respone.StatusCode == HttpStatusCode.OK && respone.ResponseStatus == ResponseStatus.Completed)
                {
                    result = respone?.Content;
                }
            }
            return result;
        }

        void VerifyAccessToken()
        {
            try
            {
                string cleaned = SecureStringHelper.ConvertToString(AccessToken);
                IsAccessTokenValid = Regex.IsMatch(cleaned ?? string.Empty, RegexHelper.LexOfficeAccessToken) && !string.IsNullOrEmpty(cleaned);
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
        public void SetAccessToken(SecureString token)
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
                string uriString = $"{_appBaseUrl}";
                try
                {
                    if (httpClient is not null)
                    {
                        HttpResponseMessage? response = await httpClient.GetAsync(uriString, cts.Token).ConfigureAwait(false);
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
        public async Task<ObservableCollection<LexContact>> GetContactsAsync(LexContactType type, int page = 0, int size = 25, int coolDown = 20)
        {
            ObservableCollection<LexContact> result = [];

            string cmd = string.Format("contacts{0}",
                type == LexContactType.Customer ? "?customer=true" : "?vendor=true"              
                );
            cmd += $"&page={page}&size={size}";

            string? jsonString = await BaseApiCallAsync(cmd, Method.Get) ?? string.Empty;
            LexContactsList? contacts = JsonConvert.DeserializeObject<LexContactsList>(jsonString);
            if (contacts != null)
            {
                result = new ObservableCollection<LexContact>(contacts.Content);
                if (page < contacts.TotalPages)
                {
                    page++;
                    ObservableCollection<LexContact> append = await GetContactsAsync(type, page, size);
                    result = new ObservableCollection<LexContact>(result.Concat(append));
                    await Task.Delay(coolDown < 20 ? 20 : coolDown);
                    return result;
                }
            }
            return result;
        }

        public async Task<LexContact?> GetContactAsync(Guid id)
        {
            string? jsonString = await BaseApiCallAsync($"contacts/{id}", Method.Get) ?? string.Empty;
            LexContact? contact = JsonConvert.DeserializeObject<LexContact>(jsonString);
            return contact;
        }
        #endregion

        #region Invoices
        public async Task<ObservableCollection<VoucherListContent>> GetInvoiceListAsync(LexVoucherStatus status, bool archived = false, int page = 0, int size = 25)
        {
            ObservableCollection<VoucherListContent> result = [];

            string type = LexVoucherType.invoice.ToString();
            string cmd = string.Format("voucherlist?{0}&{1}&{2}",
                $"voucherType={type}",
                $"voucherStatus={status}",
                $"archived={archived}"
                );
            cmd += $"&page={page}&size={size}";

            string? jsonString = await BaseApiCallAsync(cmd, Method.Get) ?? string.Empty;
            LexVoucherList? list = JsonConvert.DeserializeObject<LexVoucherList>(jsonString);
            if (list is not null)
            {
                if (page < list.TotalPages - 1)
                {
                    page++;
                    result = new ObservableCollection<VoucherListContent>(list.Content);
                    ObservableCollection<VoucherListContent> append = await GetInvoiceListAsync(status, archived, page, size);
                    result = new ObservableCollection<VoucherListContent>(result.Concat(append));
                    return result;
                }
            }
            return result;
        }

        public async Task<ObservableCollection<LexQuotation>> GetInvoicesAsync(List<Guid> ids)
        {
            ObservableCollection<LexQuotation> result = [];
            foreach (Guid Id in ids)
            {
                LexQuotation? quote = await GetInVoiceAsync(Id);
                if (quote is not null)
                    result.Add(quote);
            }
            return result;
        }
        public async Task<ObservableCollection<LexQuotation>> GetInvoicesAsync(ObservableCollection<VoucherListContent> voucherList)
        {
            List<Guid> ids = voucherList.Select(id => id.Id).ToList();
            return await GetInvoicesAsync(ids);
        }

        public async Task<LexQuotation?> GetInVoiceAsync(Guid id)
        {
            string? jsonString = await BaseApiCallAsync($"invoices/{id}", Method.Get) ?? string.Empty;
            LexQuotation? contact = JsonConvert.DeserializeObject<LexQuotation>(jsonString);
            return contact;
        }
        #endregion

        #region Quotations
        public async Task<ObservableCollection<VoucherListContent>> GetQuotationListAsync(LexVoucherStatus status, bool archived = false, int page = 0, int size = 25)
        {
            ObservableCollection<VoucherListContent> result = [];

            string type = LexVoucherType.quotation.ToString();
            string cmd = string.Format("voucherlist?{0}&{1}&{2}",
                $"voucherType={type}",
                $"voucherStatus={status}",
                $"archived={archived}"
                );
            cmd += $"&page={page}&size={size}";

            string? jsonString = await BaseApiCallAsync(cmd, Method.Get) ?? string.Empty;
            LexVoucherList? list = JsonConvert.DeserializeObject<LexVoucherList>(jsonString);
            if (list is not null)
            {
                if (page < list.TotalPages - 1)
                {
                    page++;
                    result = new ObservableCollection<VoucherListContent>(list.Content);
                    ObservableCollection<VoucherListContent> append = await GetQuotationListAsync(status, archived, page, size);
                    result = new ObservableCollection<VoucherListContent>(result.Concat(append));
                    return result;
                }
            }
            return result;
        }

        public async Task<ObservableCollection<LexQuotation>> GetQuotationsAsync(List<Guid> ids)
        {
            ObservableCollection<LexQuotation> result = [];
            foreach (Guid Id in ids)
            {
                LexQuotation? quote = await GetQuotationAsync(Id);
                if (quote is not null)
                    result.Add(quote);
            }
            return result;
        }
        public async Task<ObservableCollection<LexQuotation>> GetQuotationsAsync(ObservableCollection<VoucherListContent> voucherList)
        {
            List<Guid> ids = voucherList.Select(id => id.Id).ToList();
            return await GetQuotationsAsync(ids);
        }

        public async Task<LexQuotation?> GetQuotationAsync(Guid id)
        {
            string? jsonString = await BaseApiCallAsync($"quotations/{id}", Method.Get) ?? string.Empty;
            LexQuotation? contact = JsonConvert.DeserializeObject<LexQuotation>(jsonString);
            return contact;
        }
        #endregion

        #endregion
    }
}
