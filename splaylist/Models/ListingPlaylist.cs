using Newtonsoft.Json;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace splaylist.Models
{
    public class ListingPlaylist
    {
        public SimplePlaylist SimplePlaylist { get; protected set; }

        public ListingPlaylist(SimplePlaylist sp)
        {
            SimplePlaylist = sp;
        }


        public string Name => SimplePlaylist?.Name;

        public string Owner => SimplePlaylist?.Owner.DisplayName;
        public string Public => SimplePlaylist?.Public.ToString();

        public int? TrackCount => SimplePlaylist?.Tracks?.Total;



        // Workaround for selecting in DataGrid
        [JsonConstructor]
        public ListingPlaylist(string Id)
        {
            _id = Id;
        }

        private string _id;

        [JsonProperty]
        public string Id {
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

    }
}
