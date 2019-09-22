using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace splaylist.Helpers
{

    /// <summary>
    /// Singleton for SpotifyWebAPI class.
    /// 
    /// Can't use SpotifyWebAPI as a singleton directly due to the need to initialise it with the access token.
    /// </summary>
    public class APIHelper
    {
        bool _authenticated = false;

        public void Authenticate(string accessToken, string tokenType)
        {
            S = new SpotifyWebAPI()
            {
                AccessToken = accessToken,
                TokenType = tokenType
            };

            _authenticated = true;
        }

        public SpotifyWebAPI S { get; private set; }
    }
}
