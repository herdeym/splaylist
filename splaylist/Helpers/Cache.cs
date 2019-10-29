using SpotifyAPI.Web.Models;
using System.Collections.Generic;

namespace splaylist.Helpers
{
    public class Cache
    {

        public static Dictionary<string, FullArtist> FullArtists;
        public static Dictionary<string, FullAlbum> FullAlbums;
        public static Dictionary<string, FullTrack> FullTracks;
        public static Dictionary<string, AudioFeatures> AnalysedTracks;
        public static Dictionary<string, SimplePlaylist> SimplePlaylists;
        public static Dictionary<string, FullPlaylist> FullPlaylists;

        // A user's loaded playlists
        public static List<SimplePlaylist> LoadedPlaylists;

        public Cache()
        {
            SimplePlaylists = new Dictionary<string, SimplePlaylist>();
            FullPlaylists = new Dictionary<string, FullPlaylist>();
            FullArtists = new Dictionary<string, FullArtist>();
            FullAlbums = new Dictionary<string, FullAlbum>();
            FullTracks = new Dictionary<string, FullTrack>();
            AnalysedTracks = new Dictionary<string, AudioFeatures>();
        }

        
        // no point saving simple objects as they'll be contained by FullTrack

        // NOTE - Dictionary syntax is dict[key] = value; not dict.Add(key, value); due to exceptions occuring when a key already exists

        public static void Save(FullArtist fa)
        {
            FullArtists[fa.Id] = fa;
        }


        public static void Save(FullAlbum fa)
        {
            FullAlbums[fa.Id] = fa;
        }

        // Tracks - come with simple information which must be added
        public static void Save(FullTrack fa)
        {
            FullTracks[fa.Id] = fa;
        }

        public static void Save(FullPlaylist fp) 
        {
            // cant cache tracks immediately as we get a paging object
            FullPlaylists[fp.Id] = fp;
        }

        public static void Save(AudioFeatures af)
        {
            AnalysedTracks[af.Id] = af;
        }

        public static void Save(SimplePlaylist sp)
        {
            SimplePlaylists[sp.Id] = sp;
        }


    }
}
