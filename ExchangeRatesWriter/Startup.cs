using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using ExchangeRatesWriter.Messaging;
using ExchangeRatesWriter.Repository.mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ExchangeRatesWriter
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
            services.AddAutoMapper(typeof(Startup));
            services.Configure<MongoSettings>(Configuration.GetSection(nameof(MongoSettings)));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<MongoSettings>>().Value);
            services.AddScoped<IExchangeRatesRepository, ExchangeRatesRepository>();

            #region RabbitMQ Dependencies

            var rabbitMQConnection = new ConnectionFactory()
            {
                HostName = Configuration["EventBus:HostName"],
                UserName = Configuration["EventBus:UserName"],
                Password = Configuration["EventBus:Password"]
            }.CreateConnection();

            services.AddSingleton(sp =>
            {
                return rabbitMQConnection;
            });

            #endregion

            services.AddHostedService<EventBusRabbitMQConsumer>();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
