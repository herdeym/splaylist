using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using splaylist.Models;
using SpotifyAPI.Web.Models;

namespace splaylist.Helpers
{
    public class Requester
    {


        private const int ALBUM_REQUEST_LIMIT = 20;
        private const int ARTIST_REQUEST_LIMIT = 50;
        private const int PLAYLIST_REQUEST_LIMIT = 50;
        private const int FEATURE_REQUEST_LIMIT = 100;


        public static async Task<List<SimplePlaylist>> GetUserPlaylistsAsync(string UserID)
        {
            var firstPage = await API.S.GetUserPlaylistsAsync(UserID, PLAYLIST_REQUEST_LIMIT);
            var depaginate = new Depaginator<SimplePlaylist>();
            var results = await depaginate.Depage(firstPage);
            Cache.LoadedPlaylists = results; 
            return results;
        }


        public static async Task<List<ListingTrack>> GetPlaylistTracks(FullPlaylist fp)
        {
            var depager = new Depaginator<PlaylistTrack>();
            var depagedPlaylist = await depager.Depage(fp.Tracks);

            var results = new List<ListingTrack>();

            // convert PlaylistTrack to ListingTrack with associated index
            for (int i = 0; i < depagedPlaylist.Count; i++)
            {
                PlaylistTrack playlistTrack = depagedPlaylist[i];
                results.Add(new ListingTrack(playlistTrack, i));

                // Cache just the FullTrack
                Cache.Save(playlistTrack.Track);
            }

            return results;

            // TODO - Extract loading status
        }


        /// <summary>
        /// Downloads full albums for a playlist
        /// </summary>
        /// <param name="playlist"></param>
        /// <returns></returns>
        public static async Task<bool> CacheFullAlbums(List<ListingTrack> playlist)
        {
            var ids = new List<string>();
            foreach (var track in playlist)
            {
                ids.Add(track.AlbumId);
            }
            return await CacheFullAlbums(ids);
        }


        public static async Task<bool> CacheFullAlbums(List<string> ids)
        {
            // Split the list so that it doesn't exceed 20 albums at one time
            if (ids.Count > ALBUM_REQUEST_LIMIT)
            {
                SplitList(CacheFullAlbums, ids, ALBUM_REQUEST_LIMIT);
            }

            var request = await API.S.GetSeveralAlbumsAsync(ids);
            if (request.HasError()) return false;
            foreach (var album in request.Albums)
            {
                Cache.Save(album);
            }
            return true;
        }



        public static async Task<bool> CacheFullArtists(List<ListingTrack> playlist)
        {
            var ids = new List<string>();
            foreach (var track in playlist)
            {
                foreach (var artist in track.ArtistObjects)
                {
                    ids.Add(artist.Id);
                }
            }
            return await CacheFullArtists(ids);
        }


        public static async Task<bool> CacheFullArtists(List<string> ids)
        {
            if (ids.Count > ARTIST_REQUEST_LIMIT)
            {
                SplitList(CacheFullArtists, ids, ARTIST_REQUEST_LIMIT);
            }


            var request = await API.S.GetSeveralArtistsAsync(ids);
            if (request.HasError()) return false;
            foreach (var artist in request.Artists)
            {
                Cache.Save(artist);
            }
            return true;
        }

        public static async Task<bool> CacheAnalysedTracks(List<ListingTrack> playlist)
        {
            var ids = new List<string>();
            foreach (var track in playlist)
            {
                    ids.Add(track.Id);
            }
            return await CacheAnalysedTracks(ids);
        }


        public static async Task<bool> CacheAnalysedTracks(List<string> ids)
        {
            // Split the list so that it doesn't exceed 20 albums at one time
            if (ids.Count > FEATURE_REQUEST_LIMIT)
            {
                SplitList(CacheAnalysedTracks, ids, FEATURE_REQUEST_LIMIT);
            }


            var request = await API.S.GetSeveralAudioFeaturesAsync(ids);
            if (request.HasError()) return false;
            foreach (var af in request.AudioFeatures)
            {
                Cache.Save(af);
            }
            return true;
        }


        // Was repeating this a bit
        // todo - propogate errors if they come up
        private static async void SplitList(Func<List<string>, Task> cacher, List<string> ids, int limit)
        {

            for (int i = 0; i <= ids.Count; i += limit)
            {
                // if we're at the end of the list, only request up to count, as otherwise we hit an OutOfBounds exception
                if (i + limit > ids.Count)
                {
                    await cacher(ids.GetRange(i, ids.Count - i));
                }
                else
                {
                    await cacher(ids.GetRange(i, limit));
                }
            }

        }
    }
}
