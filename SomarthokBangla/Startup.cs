using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SomarthokBangla.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SomarthokBangla
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.ConfigureApplicationCookie(options =>
            {


                options.LoginPath = $"/Customer/User/Login";
                options.LogoutPath = $"/account/logout";
                options.AccessDeniedPath = $"/account/accessDenied";
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie();



//            services
//.AddAuthentication(o => { o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme; })
//.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o => { o.LoginPath = new PathString("/account/login/"); })
//.AddCookie("Admin", o => { o.LoginPath = new PathString("/Admin/User/Login/"); })
//.AddCookie("Customer", o => { o.LoginPath = new PathString("/Customer/User/Login/"); })
//.AddCookie("Inventory", o => { o.LoginPath = new PathString("/Inventory/User/Login/"); });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: "areas",
                template: "{area=Customer}/{controller=Home}/{action=Index}/{id?}"
                  );
            });
        }
    }
}
