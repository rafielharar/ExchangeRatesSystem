using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using ExchangeRatesReader.Messaging;
using ExchangeRatesReader.Service;
using Common;

namespace ExchangeRatesReader
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
            //services.AddScoped<IExchangeRatesMapper, ExchangeRatesAPI.ExchangeRatesMapper>();
            services.AddScoped<IExchangeRatesMapper, LiveRatesAPI.ExchangeRatesMapper>();
            services.AddHostedService<ExchangeRatesReaderService>();

            #region RabbitMQ Dependencies

            var rabbitMQConnection = new ConnectionFactory()
            {
                HostName = Configuration["EventBus:HostName"],
                UserName = Configuration["EventBus:UserName"],
                Password = Configuration["EventBus:Password"]
            }.CreateConnection();

            services.AddSingleton<IEventBusProducer>(sp =>
            {
                return new RabbitMQEventBusProducer(rabbitMQConnection);
            });

            services.AddSingleton(sp =>
            {
                return rabbitMQConnection;
            });

            #endregion
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
