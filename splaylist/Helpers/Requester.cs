using splaylist.Models;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace splaylist.Helpers
{
    /// <summary>
    /// Retrieve information from cache
    /// </summary>
    public class Requester
    {

        // Limits
        private const int ALBUM_REQUEST_LIMIT = 20;
        private const int ARTIST_REQUEST_LIMIT = 50;
        private const int PLAYLIST_REQUEST_LIMIT = 50;
        private const int ANALYSIS_REQUEST_LIMIT = 100;



        private static (List<string> TrackIDs, List<string> ArtistIDs, List<string> AlbumIDs) ExtractIDsFromPlaylist(List<ListingTrack> playlist)
        {
            var trackIDs = new List<string>();
            var artistIDs = new List<string>();
            var albumIDs = new List<string>();

            foreach (var track in playlist)
            {
                trackIDs.Add(track.Id);
                albumIDs.Add(track.AlbumId);

                foreach (var artist in track.ArtistObjects)
                {
                    artistIDs.Add(artist.Id);
                }
            }

            return (trackIDs, artistIDs, albumIDs);
        }


        /// <summary>
        /// Splits a list before creating a request, as it will cut off after a certain amount of IDs
        /// </summary>
        /// <param name="cacher">The function to request recursively</param>
        /// <param name="ids">List of IDs in string form</param>
        /// <param name="limit">The limit corresponding to this request</param>
        /// <returns>true on success, false on error</returns>
        private static async Task<bool> SplitRequestIDs(Func<List<string>, LoadingStatus, Task<bool>> cacher, List<string> ids, int limit, LoadingStatus status=null)
        {
            bool success = true;

            for (int i = 0; i <= ids.Count; i += limit)
            {
                // if we're at the end of the list, only request up to count, as otherwise we hit an OutOfBounds exception
                if (i + limit > ids.Count)
                {
                    // change the success variable only if it returned false
                    if (!await cacher(ids.GetRange(i, ids.Count - i), status)) success = false;
                    
                }
                else
                {
                    if (!await cacher(ids.GetRange(i, limit), status)) success = false;
                    
                }
                status?.SetLoaded(i);
            }

            return success;
        }



        /// <summary>
        /// Gets a user's playlists as a list of ListingPlaylist objects
        /// </summary>
        /// <param name="userID">The ID of the user to look up.</param>
        /// <param name="forceRequest">If false, this function will attempt to return the result from the cache first. If true, it will always make a fresh request and update the cache.</param>
        /// <returns></returns>
        public static async Task<List<ListingPlaylist>> GetUserPlaylistListing(string userID, bool forceRequest=false)
        {
            if (!forceRequest && Cache.UsersPlaylists != null) return Cache.UsersPlaylists;
            var firstPage = await API.S.GetUserPlaylistsAsync(userID, PLAYLIST_REQUEST_LIMIT);
            var results = await Depaginator<SimplePlaylist>.Depage(firstPage);

            var lp = new List<ListingPlaylist>();
            foreach (var playlist in results)
            {
                lp.Add(new ListingPlaylist(playlist));
                Cache.Save(playlist);
            }

            Cache.UsersPlaylists = lp;
            return lp;
        }


        public static async Task<List<ListingTrack>> GetPlaylistTracks(FullPlaylist fp, LoadingStatus status = null)
        {
            var depagedPlaylist = await Depaginator<PlaylistTrack>.Depage(fp.Tracks, status);

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

        }



        public static async Task<bool> CacheFullAlbums(List<string> ids, LoadingStatus status = null)
        {
            // Split the list so that it doesn't exceed 20 albums at one time
            if (ids.Count > ALBUM_REQUEST_LIMIT)
                return await SplitRequestIDs(CacheFullAlbums, ids, ALBUM_REQUEST_LIMIT, status);

            var request = await API.S.GetSeveralAlbumsAsync(ids);
            if (request.HasError()) return false;
            foreach (var album in request.Albums) Cache.Save(album);
            return true;
        }


        public static async Task<bool> CacheFullArtists(List<string> ids, LoadingStatus status = null)
        {
            if (ids.Count > ARTIST_REQUEST_LIMIT)
                return await SplitRequestIDs(CacheFullArtists, ids, ARTIST_REQUEST_LIMIT, status);

            var request = await API.S.GetSeveralArtistsAsync(ids);
            if (request.HasError()) return false;
            foreach (var artist in request.Artists) Cache.Save(artist);
            return true;
        }


        public static async Task<bool> CacheAnalysedTracks(List<string> ids, LoadingStatus status = null)
        {
            // Split the list so that it doesn't exceed 20 albums at one time
            if (ids.Count > ANALYSIS_REQUEST_LIMIT)
                return await SplitRequestIDs(CacheAnalysedTracks, ids, ANALYSIS_REQUEST_LIMIT, status);

            var request = await API.S.GetSeveralAudioFeaturesAsync(ids);
            if (request.HasError()) return false;
            foreach (var af in request.AudioFeatures) Cache.Save(af);

            return true;
        }






        public static async Task<List<ListingTrack>> GetAndExtendPlaylist(string id, LoadingStatus status)
        {
            var fullPlaylist = await API.S.GetPlaylistAsync(id);

            status?.ChangeStage(LoadingStatus.Stage.Tracks, 0);
            var playlistContents = await Requester.GetPlaylistTracks(fullPlaylist, status);

            var retrievedIDs = ExtractIDsFromPlaylist(playlistContents);

            status?.ChangeStage(LoadingStatus.Stage.Analysis, retrievedIDs.TrackIDs.Count);
            var CachedAnalysis = await Requester.CacheAnalysedTracks(retrievedIDs.TrackIDs, status);

            status?.ChangeStage(LoadingStatus.Stage.Artists, retrievedIDs.ArtistIDs.Count);
            var CachedArtists = await Requester.CacheFullArtists(retrievedIDs.ArtistIDs, status);

            // Album is no use unless you want album popularity or the label of an album
            // status?.ChangeStage(LoaderInfo.Stage.Albums, retrievedIDs.AlbumIDs.Count);
            // var CachedAlbums = await Requester.CacheFullAlbums(retrievedIDs.AlbumIDs, status);

            return playlistContents;
        }


    }
}
