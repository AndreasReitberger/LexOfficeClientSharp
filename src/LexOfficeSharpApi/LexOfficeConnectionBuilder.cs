#if !NETFRAMEWORK
using AndreasReitberger.API.REST.Enums;
using AndreasReitberger.API.REST.Interfaces;
using AndreasReitberger.API.REST;
using System.Collections.Generic;
using System.Threading.RateLimiting;
using System;
#endif

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
            /// <summary>
            /// Build the <seealso cref="LexOfficeClient"/> with the given parameters
            /// </summary>
            /// <returns><seealso cref="LexOfficeClient"/></returns>
            public LexOfficeClient Build()
            {
                if (string.IsNullOrEmpty(_client.ApiTargetPath))
                    _client.ApiTargetPath = "https://api.lexoffice.io/";
                return _client;
            }

            /// <summary>
            /// Set the web address for the rest api connection
            /// </summary>
            /// <param name="webAddress">The rest api target uri, mostly <c>https://api.lexoffice.io/</c></param>
            /// <returns><seealso cref="LexOfficeConnectionBuilder"/></returns>
            public LexOfficeConnectionBuilder WithWebAddress(string webAddress = "https://api.lexoffice.io/", string apiVersion = "v1")
            {
                _client.ApiTargetPath = webAddress;
                _client.ApiVersion = apiVersion;
                return this;
            }
#if NETFRAMEWORK
            public LexOfficeConnectionBuilder WithApiKey(string apiKey)
#else
            /// <summary>
            /// Set the api key for the rest api connection
            /// </summary>
            /// <param name="apiKey">The api key</param>
            /// <param name="tokenName">The token name, which is mostly <c>Authorization</c></param>
            /// <returns><seealso cref="LexOfficeConnectionBuilder"/></returns>
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
#if !NETFRAMEWORK

            /// <summary>
            /// Set the rate limiter for the rest api connection
            /// </summary>
            /// <param name="autoReplenishment"></param>
            /// <param name="tokenLimit">Maximum number of tokens that can be in the bucket at any time</param>
            /// <param name="tokensPerPeriod">Maximum number of tokens to be restored in each replenishment</param>
            /// <param name="replenishmentPeriod">Enable auto replenishment</param>
            /// <param name="queueLimit">Size of the queue</param>
            /// <returns><seealso cref="LexOfficeConnectionBuilder"/></returns>
            public LexOfficeConnectionBuilder WithRateLimiter(bool autoReplenishment, int tokenLimit, int tokensPerPeriod, double replenishmentPeriod, int queueLimit = int.MaxValue)
            {
                _client.Limiter = new TokenBucketRateLimiter(new()
                {
                    TokenLimit = tokenLimit,
                    TokensPerPeriod = tokensPerPeriod,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = queueLimit,
                    ReplenishmentPeriod = TimeSpan.FromSeconds(replenishmentPeriod),
                    AutoReplenishment = true,
                });
                return this;
            }
#endif

            /// <summary>
            /// Set the timeout for the connection in ms (default is 10000 ms)
            /// </summary>
            /// <param name="timeout">The timeout in ms</param>
            /// <returns><seealso cref="LexOfficeConnectionBuilder"/></returns>
            public LexOfficeConnectionBuilder WithTimeout(int timeout = 10000)
            {
                _client.DefaultTimeout = timeout;
                return this;
            }
            #endregion

        }
    }
}
