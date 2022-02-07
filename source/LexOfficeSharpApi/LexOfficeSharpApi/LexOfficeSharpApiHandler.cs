using AndreasReitberger.Core.Utilities;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LexOfficeSharpApi
{
    // https://docs.microsoft.com/en-us/dotnet/standard/library-guidance/cross-platform-targeting
    // https://developers.lexoffice.io/docs/#lexoffice-api-documentation/?cid=1766
    public class LexOfficeSharpApiHandler : BaseModel
    {

        #region Instance
        static LexOfficeSharpApiHandler _instance = null;
        static readonly object Lock = new object();
        public static LexOfficeSharpApiHandler Instance
        {
            get
            {
                lock (Lock)
                {
                    if (_instance == null)
                        _instance = new LexOfficeSharpApiHandler();
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
        static readonly HttpClient client = new HttpClient();
        #endregion

        #region Variable
        const string _appBaseUrl = "https://api.lexoffice.io/";
        const string _apiVersion = "v1";
        #endregion

        #region Properties
        //public SecureString AccessToken { get; set; }

        [JsonProperty(nameof(AccessToken))]
        [XmlAttribute(nameof(AccessToken))]
        SecureString _accessToken = null;
        [JsonIgnore]
        [XmlIgnore]
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
        [JsonIgnore]
        [XmlIgnore]
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
        [JsonIgnore]
        [XmlIgnore]
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
        [JsonIgnore]
        [XmlIgnore]
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
        public LexOfficeSharpApiHandler()
        {
            IsInitialized = false;
        }
        public LexOfficeSharpApiHandler(SecureString accessToken)
        {
            AccessToken = accessToken;
            IsInitialized = true;
            Instance = this;
        }
        #endregion

        #region Methods
        async Task<string> BaseApiCallAsync(string command, Method method = Method.Get)
        {
            string result = string.Empty;

            var client = new RestClient($"{_appBaseUrl}{_apiVersion}/");

            var request = new RestRequest(command, method);
            request.AddHeader("Authorization", $"Bearer {SecureStringHelper.ConvertToString(AccessToken)}");
            request.RequestFormat = DataFormat.Json;
            request.Timeout = 2500;

            var respone = await client.ExecuteAsync(request);
            if(respone.StatusCode == System.Net.HttpStatusCode.OK)
                result = respone.Content;

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
                    HttpResponseMessage response = await client.GetAsync(uriString, cts.Token).ConfigureAwait(false);
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
        public async Task<ObservableCollection<LexContact>> GetContactsAsync(LexContactType type, int page = 0, int size = 25, int coolDown = 20)
        {
            ObservableCollection<LexContact> result = new ObservableCollection<LexContact>();

            string cmd = string.Format("contacts{0}",
                type == LexContactType.Customer ? "?customer=true" : "?vendor=true"              
                );
            cmd += $"&page={page}&size={size}";

            var jsonString = await BaseApiCallAsync(cmd, Method.Get);
            LexContactsList contacts = JsonConvert.DeserializeObject<LexContactsList>(jsonString);
            if (contacts != null)
            {
                result = new ObservableCollection<LexContact>(contacts.Content);
                if (page < contacts.TotalPages)
                {
                    page++;   
                    var append = await GetContactsAsync(type, page, size);
                    result = new ObservableCollection<LexContact>(result.Concat(append));
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
        public async Task<ObservableCollection<VoucherListContent>> GetInvoiceListAsync(LexVoucherStatus status, bool archived = false, int page = 0, int size = 25)
        {
            ObservableCollection<VoucherListContent> result = new ObservableCollection<VoucherListContent>();

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
                    result = new ObservableCollection<VoucherListContent>(list.Content);
                    var append = await GetInvoiceListAsync(status, archived, page, size);
                    result = new ObservableCollection<VoucherListContent>(result.Concat(append));
                    return result;
                }
            }
            return result;
        }

        public async Task<ObservableCollection<LexQuotation>> GetInvoicesAsync(List<Guid> Ids)
        {
            ObservableCollection<LexQuotation> result = new ObservableCollection<LexQuotation>();
            foreach (Guid Id in Ids)
            {
                var quote = await GetInVoiceAsync(Id);
                if (quote != null)
                    result.Add(quote);
            }
            return result;
        }
        public async Task<ObservableCollection<LexQuotation>> GetInvoicesAsync(ObservableCollection<VoucherListContent> VoucherList)
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
        public async Task<ObservableCollection<VoucherListContent>> GetQuotationListAsync(LexVoucherStatus status, bool archived = false, int page = 0, int size = 25)
        {
            ObservableCollection<VoucherListContent> result = new ObservableCollection<VoucherListContent>();

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
                    result = new ObservableCollection<VoucherListContent>(list.Content);
                    var append = await GetQuotationListAsync(status, archived, page, size);
                    result = new ObservableCollection<VoucherListContent>(result.Concat(append));
                    return result;
                }
            }
            return result;
        }

        public async Task<ObservableCollection<LexQuotation>> GetQuotationsAsync(List<Guid> Ids)
        {
            ObservableCollection<LexQuotation> result = new ObservableCollection<LexQuotation>();
            foreach (Guid Id in Ids)
            {
                var quote = await GetQuotationAsync(Id);
                if (quote != null)
                    result.Add(quote);
            }
            return result;
        }
        public async Task<ObservableCollection<LexQuotation>> GetQuotationsAsync(ObservableCollection<VoucherListContent> VoucherList)
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
