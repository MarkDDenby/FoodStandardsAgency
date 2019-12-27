using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceClient.Contracts;
using ServiceClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AutoMapper;
using FoodStandardsAgency.Rating.Contracts;
using FoodStandardsAgency.Rating;

namespace FoodStandardsAgency
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
            services.AddMvc();
            services.AddAutoMapper();
            services.AddMemoryCache();

            // Create the httpclient factory as singleton so we only use one httpclient for this session.
            services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
            services.AddTransient<IFoodStandardAgencyServiceClient, ServiceClient.FoodStandardAgencyServiceClient>();
            services.AddTransient<IRatingFactory, RatingFactory>();
            services.AddTransient<IRatingCalculator, RatingCalculator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddConsole();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                loggerFactory.AddAzureWebAppDiagnostics();
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
