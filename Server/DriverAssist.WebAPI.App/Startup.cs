using DriverAssist.Domain.MongoDB.Extensions;
using DriverAssist.WebAPI.Common;
using DriverAssist.WebAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            services.AddMongoDbProvider(options => options.Bind(mongoDBSettings));

            services
                .AddScoped<IDriverSevice, DriverService>()
                .AddScoped<IVehicleService, VehicleService>()
                .AddScoped<INotificationService, NotificationService>();

            services.AddControllers()
                .AddNewtonsoftJson(options => options.UseMemberCasing());

            services.AddSwaggerGen();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
