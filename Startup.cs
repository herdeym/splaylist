using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using splaylist.Helpers;

namespace splaylist
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Config>();

            // following line shouldn't necessarily be a singleton, but useful for testing / irrelevant in client Blazor
            services.AddSingleton<AuthHelper>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            
            app.AddComponent<App>("app");
        }
    }
}
