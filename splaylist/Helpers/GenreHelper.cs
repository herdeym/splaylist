using splaylist.Models;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace splaylist.Helpers
{
    public class GenreHelper
    {
        public static ListingPlaylist ProcessGenres(ListingPlaylist lp)
        {
            var genres = new Dictionary<string, Dictionary<string, ListingTrack>>();

            foreach (var track in lp.Tracks)
            {
                var result = "";

                foreach (var artist in track?.ArtistObjects)
                {
                    var fullartist = Cache.GetFullArtist(artist?.Id);
                    foreach (var genre in fullartist?.Genres)
                    {
                        result += genre + "; ";

                        // if there's no dictionary for this genre, create it
                        if (!genres.ContainsKey(genre)) 
                            genres[genre] = new Dictionary<string, ListingTrack>();

                        genres[genre][track.Id] = track;
                    }

                }

                track.GenreString = result;
            }


            lp.SongsByGenre = genres;
            return lp;
        }




        // deprecated as we're using ProcessGenres, still keeping around
        private static string SetGenreString(FullTrack ft)
        {
            var result = "";
            foreach (var artist in ft?.Artists)
            {
                var fullartist = Cache.GetFullArtist(artist.Id);
                foreach (var genre in fullartist?.Genres)
                {
                    result += genre + "; ";
                }

            }
            return result;
        }
    }
}
