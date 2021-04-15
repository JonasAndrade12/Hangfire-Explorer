namespace Hangfire_Explorer
{
    using System;
    using Hangfire;
    using Hangfire_Explorer.Configuration;
    using Hangfire_Explorer.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        private readonly IApplicationSettings applicationSettings;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            this.applicationSettings = this.Configuration
                .Get<ApplicationSettings>();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddLogging();
            services.Configuration(this.applicationSettings);
            services.ConfigureHangfire();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Exemple of Fire-and-forget job
            backgroundJobs.Enqueue(() => Console.WriteLine("Hangfire started!"));

            // Enable Hangfire dashboard in URL/hangfire
            app.UseHangfireDashboard();
        }
    }
}
