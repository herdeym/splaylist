using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
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


            // Blazorise - used for DataGrid
            services.AddBlazorise(options =>
                {
                    options.ChangeTextOnKeyPress = true; // optional
                })
                .AddBootstrapProviders()
                .AddFontAwesomeIcons();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            
            app.AddComponent<App>("app");
        }
    }
}
