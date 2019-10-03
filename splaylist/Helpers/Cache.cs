using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpotifyAPI.Web.Models;

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

        // Simple ones which are waiting to be downloaded
        private static Dictionary<string, SimpleAlbum> _pendingAlbums;
        private static Dictionary<String, SimpleArtist> _pendingArtists;

        // splaylist's custom objects
        public static Dictionary<string, Track> Tracks;
        public static Dictionary<string, FullPlaylist> FullPlaylists;

        public Cache()
        {
            SimpleArtists = new Dictionary<string, SimpleArtist>();
            FullArtists = new Dictionary<string, FullArtist>();
            SimpleAlbums = new Dictionary<string, SimpleAlbum>();
            FullAlbums = new Dictionary<string, FullAlbum>();
            FullTracks = new Dictionary<string, FullTrack>();

            _pendingArtists = new Dictionary<string, SimpleArtist>();
            _pendingAlbums = new Dictionary<string, SimpleAlbum>();

            Tracks = new Dictionary<string, Track>();
            FullPlaylists = new Dictionary<string, FullPlaylist>();
        }

// Simple / full artists / albums
// Simple ones can be replaced with full after an API request
        public void Save(SimpleArtist sa)
        {
            SimpleArtists.Add(sa.Id, sa);
            _pendingArtists.Add(sa.Id, sa);
        }


        public void Save(FullArtist fa)
        {
            FullArtists.Add(fa.Id, fa);
        }


        public void Save(SimpleAlbum sa)
        {
            SimpleAlbums.Add(sa.Id, sa);
            _pendingAlbums.Add(sa.Id, sa);
        }

        public void Save(FullAlbum fa)
        {
            FullAlbums.Add(fa.Id, fa);
        }

        // Tracks - come with simple information which must be added
        public void Save(FullTrack fa)
        {
            FullTracks.Add(fa.Id, fa);
            Save(fa.Album);

            foreach (var artist in fa.Artists)
            {
                Save(artist);
            }
            
        }


        public void Save(FullPlaylist fp)
        {
            FullPlaylists.Add(fp.Id, fp);
        }



    }
}
