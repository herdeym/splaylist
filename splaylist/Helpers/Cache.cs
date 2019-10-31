using SpotifyAPI.Web.Models;
using System.Collections.Generic;
using splaylist.Models;

namespace splaylist.Helpers
{
    public class Cache
    {

        private static NullHandlingDictionary<string, FullArtist> FullArtists;
        private static NullHandlingDictionary<string, FullAlbum> FullAlbums;
        private static NullHandlingDictionary<string, AudioFeatures> AnalysedTracks;
        private static NullHandlingDictionary<string, ListingPlaylist> Playlists;

        // A user's loaded playlists
        public static List<ListingPlaylist> UsersPlaylists { get; set; }

        public Cache()
        {
            Playlists = new NullHandlingDictionary<string, ListingPlaylist>();
            FullArtists = new NullHandlingDictionary<string, FullArtist>();
            FullAlbums = new NullHandlingDictionary<string, FullAlbum>();
            AnalysedTracks = new NullHandlingDictionary<string, AudioFeatures>();
        }

        // NOTE - Dictionary syntax is dict[key] = value; not dict.Add(key, value); due to exceptions occuring when a key already exists

        public static void Save(FullArtist fa) => FullArtists.Save(fa?.Id, fa);

        public static void Save(FullAlbum fa) => FullAlbums.Save(fa?.Id, fa);

        public static void Save(AudioFeatures af) => AnalysedTracks.Save(af?.Id, af);

        public static void Save(ListingPlaylist p) => Playlists.Save(p?.Id, p);


        // Check if ID is in cache
        // Null checks necessary due to local tracks causing issues
        public static bool HasFullArtist(string id) => FullArtists.ContainsKey(id);

        public static bool HasAnalysedTrack(string id) => AnalysedTracks.ContainsKey(id);

        public static bool HasFullAlbum(string id) => FullAlbums.ContainsKey(id);

        public static bool HasPlaylist(string id) => Playlists.ContainsKey(id);


        public static FullArtist GetFullArtist(string id) => FullArtists.Get(id);

        public static FullAlbum GetFullAlbum(string id) => FullAlbums.Get(id);

        public static ListingPlaylist GetPlaylist(string id) => Playlists.Get(id);

        public static AudioFeatures GetAnalysedTrack(string id) => AnalysedTracks.Get(id);
    }
}
