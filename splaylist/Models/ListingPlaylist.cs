using Newtonsoft.Json;
using SpotifyAPI.Web.Models;

namespace splaylist.Models
{
    public class ListingPlaylist
    {

        // Workaround for selecting in DataGrid
        [JsonConstructor]
        public ListingPlaylist(string id)
        {
            Id = id;
        }


        public ListingPlaylist(SimplePlaylist sp)
        {
            SimplePlaylist = sp;
            Id = sp.Id;
        }


        [JsonProperty]
        public string Id { get; protected set; }

        public SimplePlaylist SimplePlaylist { get; protected set; }

        public string Name => SimplePlaylist?.Name;

        public string Owner => SimplePlaylist?.Owner.DisplayName;

        public int? TrackCount => SimplePlaylist?.Tracks?.Total;

        public string Collaborative => SimplePlaylist?.Collaborative.ToString();

        // TODO - other people's public playlists show the following as false
        public string Public => SimplePlaylist?.Public.ToString();


    }
}
