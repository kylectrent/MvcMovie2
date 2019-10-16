using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using MvcMovie2.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvcMovie2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using MvcMovie2.Data;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Http;

namespace MvcMovie2
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<MvcMovie2Context>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MvcMovie2Context")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication()
                .AddGoogle(options => {
                    IConfigurationSection googleAuthNSection =
                    Configuration.GetSection("Authentication:Google");
                    options.ClientId = googleAuthNSection["GoogleClientId"];
                    options.ClientSecret = googleAuthNSection["GoogleClientSecret"];
                })
                .AddMicrosoftAccount(options =>
                {
                    IConfigurationSection microsoftAuthNSection =
                    Configuration.GetSection("Authentication:Microsoft");
                    options.ClientId = microsoftAuthNSection["MicrosoftClientId"];
                    options.ClientSecret = microsoftAuthNSection["MicrosoftClientSecret"];
                })
                .AddFacebook(options =>
                {
                    IConfigurationSection facebookAuthNSection =
                    Configuration.GetSection("Authentication:Facebook");
                    options.AppId = facebookAuthNSection["FacebookClientId"];
                    options.AppSecret = facebookAuthNSection["FacebookClientSecret"];
                })
                .AddTwitter(options =>
                {
                    IConfigurationSection twitterAuthNSection =
                    Configuration.GetSection("Authentication:Twitter");
                    options.ConsumerKey = twitterAuthNSection["TwitterClientId"];
                    options.ConsumerSecret = twitterAuthNSection["TwitterClientSecret"];
                });
            /*services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });*/


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            /*app.UsePathBase("/signin-facebook");
            app.Use((context, next) =>
            {
                if (context.Request.Path.StartsWithSegments("/signin-facebook", out var remainder))
                {
                    context.Request.Path = remainder;
                }

                return next();
            });*/
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
