using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using SpotifyAPI.Web.Models;

namespace splaylist.Helpers
{

    /// <summary>
    /// Singleton for SpotifyWebAPI class.
    /// 
    /// Can't use SpotifyWebAPI as a singleton directly due to the need to initialise it with the access token.
    /// </summary>
    public class APIHelper
    {

        public static SpotifyWebAPI S { get; private set; }

        public PrivateProfile UserProfile { get; private set; }

        public bool IsAuthenticated()
        {
            if (S == null) return false;
            if (string.IsNullOrEmpty(S.AccessToken)) return false;
            return true;
        }

        public async Task<bool> Authenticate(string accessToken, string tokenType)
        {
            S = new SpotifyWebAPI()
            {
                AccessToken = accessToken,
                TokenType = tokenType
            };

            UserProfile = await S.GetPrivateProfileAsync();

            // TODO - handle faulty logins
            // this is mainly here so /callback waits for SpotifyWebAPI to be initialised before redirecting
            return true;

        }

    }
}
