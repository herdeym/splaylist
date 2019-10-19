using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using SpotifyAPI.Web.Models;
using splaylist.Helpers;

namespace splaylist.Models
{
 /// <summary>
 ///  Track used in listings of tracks.
 ///
 /// While ideally this would inherit from FullTrack and just add extra methods, downcasting proved too cumbersome.
 /// </summary>
    public class ListingTrack
 {

        // SimpleTrack encountered when getting tracks off a FullAlbum (simple album doesn't have track info), or recommendations.
        // Commented out as it is not relevant at this point in time (using playlists only)
        // Relevant fields - SimpleArtist, Id, Name, DurationMs
        // Since FullTrack has been modified to extend SimpleTrack, we can actually cast FullTrack for it
        // private SimpleTrack _simpleTrack;
       

            /// <summary>
            /// Index so that the DataGrid has a primary key when rearranging the display of the playlist
            /// </summary>
        public int Index { get; protected set; }

        public ListingTrack(FullTrack ft, int index=-1)
        {
            FullTrack = ft;
            Index = index;
        }

        public ListingTrack(PlaylistTrack pt, int index=-1)
        {
            PlaylistTrack = pt;
            FullTrack = pt.Track;
            Index = index;
        }

        public ListingTrack(SavedTrack st, int index = -1)
        {
            SavedTrack = st;
            FullTrack = st.Track;
            Index = index;
        }


        // SavedTrack mutually exclusive from PlaylistTrack
        public SavedTrack SavedTrack { get; protected set; }

        public PlaylistTrack PlaylistTrack { get; protected set; }

        public FullTrack FullTrack { get; protected set; }

        /// <summary>
        /// TunableTrack, with properties for tempo, valence, etc
        /// Does not contain ID so can't be used as constructor
        /// </summary>
        private TuneableTrack _tuneableTrack;
        public TuneableTrack TuneableTrack { get
            {
                if (_tuneableTrack != null) return _tuneableTrack;

                if (Cache.TuneableTracks.TryGetValue(FullTrack.Id, out var result))
                {
                    _tuneableTrack = result;
                    return result;
                }

                // If we're waiting on the tunable track, add it to the cache's pending list
                Cache.PendingTuning.Add(FullTrack.Id, FullTrack);
                return null;
            }
        }


        // Not pretty, but the datagrid wasn't handling nested objects 
        public string TrackTitle => FullTrack?.Name;
        public string Album => FullTrack?.Album?.Name;
        public string Id => FullTrack?.Id;

        // below contains genre (but not as useful as artist), tracks, and popularity
        // public FullAlbum FullAlbum;

        // Below contains genre info, retrieve from cache
        // public List<FullArtist> FullArtists;

        public List<string> Genres
        {
            get
            {
                // hack because I haven't removed local tracks from the playlist yet
                if (FullTrack?.Artists[0]?.Id == null) return null;

                // TODO - correct for multiple artists
                if (Cache.FullArtists.TryGetValue(FullTrack?.Artists[0]?.Id, out var artist))
                {
                    return artist.Genres;
                }

                return null;
            }
        }




        // todo - fix for precision
        public string AlbumDate => FullTrack?.Album?.ReleaseDate;



        public string ArtistString
        {
            get
            {
                {
                    if (FullTrack?.Artists?.Count == 0) return "";

                    string result = FullTrack?.Artists[0]?.Name;
                    for (int i = 1; i < FullTrack.Artists.Count; i++)
                    {
                        result += "; " + FullTrack.Artists[i]?.Name;
                    }

                    return result;
                }
            }
        }



    }
}
