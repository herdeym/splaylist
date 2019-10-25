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
    public static class Depaginator<T>
    {


        public static async Task<List<T>> Depage(Paging<T> page, LoaderInfo loader=null)
        {
            // if no loader is passed, create one and pretend it doesn't exist
            if (loader == null) loader = new LoaderInfo();

            var ProgressItems = new List<T>();
            var passedPage = page;
            loader.Available = page.Total;

            // Retrieve items from page passed in argument
            ProgressItems = page.Items;
            loader.Loaded = ProgressItems.Count;

            // then iterate over all the next pages
            while (page.HasNextPage())
            {
                page = await API.S.GetNextPageAsync(page);
                ProgressItems.AddRange(page.Items);
                loader.Loaded = ProgressItems.Count;
            }

            // Handle previous pages if supplied page parameter was not the first page
            while (passedPage.HasPreviousPage())
            {
                passedPage = await API.S.GetPreviousPageAsync(passedPage);
                ProgressItems.AddRange(passedPage.Items);
                loader.Loaded = ProgressItems.Count;
            }

            return ProgressItems;
        }


        public static Tuple<Task<List<T>>, LoaderInfo> DepageWithStatus(Paging<T> page)
        {
            LoaderInfo loader = new LoaderInfo();
            return new Tuple<Task<List<T>>, LoaderInfo>(Depage(page, loader), loader);
        }


    }
}
