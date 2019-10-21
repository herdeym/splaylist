using SpotifyAPI.Web.Models;
using System.Collections.Generic;

namespace splaylist.Models
{
    public class Cache
    {

        // todo - create temporary dictionaries which store Simple Artists/Albums until the API has returned Full Artists/Albums

        public static Dictionary<string, SimpleArtist> SimpleArtists;
        public static Dictionary<string, FullArtist> FullArtists;
        public static Dictionary<string, SimpleAlbum> SimpleAlbums;
        public static Dictionary<string, FullAlbum> FullAlbums;
        public static Dictionary<string, FullTrack> FullTracks;
        public static Dictionary<string, AudioFeatures> AnalysedTracks;

        // Simple ones which are waiting to be downloaded
        internal static Dictionary<string, SimpleAlbum> PendingAlbums;
        internal static Dictionary<string, SimpleArtist> PendingArtists;
        internal static Dictionary<string, SimpleTrack> PendingTuning;

        // splaylist's custom objects
        // public static Dictionary<string, Track> Tracks;
        public static Dictionary<string, FullPlaylist> FullPlaylists;

        public static Dictionary<string, ListingPlaylist> Playlists;

        // A user's loaded playlists
        public static List<ListingPlaylist> UsersPlaylists;

        public Cache()
        {
            SimpleArtists = new Dictionary<string, SimpleArtist>();
            FullArtists = new Dictionary<string, FullArtist>();
            SimpleAlbums = new Dictionary<string, SimpleAlbum>();
            FullAlbums = new Dictionary<string, FullAlbum>();
            FullTracks = new Dictionary<string, FullTrack>();
            AnalysedTracks = new Dictionary<string, AudioFeatures>();

            PendingArtists = new Dictionary<string, SimpleArtist>();
            PendingAlbums = new Dictionary<string, SimpleAlbum>();
            PendingTuning = new Dictionary<string, SimpleTrack>();

            // Tracks = new Dictionary<string, Track>();
            FullPlaylists = new Dictionary<string, FullPlaylist>();

            Playlists = new Dictionary<string, ListingPlaylist>();
        }

        // Simple / full artists / albums
        // Simple ones can be replaced with full after an API request

        // NOTE - Dictionary syntax is dict[key] = value; not dict.Add(key, value); due to exceptions occuring when a key already exists
        public static void Save(SimpleArtist sa)
        {
            if (sa?.Id == null) return;
            SimpleArtists[sa.Id] = sa;
            PendingArtists[sa.Id] = sa;
        }


        public static void Save(FullArtist fa)
        {
            if (fa?.Id == null) return;
            FullArtists[fa.Id] = fa;
        }


        public static void Save(SimpleAlbum sa)
        {
            if (sa?.Id == null) return;
            SimpleAlbums[sa.Id] = sa;
            PendingAlbums[sa.Id] = sa;
        }

        public static void Save(FullAlbum fa)
        {
            if (fa?.Id == null) return;
            FullAlbums[fa.Id] = fa;
        }

        // Tracks - come with simple information which must be added
        public static void Save(FullTrack fa)
        {
            if (fa?.Id == null) return;
            FullTracks[fa.Id] = fa;
            PendingTuning[fa.Id] = fa;

            Save(fa.Album);

            foreach (var artist in fa.Artists)
            {
                Save(artist);
            }

        }


        public static void Save(FullPlaylist fp)
        {
            FullPlaylists[fp.Id] = fp;
            // cant cache tracks here as we get a paging object
        }

        public static void Save(AudioFeatures af)
        {
            AnalysedTracks[af.Id] = af;
        }


        public static void Save(ListingPlaylist lp)
        {
            Playlists[lp.Id] = lp;
        }

    }
}
