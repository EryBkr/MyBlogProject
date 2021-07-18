using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyBlog.Entities.Concrete;
using MyBlog.Mvc.AutoMapper.Profiles;
using MyBlog.Mvc.Filters;
using MyBlog.Mvc.Helpers.Abstract;
using MyBlog.Mvc.Helpers.Concrete;
using MyBlog.Services.AutoMapper.Profiles;
using MyBlog.Services.Extensions;
using MyBlog.Shared.Utilities.Extensions;
using NToastNotify;
using System.Text.Json.Serialization;

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
            //RazorRuntime kütüphanesi watch gibi çalýþýr,deðiþiklikleri anlýk olarak görebiliriz
            //AddJsonOptions kýsmýnda json iletimi ile ilgili konfigürasyonlarý tanýmlýyoruz
            //Toastr Mesajlarýný C# tarafýnda oluþturabilmemizi saðlayan yapýyý konfigüre ettik
            services.AddControllersWithViews(opt =>
            {
                //Null Exception Hatasýný Türkçeleþtirdik
                opt.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(val => "Bu Alan Boþ Geçilmemelidir");

                //Hata kontrol filter imizi middleware kýsmýmýza ekledik
                opt.Filters.Add<MvcExceptionFilter>();
            }).AddNToastNotifyToastr(new ToastrOptions
            {
                TimeOut = 5000
            }).AddRazorRuntimeCompilation().AddJsonOptions(opt =>
            {
                //Enum kullanýmý için ekledik
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                //Ýçiçe json datalar için ekledik
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });

            services.AddSession();

            //Hakkýmýzda sayfamýz için oluþturduðumuz modelimize appsettings teki verilerimizi bind edecektir.
            services.Configure<AboutUsPageInfo>(Configuration.GetSection("AboutUsPageInfo"));

            //Layout sayfamýz için oluþturduðumuz modelimize appsettings teki verilerimizi bind edecektir.
            services.Configure<WebSiteInfo>(Configuration.GetSection("WebSiteInfo"));

            //Mail göndermek için oluþturduðumuz smtp konfigürasyonlarýmýz modelimize appsettings teki verilerimizi bind edecektir.
            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));

            //Okunan makale ile alakalý sayfada gösterilecek makalelerin hangi þartlarla ve sýrayla geleceðinizi belirleyen appsettings.json da ki verileri modelimize bind edecektir
            services.Configure<ArticleRightSideBarWidgetOptions>(Configuration.GetSection("ArticleRightSideBarWidgetOptions"));

            //Ýþ Katmanýnda ki baðýmlýlýklarý çözdüðümüz ve connection String bilgisini verdiðimiz extension sýnýfýmýz
            services.LoadMyServices(Configuration.GetConnectionString("LocalDB"));
            //Resim iþlemleri için oluþturduðumuz class ý implemente ettik
            services.AddScoped<IImageHelper, ImageHelper>();

            //Cookie Ayarlarý
            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = new PathString("/Admin/Auth/Login");//Yetki yok ise buraya yönlendiriyoruz
                opt.LogoutPath = new PathString("/Admin/Auth/Logout");
                opt.Cookie = new CookieBuilder
                {
                    Name = "MyBlog",
                    HttpOnly = true,//UI Tarafýndan cookie lere eriþilememesi için tanýmlýyoruz XSS önleniyor
                    SameSite = SameSiteMode.Strict,//CRSF Ataðýný önlemek için kullanýlýr.Cookie kendi sitemizden gelmelidir
                    SecurePolicy = CookieSecurePolicy.SameAsRequest,//HTTP den istek gelirse HTTP olarak HTTPS den istek gelirse ona uygun olarak dönülür.Olmasý Gereken Always dir
                };
                opt.SlidingExpiration = true;//Süre sýfýrlamasýný saðlar
                opt.ExpireTimeSpan = System.TimeSpan.FromDays(7);
                opt.AccessDeniedPath = new PathString("/Admin/Auth/AccessDenied"); //Yetkisi olmayan bir yere girmeye çalýþan üyenin yönlendireleceði yer
            });

            //appsettings üzerinde ki section ý deðiþtirebilmek için ekledik
            services.ConfigureWritable<AboutUsPageInfo>(Configuration.GetSection("AboutUsPageInfo"));
            //appsettings üzerinde ki section ý deðiþtirebilmek için ekledik
            services.ConfigureWritable<WebSiteInfo>(Configuration.GetSection("WebSiteInfo"));
            //appsettings üzerinde ki section ý deðiþtirebilmek için ekledik
            services.ConfigureWritable<SmtpSettings>(Configuration.GetSection("SmtpSettings"));
            //Okunan makale ile alakalý sayfada gösterilecek makalelerin hangi þartlarla ve sýrayla geleceðinizi belirleyen appsettings.json da ki verileri deðiþtirmemizi saðlayacaktýr
            services.ConfigureWritable<ArticleRightSideBarWidgetOptions>(Configuration.GetSection("ArticleRightSideBarWidgetOptions"));


            //AutoMappper Profile Class larýmýzý burada tanýmladýk.
            services.AddAutoMapper(typeof(CategoryProfile), typeof(ArticleProfile), typeof(UserProfile), typeof(ViewModelsProfile), typeof(CommentProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages(); //404 hata vb... yönlendirmeler için
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); //Kimlik kontrolü 
            app.UseAuthorization(); //Yetkilendirme


            //Toastr Mesajlarýný C# tarafýnda oluþturabilmemizi saðlayan kütüphaneyi projemize ekledik
            app.UseNToastNotify();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
                    );
                endpoints.MapControllerRoute( //SEO uyumlu makale URL i için ekledik
                    name: "article",
                    pattern: "Makaleler/{title}/{articleId}",
                    defaults: new { controller = "Article", action = "Detail" }
                    );
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
