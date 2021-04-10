using Microsoft.Extensions.DependencyInjection;
using MyBlog.Data.Abstract;
using MyBlog.Data.Concrete;
using MyBlog.Data.Concrete.EntityFramework.Contexts;
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
        public static IServiceCollection LoadMyServices(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddDbContext<MyBlogContext>();
            serviceDescriptors.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceDescriptors.AddScoped<ICategoryService, CategoryManager>();
            serviceDescriptors.AddScoped<IArticleService, ArticleManager>();

            return serviceDescriptors;
        }
    }
}
