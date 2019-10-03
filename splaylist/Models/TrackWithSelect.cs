using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpotifyAPI.Web.Models;

namespace splaylist.Models
{
    public class TrackWithSelect : Track
    {
        public TrackWithSelect(FullTrack fullTrack) : base(fullTrack) { }


        public bool Selected;
    }
}
