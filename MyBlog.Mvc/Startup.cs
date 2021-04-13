using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyBlog.Services.AutoMapper.Profiles;
using MyBlog.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyBlog.Mvc
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
            //RazorRuntime k�t�phanesi watch gibi �al���r,de�i�iklikleri anl�k olarak g�rebiliriz
            //AddJsonOptions k�sm�nda json iletimi ile ilgili konfig�rasyonlar� tan�ml�yoruz
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddJsonOptions(opt=> 
            {
                //Enum kullan�m� i�in ekledik
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                //��i�e json datalar i�in ekledik
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            }); 

            //�� Katman�nda ki ba��ml�l�klar� ��zd���m�z extension s�n�f�m�z
            services.LoadMyServices();

            services.AddAutoMapper(typeof(CategoryProfile), typeof(ArticleProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages(); //404 hata vb... y�nlendirmeler i�in
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
                    );
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
