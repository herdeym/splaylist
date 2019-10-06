using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using splaylist.Models;

namespace splaylist.Helpers
{
    public class RequestSimpleToFull
    {


        private const int ALBUM_REQUEST_LIMIT = 20;
        private const int ARTIST_REQUEST_LIMIT = 50;

        public async void UpdateFullAlbums()
        {
            List<string> requestIds = new List<string>();

            foreach (var album in Cache.SimpleAlbums)
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


        protected async void SubmitAlbumRequest(List<string> ids)
        {
            if (ids.Count == 0) return;
            var requested = await API.S.GetSeveralAlbumsAsync(ids);
            foreach (var album in requested.Albums)
            {
                Cache.Save(album);
                Cache.PendingAlbums.Remove(album.Id);
            }
        }


        public async void UpdateFullArtists()
        {
                List<string> requestIds = new List<string>();

                foreach (var artist in Cache.PendingArtists)
                {
                    requestIds.Add(artist.Key);
                    if (requestIds.Count == ARTIST_REQUEST_LIMIT)
                    {
                        SubmitArtistRequest(requestIds);
                        requestIds.Clear();
                    }
                }
                // request any that didn't full up a request of 50
                SubmitArtistRequest(requestIds);
        }

        protected async void SubmitArtistRequest(List<string> ids)
        {
            if (ids.Count == 0) return;
            var requested = await API.S.GetSeveralArtistsAsync(ids);
            foreach (var artist in requested.Artists)
            {
                Cache.Save(artist);
                Cache.PendingArtists.Remove(artist.Id);
            }
        }
    }
}
