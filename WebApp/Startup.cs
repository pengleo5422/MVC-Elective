using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using Microsoft.AspNetCore.Http;

namespace WebApp
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
            services.AddControllersWithViews();

            services.AddDbContext<WebAppContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("WebAppContext")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.Configure<CookiePolicyOptions>(options =>
              {
                                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                  options.CheckConsentNeeded = context => false;       //改为false或者直接注释掉，上面的Session才能正常使用
                                 options.MinimumSameSitePolicy = SameSiteMode.None;
                            });
            services.AddDistributedMemoryCache();
            services.AddMvc(); 
            services.AddSession();
            services.AddRazorPages();
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();

            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseCookiePolicy();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Teaches}/{action=Page}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
