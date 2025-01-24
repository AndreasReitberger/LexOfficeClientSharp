#if !NETFRAMEWORK
using AndreasReitberger.API.REST.Enums;
using AndreasReitberger.API.REST.Interfaces;
using AndreasReitberger.API.REST;
#endif
using System.Collections.Generic;

namespace AndreasReitberger.API.LexOffice
{
    public partial class LexOfficeClient
    {
        public class LexOfficeConnectionBuilder
        {
            #region Instance
            readonly LexOfficeClient _client = new();
            #endregion

            #region Methods

            public LexOfficeClient Build()
            {
                return _client;
            }
            public LexOfficeConnectionBuilder WithWebAddress(string webAddress)
            {
                _client.ApiTargetPath = webAddress;
                return this;
            }
#if NETFRAMEWORK
            public LexOfficeConnectionBuilder WithApiKey(string apiKey)
#else
            public LexOfficeConnectionBuilder WithApiKey(string apiKey, string tokenName = "Authorization")
#endif
            {
#if NETFRAMEWORK
                _client.AccessToken = apiKey;
#else
                _client.AuthHeaders = new Dictionary<string, IAuthenticationHeader>() { { tokenName, new AuthenticationHeader()
                    {
                        Target = AuthenticationHeaderTarget.Header,
                        Token = $"Bearer {apiKey}",
                    }
                } };
#endif
                return this;
            }

        #endregion

        }
    }
}
