using Newtonsoft.Json;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
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

        // Following is required when selecting the track in the datagrid
        [JsonConstructor]
        public ListingTrack(string id)
        {
            Id = id;
        }

        [JsonProperty]
        public string Id { get; protected set; }


        // SimpleTrack encountered when getting tracks off a FullAlbum (simple album doesn't have track info), or recommendations.
        // Commented out as it is not relevant at this point in time (using playlists only)
        // Relevant fields - SimpleArtist, Id, Name, DurationMs
        // Since FullTrack has been modified to extend SimpleTrack, we can actually cast FullTrack for it
        // private SimpleTrack _simpleTrack;


        /// <summary>
        /// Index so that the DataGrid has a primary key when rearranging the display of the playlist
        /// </summary>
        public int Index { get; protected set; }

        public ListingTrack(FullTrack ft, int index = -1)
        {
            FullTrack = ft;
            Index = index;
            Id = ft.Id;
        }

        public ListingTrack(PlaylistTrack pt, int index = -1)
        {
            PlaylistTrack = pt;
            FullTrack = pt.Track;
            Index = index;
            Id = pt.Track.Id;
        }

        public ListingTrack(SavedTrack st, int index = -1)
        {
            SavedTrack = st;
            FullTrack = st.Track;
            Index = index;
            Id = st.Track.Id;
        }


        // SavedTrack mutually exclusive from PlaylistTrack
        public SavedTrack SavedTrack { get; protected set; }

        public PlaylistTrack PlaylistTrack { get; protected set; }


        // TODO - get cached FullTrack by id in case this is something constructed just from the ID
        public FullTrack FullTrack { get; protected set; }

        /// <summary>
        /// TunableTrack, with properties for tempo, valence, etc
        /// Does not contain ID so can't be used as constructor
        /// </summary>
        private AudioFeatures _analysed;
        public AudioFeatures Analysed
        {
            get
            {
                if (_analysed != null) return _analysed;
                _analysed = Cache.GetAnalysedTrack(Id);
                return _analysed;
            }
        }

    
        // Not pretty, but the datagrid wasn't handling nested objects 
        public string TrackTitle => FullTrack?.Name;
        public string AlbumName => FullTrack?.Album?.Name;

        public int? DurationMs => FullTrack?.DurationMs;


        public string AlbumId => FullTrack?.Album?.Id;

        public List<SimpleArtist> ArtistObjects => FullTrack?.Artists;

        // below contains genre (but not as useful as artist), tracks, and popularity
        // public FullAlbum FullAlbum;

        // Below contains genre info, retrieve from cache
        // private List<FullArtist> _fullArtists;
        // public List<FullArtist> FullArtists;

        public List<string> Genres
        {
            get
            {
                // TODO - correct for multiple artists
                return Cache.GetFullArtist(FullTrack?.Artists[0]?.Id)?.Genres;
            }
        }




        // todo - fix for precision
        public string AlbumDate => FullTrack?.Album?.ReleaseDate;

        public DateTime? PlaylistAddedDate => PlaylistTrack?.AddedAt;



        public string ArtistString
        {
            get
            {
                {
                    if (FullTrack?.Artists?.Count == 0) return "";

                    string result = FullTrack?.Artists[0]?.Name;
                    for (int i = 1; i < FullTrack?.Artists?.Count; i++)
                    {
                        result += "; " + FullTrack?.Artists[i]?.Name;
                    }

                    return result;
                }
            }
        }

        #region AudioFeatures
        // mode - major / minor
        // key - pitch class notation

        private enum ModeType
        {
            Minor = 0,
            Major = 1
        }

        public string Mode
        {
            get
            {
                if (Analysed == null) return "";
                return ((ModeType)Analysed.Mode).ToString();
            }
        }


        private string GetKey(int? k)
        {
            // can't do enum kludge because of # symbol
            switch (k)
            {
                case 0: return "C";
                case 1: return "C#";
                case 2: return "D";
                case 3: return "D#";
                case 4: return "E";
                case 5: return "F";
                case 6: return "F#";
                case 7: return "G";
                case 8: return "G#";
                case 9: return "A";
                case 10: return "A#";
                case 11: return "B";
                default: return "";
            }
        }

        public string Key => GetKey(Analysed?.Key);
        public float? Acousticness => Analysed?.Acousticness;
        public float? Danceability => Analysed?.Danceability;
        public float? Instrumentalness => Analysed?.Instrumentalness;
        public float? Energy => Analysed?.Energy;
        public float? Liveness => Analysed?.Liveness;
        public float? Loudness => Analysed?.Loudness;
        public float? Speechiness => Analysed?.Speechiness;
        public float? Tempo => Analysed?.Tempo;
        public int? TimeSignature => Analysed?.TimeSignature;

        public string TimeSignatureString
        {
            get
            {
                // Assumes all measures are over 4
                if (TimeSignature == null) return "";
                return $"{TimeSignature}/4";
            }
        }

        public float? Valence => Analysed?.Valence;



        #endregion
    }
}
