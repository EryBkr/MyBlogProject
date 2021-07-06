using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyBlog.Data.Abstract;
using MyBlog.Data.Concrete;
using MyBlog.Data.Concrete.EntityFramework.Contexts;
using MyBlog.Entities.Concrete;
using MyBlog.Services.Abstract;
using MyBlog.Services.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection LoadMyServices(this IServiceCollection serviceDescriptors,string connectionString)
        {
            //Connection String imiz appsettings te bulunduğu için migration işlemlerimizi Data katmanına gelerek ve başlangıç projesini UI olarak belirleyerek yapmamız gerekmektedir
            //dotnet ef --startup-project.. /MyBlog.Mvc  migrations add SeedingCategories
            //dotnet ef  --startup-project.. /MyBlog.Mvc database update

            serviceDescriptors.AddDbContext<MyBlogContext>(opt=>opt.UseSqlServer(connectionString));
            serviceDescriptors.AddIdentity<User, Role>(opt=> 
            {
                //Password Options
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 3;
                opt.Password.RequiredUniqueChars = 0;//Unique karakterlerden kaç tane olması gerekiyor
                opt.Password.RequireNonAlphanumeric = false; //Özel karakterler kullanılabilir mi?
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                //User & Email Options
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; //İzin verilen karakterler
                opt.User.RequireUniqueEmail = true; //email adresi unique mi olsun
            }).AddEntityFrameworkStores<MyBlogContext>();
            serviceDescriptors.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceDescriptors.AddScoped<ICategoryService, CategoryManager>();
            serviceDescriptors.AddScoped<IArticleService, ArticleManager>();
            serviceDescriptors.AddScoped<ICommentService, CommentManager>();

            return serviceDescriptors;
        }
    }
}
