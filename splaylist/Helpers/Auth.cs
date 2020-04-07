using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Linq;

namespace splaylist.Helpers
{
    public class Auth
    {

        // TODO - Utilise state parameter of API to prevent replay attacks

        #region Config Variables

        private const string _scopes = "playlist-read-collaborative" +
            " playlist-modify-private" +
            " playlist-modify-public" +
            " playlist-read-private" +
            " user-library-modify" +
            " user-library-read" +
            // "%20user-follow-read" +
            " user-read-private";

        private static string _clientID = "520ed4783a0f410a8a0436513f0bf048";
        private static string _callbackURI = "http://localhost:51660/callback";

        public static string ClientID => _clientID;
        public static string BaseURI => _callbackURI;

        public static bool ShowSpotifyPermissions = false;

        #endregion


        // Possible parameters after requesting a token
        public string AccessToken { get; private set; }
        public string TokenType { get; private set; }
        public string ExpiresIn { get; private set; }
        public string State { get; private set; }
        public string Error { get; private set; }
        public string FullUri { get; private set; }


        public bool SetLinkParams(string uriString)
        {
            FullUri = uriString;

            // if successful callback, the link contains a "hash fragment" rather than a query string
            // meaning '#' needs to be replaced with '&' (kludge as it isn't correct syntax)
            uriString = uriString.Replace('#', '&');
            // and for some reason the same thing happened while testing error, so replace the initial '?'
            uriString = uriString.Replace('?', '&');

            var parsed = QueryHelpers.ParseQuery(uriString);

            AccessToken = parsed.TryGetValue("access_token", out var access_token_sv) ? access_token_sv.First() : "";
            TokenType = parsed.TryGetValue("token_type", out var token_type_sv) ? token_type_sv.First() : "";
            ExpiresIn = parsed.TryGetValue("expires_in", out var expires_in_sv) ? expires_in_sv.First() : "";
            State = parsed.TryGetValue("state", out var state_sv) ? state_sv.First() : "";
            Error = parsed.TryGetValue("error", out var error_sv) ? error_sv.First() : "";

            if (Error != "") return false;
            return true;
        }


        public static Uri GetLoginLink(string _baseURI)
        {
            return new Uri("https://accounts.spotify.com/authorize" +
                "?response_type=token" +
                // next line won't do anything until nonce is set in a cookie
                //"&state=" + nonce +
                "&client_id=" + ClientID +
                "&redirect_uri=" + _baseURI + "callback" +
                "&scope=" + _scopes +
                "&show_dialog=" + ShowSpotifyPermissions.ToString());
        }

        public static Uri GetLoginLink()
        {
            // fallback - the login links need to use NavigationManager.BaseURI to generate the right callback URI
            return GetLoginLink(BaseURI);
        }
            
    }

}
