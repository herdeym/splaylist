using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace splaylist.Helpers
{
    public class AuthHelper
    {

        // Possible parameters after requesting a token
        string _access_token = "";
        string _token_type = "";
        string _expires_in = "";
        string _state = "";
        string _error = "";
        string _full_uri = "";

        public string AccessToken { get { return _access_token; } }
        public string TokenType { get { return _token_type; } }
        public string ExpiresIn { get { return _expires_in; } }
        public string State { get { return _state; } }
        public string Error { get { return _error; } }
        public string FullUri { get { return _full_uri; } }


        string GenerateState()
        {
            Random r = new Random();
            return r.Next().ToString();
        }


        public bool SetLinkParams(Uri uri)
        {
            string uriString = uri.ToString();
            return SetLinkParams(uriString);
        }

        public bool SetLinkParams(string uriString) { 
            _full_uri = uriString;

            // if successful callback, the link contains a "hash fragment" rather than a query string
            // meaning '#' needs to be replaced with '&' (kludge as it isn't correct syntax)
            uriString = uriString.Replace('#', '&');
            // and for some reason the same thing happened while testing error, so replace the initial '?'
            uriString = uriString.Replace('?', '&');

            var parsed = QueryHelpers.ParseQuery(uriString);

            _access_token = parsed.TryGetValue("access_token", out var access_token_sv) ? access_token_sv.First() : "";
            _token_type = parsed.TryGetValue("token_type", out var token_type_sv) ? token_type_sv.First() : "";
            _expires_in = parsed.TryGetValue("expires_in", out var expires_in_sv) ? expires_in_sv.First() : "";
            _state = parsed.TryGetValue("state", out var state_sv) ? state_sv.First() : "";
            _error = parsed.TryGetValue("error", out var error_sv) ? error_sv.First() : "";

            if (_error != "") return false;
            return true;
        }


        public static Uri GetLoginLink()
        {
            return new Uri("https://accounts.spotify.com/authorize" +
                "?response_type=token" +
                // next line won't do anything until nonce is set in a cookie
                //"&state=" + nonce +
                "&client_id=" + Config.ClientID +
                "&redirect_uri=" + Config.CallbackURI +
                "&scope=" + Config.Scopes +
                "&show_dialog=true");
        }
    }

}
