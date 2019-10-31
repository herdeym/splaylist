using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System.Threading.Tasks;

namespace splaylist.Helpers
{

    /// <summary>
    /// Singleton for SpotifyWebAPI class.
    /// 
    /// Can't use SpotifyWebAPI as a singleton directly due to the need to initialise it with the access token.
    /// </summary>
    public class API
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

            // handle faulty logins by checking if UserProfile gets set or not
            // useful for making /callback wait for SpotifyWebAPI to be initialised before redirecting
            return (UserProfile != null);

        }

    }
}
