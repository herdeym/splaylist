using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace splaylist
{
    public class Config
    {
        private string _clientID = "I've honest to god spent hours trying to get user secrets or environmental variables working for this ID, hence the block being commented out below. If you're unfortunate enough to rip this project off github, replace this variable with a key from https://developer.spotify.com/dashboard/applications/";
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
