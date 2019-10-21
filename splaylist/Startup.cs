using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using splaylist.Helpers;
using splaylist.Models;
using SpotifyAPI.Web;

namespace splaylist
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {


            // following shouldn't necessarily be a singleton, but useful for testing / makes it easier in client Blazor
            services.AddSingleton<Auth>();
            services.AddSingleton<API>();
            services.AddSingleton<Cache>();



        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            
            app.AddComponent<App>("app");
        }
    }
}
