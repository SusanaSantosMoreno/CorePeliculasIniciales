using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorePeliculasIniciales.Data;
using CorePeliculasIniciales.Helpers;
using CorePeliculasIniciales.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MvcCore {
    public class Startup {

        private IConfiguration configuration;

        public Startup(IConfiguration conf) { this.configuration = conf; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddTransient<PeliculasRepository>();
            services.AddDbContext<PeliculasContext>(options =>
               options.UseSqlServer(this.configuration.GetConnectionString("ConnectionCasa")));
            services.AddSingleton<PathProvider>();
            services.AddSingleton<UploadService>();

            /*CACHING*/
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddResponseCaching();

            /*SESSION*/
            services.AddDistributedMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseSession();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Peliculas}/{action=Index}/{id?}"
                );
            });
        }
    }
}
