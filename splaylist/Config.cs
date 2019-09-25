using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace splaylist
{
    public class Config
    {
        private static string _clientID = "8061b5bc221041f39b7ef54c58113f09";
        private static string _callbackURI = "http://localhost:51660/callback";

        //private IConfiguration _config;

        //public Config(IConfiguration config)
        //{
        //    _config = config;

        //    _clientID = _config["Splaylist:ClientID"];
        //    _callbackURI = _config["Splaylist:CallbackURI"];
        //}

        private const string _scopes = "playlist-read-collaborative" +
            " playlist-modify-private" +
            " playlist-modify-public" +
            " playlist-read-private" +
            " user-library-modify" +
            " user-library-read" +
            // "%20user-follow-read" +
            " user-read-private";

        public static string ClientID { get { return _clientID; } }
        public static string CallbackURI {  get { return _callbackURI; } }
        public static string Scopes { get { return _scopes; } }

        public static bool ShowSpotifyPermissions = false;
    }
}
