using SpotifyAPI.Web.Models;
using System.Collections.Generic;
using splaylist.Models;

namespace splaylist.Helpers
{
    public class Cache
    {

        private static NullHandlingDictionary<string, FullArtist> FullArtists;
        private static NullHandlingDictionary<string, FullAlbum> FullAlbums;
        private static NullHandlingDictionary<string, FullTrack> FullTracks;
        private static NullHandlingDictionary<string, AudioFeatures> AnalysedTracks;
        private static NullHandlingDictionary<string, SimplePlaylist> SimplePlaylists;
        private static NullHandlingDictionary<string, FullPlaylist> FullPlaylists;

        // A user's loaded playlists
        public static List<ListingPlaylist> UsersPlaylists { get; set; }

        public Cache()
        {
            SimplePlaylists = new NullHandlingDictionary<string, SimplePlaylist>();
            FullPlaylists = new NullHandlingDictionary<string, FullPlaylist>();
            FullArtists = new NullHandlingDictionary<string, FullArtist>();
            FullAlbums = new NullHandlingDictionary<string, FullAlbum>();
            FullTracks = new NullHandlingDictionary<string, FullTrack>();
            AnalysedTracks = new NullHandlingDictionary<string, AudioFeatures>();
        }

        // NOTE - Dictionary syntax is dict[key] = value; not dict.Add(key, value); due to exceptions occuring when a key already exists

        public static void Save(FullArtist fa) => FullArtists.Save(fa?.Id, fa);

        public static void Save(FullAlbum fa) => FullAlbums.Save(fa?.Id, fa);

        // FullTracks contain objects for SimpleAlbum and SimpleArtist, which aren't much use to us
        public static void Save(FullTrack ft) => FullTracks.Save(ft?.Id, ft);

        public static void Save(AudioFeatures af) => AnalysedTracks.Save(af?.Id, af);

        public static void Save(SimplePlaylist sp) => SimplePlaylists.Save(sp?.Id, sp);

        public static void Save(FullPlaylist fp) => FullPlaylists.Save(fp?.Id, fp);


        // Check if ID is in cache
        // Null checks necessary due to local tracks causing issues
        public static bool HasFullArtist(string id) => FullArtists.ContainsKey(id);

        public static bool HasAnalysedTrack(string id) => AnalysedTracks.ContainsKey(id);

        public static bool HasFullAlbum(string id) => FullAlbums.ContainsKey(id);

        public static bool HasFullPlaylist(string id) => FullPlaylists.ContainsKey(id);

        public static bool HasSimplePlaylist(string id) => SimplePlaylists.ContainsKey(id);

        public static bool HasTrack(string id) => FullTracks.ContainsKey(id);



        public static FullArtist GetFullArtist(string id) => FullArtists.Get(id);

        public static FullAlbum GetFullAlbum(string id) => FullAlbums.Get(id);

        public static FullPlaylist GetFullPlaylist(string id) => FullPlaylists.Get(id);

        public static SimplePlaylist GetSimplePlaylist(string id) => SimplePlaylists.Get(id);

        public static FullTrack GetFullTrack(string id) => FullTracks.Get(id);

        public static AudioFeatures GetAnalysedTrack(string id) => AnalysedTracks.Get(id);
    }
}
