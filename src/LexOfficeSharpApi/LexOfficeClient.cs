using AndreasReitberger.Core.Utilities;
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
    // https://docs.microsoft.com/en-us/dotnet/standard/library-guidance/cross-platform-targeting
    // https://developers.lexoffice.io/docs/#lexoffice-api-documentation/?cid=1766
    public class LexOfficeClient : BaseModel
    {

        #region Instance
        static LexOfficeClient _instance = null;
        static readonly object Lock = new object();
        public static LexOfficeClient Instance
        {
            get
            {
                lock (Lock)
                {
                    if (_instance == null)
                        _instance = new LexOfficeClient();
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

        bool _isActive = false;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive == value)
                    return;
                _isActive = value;
                OnPropertyChanged();
            }
        }

        bool _isInitialized = false;
        public bool IsInitialized
        {
            get => _isInitialized;
            set
            {
                if (_isInitialized == value)
                    return;
                _isInitialized = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Static
        public static string HandlerName = "LexOffice";
        public static string HandlerLicenseUri = "https://www.lexoffice.de/public-api-lizenz-nutzungsbedingungen/";
        [Obsolete("Use httpClient instead")]
        static readonly HttpClient client = new();
        #endregion

        #region Variable
        RestClient restClient;
        HttpClient httpClient;

        const string _appBaseUrl = "https://api.lexoffice.io/";
        const string _apiVersion = "v1";
        #endregion

        #region Properties

        #region General

        [JsonProperty(nameof(AccessToken))]
        [XmlAttribute(nameof(AccessToken))]
        SecureString _accessToken = null;
        [JsonIgnore, XmlIgnore]
        public SecureString AccessToken
        {
            get => _accessToken;
            set
            {
                if (_accessToken == value) return;
                _accessToken = value;
                VerifyAccessToken();
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(IsConnecting))]
        [XmlAttribute(nameof(IsConnecting))]
        bool _isConnecting = false;
        [JsonIgnore, XmlIgnore]
        public bool IsConnecting
        {
            get => _isConnecting;
            set
            {
                if (_isConnecting == value) return;
                _isConnecting = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(IsOnline))]
        [XmlAttribute(nameof(IsOnline))]
        bool _isOnline = false;
        [JsonIgnore, XmlIgnore]
        public bool IsOnline
        {
            get => _isOnline;
            set
            {
                if (_isOnline == value) return;
                _isOnline = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(IsAccessTokenValid))]
        [XmlAttribute(nameof(IsAccessTokenValid))]
        bool _isAccessTokenValid = false;
        [JsonIgnore, XmlIgnore]
        public bool IsAccessTokenValid
        {
            get => _isAccessTokenValid;
            set
            {
                if (_isAccessTokenValid == value) return;
                _isAccessTokenValid = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(DefaultTimeout))]
        int _defaultTimeout = 10000;
        [JsonIgnore, XmlIgnore]
        public int DefaultTimeout
        {
            get => _defaultTimeout;
            set
            {
                if (_defaultTimeout != value)
                {
                    _defaultTimeout = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Proxy
        [JsonProperty(nameof(EnableProxy))]
        [XmlAttribute(nameof(EnableProxy))]
        bool _enableProxy = false;
        [JsonIgnore, XmlIgnore]
        public bool EnableProxy
        {
            get => _enableProxy;
            set
            {
                if (_enableProxy == value) return;
                _enableProxy = value;
                OnPropertyChanged();
                UpdateRestClientInstance();
            }
        }

        [JsonProperty(nameof(ProxyUseDefaultCredentials))]
        [XmlAttribute(nameof(ProxyUseDefaultCredentials))]
        bool _proxyUseDefaultCredentials = true;
        [JsonIgnore, XmlIgnore]
        public bool ProxyUseDefaultCredentials
        {
            get => _proxyUseDefaultCredentials;
            set
            {
                if (_proxyUseDefaultCredentials == value) return;
                _proxyUseDefaultCredentials = value;
                OnPropertyChanged();
                UpdateRestClientInstance();
            }
        }

        [JsonProperty(nameof(SecureProxyConnection))]
        [XmlAttribute(nameof(SecureProxyConnection))]
        bool _secureProxyConnection = true;
        [JsonIgnore, XmlIgnore]
        public bool SecureProxyConnection
        {
            get => _secureProxyConnection;
            private set
            {
                if (_secureProxyConnection == value) return;
                _secureProxyConnection = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(ProxyAddress))]
        [XmlAttribute(nameof(ProxyAddress))]
        string _proxyAddress = string.Empty;
        [JsonIgnore, XmlIgnore]
        public string ProxyAddress
        {
            get => _proxyAddress;
            private set
            {
                if (_proxyAddress == value) return;
                _proxyAddress = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(ProxyPort))]
        [XmlAttribute(nameof(ProxyPort))]
        int _proxyPort = 443;
        [JsonIgnore, XmlIgnore]
        public int ProxyPort
        {
            get => _proxyPort;
            private set
            {
                if (_proxyPort == value) return;
                _proxyPort = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(ProxyUser))]
        [XmlAttribute(nameof(ProxyUser))]
        string _proxyUser = string.Empty;
        [JsonIgnore, XmlIgnore]
        public string ProxyUser
        {
            get => _proxyUser;
            private set
            {
                if (_proxyUser == value) return;
                _proxyUser = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty(nameof(ProxyPassword))]
        [XmlAttribute(nameof(ProxyPassword))]
        SecureString _proxyPassword;
        [JsonIgnore, XmlIgnore]
        public SecureString ProxyPassword
        {
            get => _proxyPassword;
            private set
            {
                if (_proxyPassword == value) return;
                _proxyPassword = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #endregion

        #region EventHandlers
        public event EventHandler Error;
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

        async Task<string> BaseApiCallAsync(string command, Method method = Method.Get, CancellationTokenSource cts = default)
        {
            string result = string.Empty;
            if (cts == default)
            {
                cts = new(DefaultTimeout);
            }
            if (restClient == null)
            {
                UpdateRestClientInstance();
            }

            RestRequest request = new(command, method);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", $"Bearer {SecureStringHelper.ConvertToString(AccessToken)}");

            RestResponse respone = await restClient.ExecuteAsync(request, cts.Token).ConfigureAwait(false);
            if (respone.StatusCode == HttpStatusCode.OK && respone.ResponseStatus == ResponseStatus.Completed)
            {
                result = respone.Content;
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
        Uri GetProxyUri()
        {
            return ProxyAddress.StartsWith("http://") || ProxyAddress.StartsWith("https://") ? new Uri($"{ProxyAddress}:{ProxyPort}") : new Uri($"{(SecureProxyConnection ? "https" : "http")}://{ProxyAddress}:{ProxyPort}");
        }

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
        public void SetAccessToken(SecureString Token)
        {
            AccessToken = Token;
            IsInitialized = true;
        }
        #endregion

        #region OnlineCheck
        public async Task CheckOnlineAsync(int Timeout = 10000)
        {
            if (IsConnecting) return; // Avoid multiple calls
            IsConnecting = true;
            bool isReachable = false;
            try
            {
                // Cancel after timeout
                var cts = new CancellationTokenSource(new TimeSpan(0, 0, 0, 0, Timeout));
                string uriString = $"{_appBaseUrl}";

                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(uriString, cts.Token).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();
                    if (response != null)
                    {
                        isReachable = response.IsSuccessStatusCode;
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
        public async Task<List<LexContact>> GetContactsAsync(LexContactType type, int page = 0, int size = 25, int coolDown = 20)
        {
            List<LexContact> result = new();

            string cmd = string.Format("contacts{0}",
                type == LexContactType.Customer ? "?customer=true" : "?vendor=true"              
                );
            cmd += $"&page={page}&size={size}";

            var jsonString = await BaseApiCallAsync(cmd, Method.Get);
            LexContactsList contacts = JsonConvert.DeserializeObject<LexContactsList>(jsonString);
            if (contacts != null)
            {
                result = new List<LexContact>(contacts.Content);
                if (page < contacts.TotalPages)
                {
                    page++;   
                    var append = await GetContactsAsync(type, page, size);
                    result = new List<LexContact>(result.Concat(append));
                    await Task.Delay(coolDown < 20 ? 20 : coolDown);
                    return result;
                }
            }

            return result;
        }

        public async Task<LexContact> GetContactAsync(Guid Id)
        {
            var jsonString = await BaseApiCallAsync(string.Format("contacts/{0}", Id.ToString()), Method.Get);
            LexContact contact = JsonConvert.DeserializeObject<LexContact>(jsonString);

            return contact;
        }
        #endregion

        #region Invoices
        public async Task<List<VoucherListContent>> GetInvoiceListAsync(LexVoucherStatus status, bool archived = false, int page = 0, int size = 25)
        {
            List<VoucherListContent> result = new List<VoucherListContent>();

            string type = LexVoucherType.invoice.ToString();
            string cmd = string.Format("voucherlist?{0}&{1}&{2}",
                $"voucherType={type}",
                $"voucherStatus={status}",
                $"archived={archived}"
                );
            cmd = cmd + $"&page={page}&size={size}";

            var jsonString = await BaseApiCallAsync(cmd, Method.Get);

            var list = JsonConvert.DeserializeObject<LexVoucherList>(jsonString);

            if (list != null)
            {
                if (page < list.TotalPages - 1)
                {
                    page++;
                    result = new List<VoucherListContent>(list.Content);
                    var append = await GetInvoiceListAsync(status, archived, page, size);
                    result = new List<VoucherListContent>(result.Concat(append));
                    return result;
                }
            }
            return result;
        }

        public async Task<List<LexQuotation>> GetInvoicesAsync(List<Guid> Ids)
        {
            List<LexQuotation> result = new List<LexQuotation>();
            foreach (Guid Id in Ids)
            {
                var quote = await GetInVoiceAsync(Id);
                if (quote != null)
                    result.Add(quote);
            }
            return result;
        }
        public async Task<List<LexQuotation>> GetInvoicesAsync(List<VoucherListContent> VoucherList)
        {
            var ids = VoucherList.Select(id => id.Id).ToList();
            return await GetInvoicesAsync(ids);
        }

        public async Task<LexQuotation> GetInVoiceAsync(Guid Id)
        {
            var jsonString = await BaseApiCallAsync(string.Format("invoices/{0}", Id.ToString()), Method.Get);
            LexQuotation contact = JsonConvert.DeserializeObject<LexQuotation>(jsonString);
            return contact;
        }
        #endregion

        #region Quotations
        public async Task<List<VoucherListContent>> GetQuotationListAsync(LexVoucherStatus status, bool archived = false, int page = 0, int size = 25)
        {
            List<VoucherListContent> result = new List<VoucherListContent>();

            string type = LexVoucherType.quotation.ToString();
            string cmd = string.Format("voucherlist?{0}&{1}&{2}",
                $"voucherType={type}",
                $"voucherStatus={status}",
                $"archived={archived}"
                );
            cmd = cmd + $"&page={page}&size={size}";

            var jsonString = await BaseApiCallAsync(cmd, Method.Get);

            var list = JsonConvert.DeserializeObject<LexVoucherList>(jsonString);

            if (list != null)
            {
                if (page < list.TotalPages - 1)
                {
                    page++;
                    result = new List<VoucherListContent>(list.Content);
                    var append = await GetQuotationListAsync(status, archived, page, size);
                    result = new List<VoucherListContent>(result.Concat(append));
                    return result;
                }
            }
            return result;
        }

        public async Task<List<LexQuotation>> GetQuotationsAsync(List<Guid> Ids)
        {
            List<LexQuotation> result = new List<LexQuotation>();
            foreach (Guid Id in Ids)
            {
                var quote = await GetQuotationAsync(Id);
                if (quote != null)
                    result.Add(quote);
            }
            return result;
        }
        public async Task<List<LexQuotation>> GetQuotationsAsync(List<VoucherListContent> VoucherList)
        {
            var ids = VoucherList.Select(id => id.Id).ToList();
            return await GetQuotationsAsync(ids);
        }

        public async Task<LexQuotation> GetQuotationAsync(Guid Id)
        {
            var jsonString = await BaseApiCallAsync(string.Format("quotations/{0}", Id.ToString()), Method.Get);
            LexQuotation contact = JsonConvert.DeserializeObject<LexQuotation>(jsonString);
            return contact;
        }
        #endregion

        #endregion
    }
}
