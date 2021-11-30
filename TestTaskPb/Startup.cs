using EventBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTaskPb.Services;

namespace TestTaskPb
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
            services.AddControllers();

            var serviceClientSettingsConfig = Configuration.GetSection("RabbitMq");
            services.Configure<RabbitMqConfiguration>(serviceClientSettingsConfig);

            var connectionString = Configuration.GetSection("DBConfig");
            services.Configure<MSSQLDBConfig>(connectionString);

            /////
            //TODO: Need to chandge lifecycle  for those singleton services
            services.AddSingleton<IDBConfig, MSSQLDBConfig>();
            services.AddSingleton<IRepository, Repos>();
            services.AddSingleton<IMessageHandler, MessageHandler>();
            /////

            services.AddHostedService<EventBusRabbitMQ>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
