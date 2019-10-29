using splaylist.Models;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace splaylist.Helpers
{

    /// <summary>
    /// Takes a Paging object and returns a regular list.
    /// Although Depaginator is only used by Requester, it contains a generic type and must be its own class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    static class Depaginator<T>
    {


        internal static async Task<List<T>> Depage(Paging<T> page, LoadingStatus status=null)
        {
            var loadedItems = page.Items;
            var passedPage = page;
            status?.SetAvailable(page.Total);
            status?.SetLoaded(loadedItems.Count);

            // then iterate over all the next pages
            while (page.HasNextPage())
            {
                page = await API.S.GetNextPageAsync(page);
                loadedItems.AddRange(page.Items);
                status?.SetLoaded(loadedItems.Count);
            }

            // Handle previous pages if supplied page parameter was not the first page
            while (passedPage.HasPreviousPage())
            {
                passedPage = await API.S.GetPreviousPageAsync(passedPage);
                loadedItems.AddRange(passedPage.Items);
                status?.SetLoaded(loadedItems.Count);
            }

            return loadedItems;
        }


    }
}
