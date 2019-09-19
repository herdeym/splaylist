using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace splaylist
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Config>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            
            app.AddComponent<App>("app");
        }
    }
}
