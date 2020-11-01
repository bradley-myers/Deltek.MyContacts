using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deltek.MyContacts.Data;
using Deltek.MyContacts.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Deltek.MyContacts
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            DataHelpers.SeedData();
            
            services.AddControllers();

            services.AddDbContext<MyContactsDbContext>(options =>
                options.UseInMemoryDatabase("MyContacts").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            
            ILogger logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();

            services.AddSingleton(logger);
            services.AddScoped<ContactRepository>();
            services.AddScoped<ContactService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}