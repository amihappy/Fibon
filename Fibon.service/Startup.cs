using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fibon.messages.Commands;
using Fibon.service.Framework;
using Fibon.service.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.vNext;

namespace Fibon.service
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
            // Add framework services.
            services.AddMvc();
            this.ConfigureRabbitMq(services);
        }

        private void ConfigureRabbitMqSubscriptions(IApplicationBuilder builder)
        {
            var client = builder.ApplicationServices.GetService<IBusClient>();
            var handler = builder.ApplicationServices.GetService<ICommandHandler<CalculateValue>>();
            client.SubscribeAsync<CalculateValue>(async (value, context) =>
            {
                await handler.HandleAsync(value);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
            this.ConfigureRabbitMqSubscriptions(app);
            this.ConfigureRabbitMqSubscriptions(app);

        }

        private void ConfigureRabbitMq(IServiceCollection services)
        {
            //services.Configure<RabbitMqOptions>(Configuration.GetSection("rabbitmq"));
            var options = new RabbitMqOptions();
            var section = Configuration.GetSection("rabbitmq");
            section.Bind(options);

            var client = BusClientFactory.CreateDefault(options);

            services.AddSingleton<IBusClient>(p => client);
            services.AddTransient<ICommandHandler<CalculateValue>, CalculatedValueCommandHandler>();
        }
    }
}
