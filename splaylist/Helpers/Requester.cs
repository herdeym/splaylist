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
        private const int TUNABLE_REQUEST_LIMIT = 100;


        #region Deprecated Helpers
        public static async void UpdateFullAlbums()
        {
            List<string> requestIds = new List<string>();

            foreach (var album in Cache.PendingAlbums)
            {
                requestIds.Add(album.Key);

                // An API request for albums can only request 20 albums at a time
                if (requestIds.Count == ALBUM_REQUEST_LIMIT)
                {
                    SubmitAlbumRequest(requestIds);
                    requestIds.Clear();
                }
            }
            SubmitAlbumRequest(requestIds);
        }


        protected static async void SubmitAlbumRequest(List<string> ids)
        {
            if (ids.Count == 0) return;
            var requested = await API.S.GetSeveralAlbumsAsync(ids);
            foreach (var album in requested.Albums)
            {
                Cache.Save(album);
                Cache.PendingAlbums.Remove(album.Id);
            }

        }


        public static async void UpdateFullArtists()
        {
            Console.WriteLine("Loading artists");
                List<string> requestIds = new List<string>();

                foreach (var artist in Cache.PendingArtists)
                {
                    Console.WriteLine(artist.Value.Name);
                    requestIds.Add(artist.Key);
                    if (requestIds.Count == ARTIST_REQUEST_LIMIT)
                    {
                        SubmitArtistRequest(requestIds);
                        Console.WriteLine(requestIds);
                        requestIds.Clear();
                    }
                }
                // request any that didn't full up a request of 50
                Console.WriteLine(requestIds);
                SubmitArtistRequest(requestIds); 
        }

        protected static async void SubmitArtistRequest(List<string> ids)
        {
            if (ids.Count == 0) return;
            var requested = await API.S.GetSeveralArtistsAsync(ids);
            foreach (var artist in requested.Artists)
            {
                Cache.Save(artist);
                Cache.PendingArtists.Remove(artist.Id);
            }
        }

        private static string ConcatenateList(List<string> list)
        {
            if (list == null) return "";
            if (list.Count == 0) return "";

            string result = list[0];
            for (int i = 1; i < list.Count; i++)
            {
                result += "; " + list[i];
            }

            return result;
        }

        #endregion


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
            }

            return results;

            // TODO - Extract loading status
        }


    }
}
