using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;

namespace TravelogApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("TravelogApi");
            services.AddDbContext<AppDbContext>(options =>
                                options.UseSqlServer(connectionString));

            //use the jwtbearer auth
            services.AddAuthentication("TravelogBearerAuth")
                    .AddJwtBearer("TravelogBearerAuth", config =>
                    {
                        config.Authority = "https://localhost:5001/";

                        //who we are
                        config.Audience = "TravelogApi";
                    });

            services.AddScoped<ITravelPlanRepository, TravelPlanRepository>();

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