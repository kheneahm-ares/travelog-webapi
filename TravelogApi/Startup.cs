using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TravelogApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //use the jwtbearer auth
            services.AddAuthentication("TravelogBearerAuth")
                    .AddJwtBearer("TravelogBearerAuth", config =>
                    {
                        config.Authority = "https://localhost:5001/";

                        //who we are
                        config.Audience = "TravelogApi";
                    });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
