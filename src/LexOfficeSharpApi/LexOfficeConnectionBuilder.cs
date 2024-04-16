using AndreasReitberger.Core.Utilities;
using System.Security;

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

            public LexOfficeConnectionBuilder WithApiKey(SecureString apiKey)
            {
                _client.AccessToken = apiKey;
                return this;
            }

            public LexOfficeConnectionBuilder WithApiKey(string apiKey)
            {
                _client.AccessToken = SecureStringHelper.ConvertToSecureString(apiKey);
                return this;
            }

            #endregion

        }
    }
}
