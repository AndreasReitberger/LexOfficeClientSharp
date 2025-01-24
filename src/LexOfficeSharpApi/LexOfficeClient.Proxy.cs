
#if NETFRAMEWORK
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Xml.Serialization;

namespace AndreasReitberger.API.LexOffice
{
    // https://developers.lexoffice.io/docs/#lexoffice-api-documentation/?cid=1766
    public partial class LexOfficeClient : ObservableObject
    {

        #region Properties


        [ObservableProperty]
        public partial bool EnableProxy { get; set; } = false;

        partial void OnEnableProxyChanged(bool value) => UpdateRestClientInstance();

        [ObservableProperty]
        public partial bool ProxyUseDefaultCredentials { get; set; } = true;

        partial void OnProxyUseDefaultCredentialsChanged(bool value) => UpdateRestClientInstance();

        [ObservableProperty]
        public partial bool SecureProxyConnection { get; set; } = true;

        partial void OnSecureProxyConnectionChanged(bool value) => UpdateRestClientInstance();

        [ObservableProperty]
        public partial string ProxyAddress { get; set; } = string.Empty;

        partial void OnProxyAddressChanged(string value) => UpdateRestClientInstance();

        [ObservableProperty]
        public partial int ProxyPort { get; set; } = 443;

        partial void OnProxyPortChanged(int value) => UpdateRestClientInstance();

        [ObservableProperty]
        public partial string ProxyUser { get; set; } = string.Empty;

        partial void OnProxyUserChanged(string value) => UpdateRestClientInstance();

        [ObservableProperty]
        [JsonIgnore, XmlIgnore]
        public partial string? ProxyPassword { get; set; }

        partial void OnProxyPasswordChanged(string? value) => UpdateRestClientInstance();

        #endregion

        #region Methods

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

    }
}
#endif