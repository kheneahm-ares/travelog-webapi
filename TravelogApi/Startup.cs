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

            services.AddCors(options =>
            {
                //allows any requests from localhost;3000 to get into app
                //without this requests from any origin will not be allowed to hit
                //this api
                options.AddPolicy("TravelogCorsPolicy", policy =>
                {
                    policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins("http://localhost:3000")
                        .WithExposedHeaders("WWW-Authenticate")
                        .AllowCredentials();
                });
            });

            //use the jwtbearer auth
            services.AddAuthentication("TravelogBearerAuth")
                    .AddJwtBearer("TravelogBearerAuth", config =>
                    {
                        config.Authority = "https://localhost:5001/";

                        //who we are, needed by server to check whether token is for this resource
                        config.Audience = "TravelogApi";
                    });

            services.AddScoped<ITravelPlanRepository, TravelPlanRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserTravelPlanRepository, UserTravelPlanRepository>();
            services.AddScoped<ITravelPlanActivityRepository, TravelPlanActivityRepository>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("TravelogCorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}