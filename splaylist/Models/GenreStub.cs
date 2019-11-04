using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace splaylist.Models
{
    public class GenreStub
    {
        public GenreStub (KeyValuePair<string, Dictionary<string, ListingTrack>> orig)
        {
            Genre = orig.Key;
            Tracks = orig.Value;
        }  

        public string Genre { get; private set; }

        public Dictionary<string, ListingTrack> Tracks { get; private set; }

        public int Count => Tracks.Count;
    }
}
