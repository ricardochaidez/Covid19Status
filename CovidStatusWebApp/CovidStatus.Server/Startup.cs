using System;
using Blazor.Analytics;
using CovidStatus.API.Repositories;
using CovidStatus.API.Repositories.Interface;
using CovidStatus.Server.ConfigurationSettings;
using CovidStatus.Server.Services;
using CovidStatus.Server.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Syncfusion.Blazor;

namespace CovidStatus.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            //Load configuration settings first
            LoadConfigurationSettings();

            ConfigureDataServices(services);

            ThirdPartySoftwareRegistration(services);

            services.AddServerSideBlazor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }

        private static void LoadConfigurationSettings()
        {
            var configurationHelper = new AppConfigurationHelper();
            configurationHelper.LoadConfigurationSettings();
        }

        private void ConfigureDataServices(IServiceCollection services)
        {
            //REST API
            //services.AddHttpClient<ICovidDataService, CovidDataServiceRestAPI>(client =>
            //{
            //    client.BaseAddress = new Uri(AppConfigurationSettings.CovidDataAPIClientAddress);
            //});

            services.AddScoped<ICovidDataRepository, CovidDataRepository>();
            services.AddScoped<ICovidDataService, CovidDataService>();
        }

        private void ThirdPartySoftwareRegistration(IServiceCollection services)
        {
            //Google Analytics
            services.AddGoogleAnalytics("");

            //Syncfusion
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(AppConfigurationSettings.SyncfusionLicense ?? "MzEwNDAxQDMxMzgyZTMyMmUzMFRnanExTXB4Zm5CVG11bTB6RS9YT1lHeVl3bGVHRGpxeFAxaEcxb09PS2s9");
            services.AddSyncfusionBlazor();
        }
    }
}
