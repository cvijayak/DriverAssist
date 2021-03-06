using DriverAssist.Domain.MongoDB.Extensions;
using DriverAssist.Infrastructure;
using DriverAssist.Infrastructure.Common;
using DriverAssist.WebAPI.Common;
using DriverAssist.WebAPI.Common.Results;
using DriverAssist.WebAPI.Configs;
using DriverAssist.WebAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DriverAssist.WebAPI.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var mongoDBSettings = Configuration.GetSection("MongoDBSettings");
            var notificationSettings = Configuration.GetSection("NotificationSettings");

            services.AddMongoDbProvider(options => options.Bind(mongoDBSettings));
            services.AddOptions<NotificationSettings>().Bind(notificationSettings);

            services
                .AddScoped<IServiceResultFactory, ServiceResultFactory>();

            services
                .AddScoped<IDriverSevice, DriverService>()
                .AddScoped<IVehicleService, VehicleService>()
                .AddScoped<IJourneyStatusService, JourneyStatusService>()
                .AddScoped<ISmsClient, SmsClient>()
                .AddScoped<IWhatsappClient, WhatsappClient>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder.WithOrigins("https://localhost:6001")
                                                                  .AllowAnyMethod()
                                                                  .AllowAnyHeader()
                                                                  .AllowCredentials());
            });
            
            services.AddSignalR();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.UseCamelCasing(false);
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.AllowInputFormatterExceptionMessages = true;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DriverAssist APIs", Version = "v1" });
            });
            services.AddSwaggerGenNewtonsoftSupport();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DriverAssist APIs");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<VehicleNotificationHub>("/hubs/vehicle-notification");
            });
        }
    }
}
