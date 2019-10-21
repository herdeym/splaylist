using Newtonsoft.Json;
using splaylist.Helpers;
using SpotifyAPI.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace splaylist.Models
{
    public class ListingPlaylist
    {

        public ListingPlaylist(SimplePlaylist sp)
        {
            SimplePlaylist = sp;
            Cache.Save(this);
        }


        public SimplePlaylist SimplePlaylist { get; protected set; }

        public FullPlaylist FullPlaylist { get; protected set; }

        public List<ListingTrack> ListingTracks { get; protected set; }


        public async Task<bool> GetFullPlaylist()
        {
            FullPlaylist = await API.S.GetPlaylistAsync(Id);
            ListingTracks = await Requester.GetPlaylistTracks(FullPlaylist);

            // TODO - raise error if it occurs
            // if (FullPlaylist == null || ListingTracks == null) return false;
            return true;
        }

        public async Task<bool> ExtendTracks()
        {
            // Requester returns bool here, correctly loaded if both true
            return await Requester.CacheAnalysedTracks(ListingTracks) &&
                   await Requester.CacheFullArtists(ListingTracks);
        }


        public string Name => SimplePlaylist?.Name;
        public string Owner => SimplePlaylist?.Owner.DisplayName;
        public string Public => SimplePlaylist?.Public.ToString();
        public int? TrackCount => SimplePlaylist?.Tracks?.Total;

        #region ID
        // JSON required for selecting in DataGrid

        [JsonConstructor]
        public ListingPlaylist(string Id)
        {
            _id = Id;
        }

        private string _id;

        [JsonProperty]
        public string Id
        {
            get
            {
                if (_id != null) return _id;
                _id = SimplePlaylist?.Id;
                return _id;
            }
            protected set
            {
                _id = value;
            }
        }

        #endregion

    }
}
