using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Abstract;
using MyBlog.Data.Concrete.EntityFramework.Contexts;
using MyBlog.Entities.Concrete;
using MyBlog.Shared.Data.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.Concrete.EntityFramework.Repositories
{
   public class EfCategoryRepository : EfEntityRepositoryBase<Category>, ICategoryRepository
    {
        //Protected olarak tanımladığımız DbContext e  erişip gerekli harici metotlarda db işlemleri yapabiliyoruz
        private MyBlogContext BlogContext { get { return _context as MyBlogContext; } }

        //UnitofWork ile alacağımız context nesnemizi base sınıfımıza gönderiyoruz
        public EfCategoryRepository(DbContext context) : base(context)
        {

        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await BlogContext.Categories.SingleOrDefaultAsync(c=>c.Id==id);
        }

        
    }
}
