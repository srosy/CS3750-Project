using Blazored.LocalStorage;
using LMS.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MatBlazor;
using System;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.Net.Http.Headers;
using Syncfusion.Blazor;

namespace LMS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor().AddCircuitOptions(options => { options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(20); }); //options.DetailedErrors = true; }); // remove circuitoptions after testing test-site only bugs
            services.AddDbContext<AzureDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AZURE_DB_CONN_STRING")), ServiceLifetime.Transient);
            services.AddSingleton<IDbService, DbService>();
            services.AddMatBlazor();
            services.AddSyncfusionBlazor();

            services.AddScoped(s =>
            {
                var navigationManager = s.GetRequiredService<NavigationManager>();
                return new HttpClient
                {
                    BaseAddress = new Uri(navigationManager.BaseUri)
                };
            });

            // add localstorage for cookies/session data
            services.AddBlazoredLocalStorage();
            services.AddBlazoredLocalStorage(config =>
                config.JsonSerializerOptions.WriteIndented = true);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
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

            // Use static files which caches importal src and link html to speed up pages
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });
        }
    }
}
