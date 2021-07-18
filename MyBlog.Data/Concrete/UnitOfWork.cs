using MyBlog.Data.Abstract;
using MyBlog.Data.Concrete.EntityFramework.Contexts;
using MyBlog.Data.Concrete.EntityFramework.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyBlogContext _context;
        
        private EfArticleRepository _articleRepository;
        private EfCommentRepository _commentRepository;
        private EfCategoryRepository _categoryRepository;


        public UnitOfWork(MyBlogContext context)
        {
            _context = context;
        }

        //?? operatörü ile repository Null ise new leme işlemi yapıyoruz
        //null ise ??= operatorü ile _entityRepository e (örneğin _articleRepository) atama işlemi yapıyoruz diğer türlü o repo hep boş kalacaktır
        public IArticleRepository Articles => _articleRepository ??=  new EfArticleRepository(_context);

        public ICategoryRepository Categories => _categoryRepository ??= new EfCategoryRepository(_context);

        public ICommentRepository Comments => _commentRepository ??= new EfCommentRepository(_context);

        //GarbageCollector ı tetiklemek için oluşturduğumuz bir yapı
        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
