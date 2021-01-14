using System.Collections.Generic;
using AutoMapper;
using Common;
using ExchangeRatesWriter.Messaging;
using ExchangeRatesWriter.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoRepository;
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

            #region MongoDb Dependencies

            //services.Configure<IMongoSettings>(Configuration.GetSection(nameof(MongoSettings)));
            //services.AddSingleton(sp => sp.GetRequiredService<IOptions<MongoSettings>>().Value);

            services.AddSingleton<IMongoSettings>(sp =>
            {
                return new MongoSettings()
                {
                    ExchangeRatesCollectionName = Configuration["MongoSettings:ExchangeRatesCollectionName"],
                    ConnectionString = Configuration["MongoSettings:ConnectionString"],
                    DatabaseName = Configuration["MongoSettings:DatabaseName"]
                };
            });

            services.AddScoped<IExchangeRatesRepository, ExchangeRatesMongoRepository>();
            
            #endregion

            services.Configure<Dictionary<string, string[]>>(Configuration.GetSection("CurrencyPairs"));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<Dictionary<string, string[]>>>().Value);

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
