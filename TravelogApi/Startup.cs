using Business.TravelPlan;
using Business.TravelPlan.Interfaces;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
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
                        config.Authority = Configuration["IdentityServerUrl"];

                        //who we are, needed by server to check whether token is for this resource
                        config.Audience = "TravelogApi";
                    });

            //repos
            services.AddScoped<ITravelPlanRepository, TravelPlanRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserTravelPlanRepository, UserTravelPlanRepository>();
            services.AddScoped<ITravelPlanActivityRepository, TravelPlanActivityRepository>();
            services.AddScoped<IPlanInvitationRepository, PlanInvitationRepository>();
            services.AddScoped<ITravelPlanStatusRepository, TravelPlanStatusRepository>();
            services.AddScoped<ITPAnnouncementRepository, TPAnnouncementRepository>();

            //bus services
            services.AddScoped<ITravelPlanService, TravelPlanService>();
            services.AddScoped<ITravelPlanInvitationService, TravelPlanInvitationService>();
            services.AddScoped<ITravelPlanStatusService, TravelPlanStatusService>();
            services.AddScoped<IUserTravelPlanService, UserTravelPlanService>();
            services.AddScoped<ITPActivityService, TPActivityService>();
            services.AddScoped<ITPAnnouncementService, TPAnnouncementService>();
            services.AddScoped<ITravelPlanInvitationService, TravelPlanInvitationService>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true; 
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