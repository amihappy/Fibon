using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fibon.api.Framework;
using Fibon.api.Handlers;
using Fibon.api.Repository;
using Fibon.messages.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.vNext;

namespace Fibon.api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //container IoC
            // Add framework services.
            services.AddMvc();
            services.AddSingleton<IRepository>(_ => new MemoryRepo());
            this.ConfigureRabbitMq(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //pipeline
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseMvc();
            this.ConfigureRabbitMqSubscriptions(app); this.ConfigureRabbitMqSubscriptions(app);
        }

        private void ConfigureRabbitMq(IServiceCollection services)
        {
            //services.Configure<RabbitMqOptions>(Configuration.GetSection("rabbitmq"));
            var options = new RabbitMqOptions();
            var section = Configuration.GetSection("rabbitmq");
            section.Bind(options);

            var client = BusClientFactory.CreateDefault(options);

            services.AddSingleton<IBusClient>(p => client);
            services.AddScoped<IEventHandler<ValueCalculatedEvent>, ValueCalculatedEventHandler>();
        }

        private void ConfigureRabbitMqSubscriptions(IApplicationBuilder builder)
        {
            var client = builder.ApplicationServices.GetService<IBusClient>();
            var handler = builder.ApplicationServices.GetService<IEventHandler<ValueCalculatedEvent>>();
            client.SubscribeAsync<ValueCalculatedEvent>(async (e, context) =>
            {
               await handler.HandleAsync(e);
            });
        }
    }
}
