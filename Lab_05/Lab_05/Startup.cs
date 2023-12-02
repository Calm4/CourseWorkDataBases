using Lab_05.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Lab_05.Middleware;
using Microsoft.AspNetCore.Identity;

namespace Lab_05
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
            string? connection = Configuration.GetConnectionString("DefaultConnection");


            services.AddDbContext<NotePlannerDbContext>(options => options.UseSqlServer(connection));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<NotePlannerDbContext>()
                .AddDefaultTokenProviders();


            services.AddControllersWithViews(options =>
            {
                options.CacheProfiles.Add("Caching",
                    new CacheProfile()
                    {
                        Duration = 248
                    });
            });
            services.AddSession();

            services.AddMemoryCache();
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
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseDbInitializer();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
