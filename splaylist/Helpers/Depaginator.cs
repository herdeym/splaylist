using SpotifyAPI.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace splaylist.Helpers
{
    public class Depaginator<T>
    {

        public int LoadedCount { get; protected set; }
        public int AvailableCount { get; protected set; }

        /// <summary>
        /// The items that have been depaged so far.
        /// Not guaranteed to be a complete depage; probably want to use the result of Depage()
        /// </summary>
        public List<T> ProgressItems { get; protected set; }


        /// <summary>
        /// Construct a Depaginator. 
        /// </summary>
        public Depaginator()
        {
            LoadedCount = 0;
            AvailableCount = 0;
        }


        public async Task<List<T>> Depage(Paging<T> page)
        {
            ProgressItems = new List<T>();
            var passedPage = page;
            AvailableCount = page.Total;

            // Retrieve items from page passed in argument
            ProgressItems = page.Items;
            LoadedCount = ProgressItems.Count;

            // then iterate over all the next pages
            while (page.HasNextPage())
            {
                page = await API.S.GetNextPageAsync(page);
                ProgressItems.AddRange(page.Items);
                LoadedCount = ProgressItems.Count;
            }

            // Handle previous pages if supplied page parameter was not the first page
            while (passedPage.HasPreviousPage())
            {
                passedPage = await API.S.GetPreviousPageAsync(passedPage);
                ProgressItems.AddRange(passedPage.Items);
                LoadedCount = ProgressItems.Count;
            }

            return ProgressItems;
        }


    }
}
