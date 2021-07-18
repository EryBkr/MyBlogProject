using LinqKit;
using Microsoft.EntityFrameworkCore;
using MyBlog.Shared.Data.Abstract;
using MyBlog.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyBlog.Shared.Data.Concrete.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        //Protected olarak tanımladığımız _context kalıtım alınan class lardan erişip gerekli harici metotlarda db işlemleri yapabiliyoruz
        protected readonly DbContext _context;

        public EfEntityRepositoryBase(DbContext context)
        {
            _context = context;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate=null)
        {
            //Filte yok ise bütün kayıtların sayısı bizlere dönecektir
            return await (predicate==null ? _context.Set<TEntity>().CountAsync() : _context.Set<TEntity>().CountAsync(predicate));
        }

        public async Task DeleteAsync(TEntity entity)
        {
            //Remove metodunun asenkron hali olmadığı için böyle bir çözüm bulduk
            await Task.Run(() => { _context.Set<TEntity>().Remove(entity); });
        }

        public async Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            //İstek Database e gitmeden sorguları biriktirmek için bir yapı kurduk
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (predicate != null)
                query = query.Where(predicate);

            //Include edilmesi istenen bir yapı var ise include işlemleri sağlanacaktır
            if (includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            //AsNoTracking(). ile sadece include ettiğimiz yapılar gelecek o yapıların içerisindekileri de include etmeyecek
            return await query.AsNoTracking().ToListAsync();

        }

        //Daha Geniş Bir Filtre Kontrolü yapıtğımız override metot oluşturmuş olduk
        public async Task<IList<TEntity>> GetAllAsyncV2(IList<Expression<Func<TEntity, bool>>> predicates, IList<Expression<Func<TEntity, object>>> includeProperties)
        {
            //İstek Database e gitmeden sorguları biriktirmek için bir yapı kurduk
            IQueryable<TEntity> query = _context.Set<TEntity>();

            //Herhangi bir filtre verilmiş mi?
            if (predicates != null && predicates.Any())
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }


            //Include edilmesi istenen bir yapı var ise include işlemleri sağlanacaktır
            if (includeProperties != null && includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            // AsNoTracking().ile sadece include ettiğimiz yapılar gelecek o yapıların içerisindekileri de include etmeyecek
            return await query.AsNoTracking().ToListAsync();
        }

        //Uzun ve karışık sorgular için oluşturduk
        public IQueryable<TEntity> GetAsQueryable()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            //İstek Database e gitmeden sorguları biriktirmek için bir yapı kurduk
            IQueryable<TEntity> query = _context.Set<TEntity>();
            query = query.Where(predicate);

            //Include edilmesi istenen bir yapı var ise include işlemleri sağlanacaktır
            if (includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            //AsNoTracking(). ile sadece include ettiğimiz yapılar gelecek o yapıların içerisindekileri de include etmeyecek
            return await query.AsNoTracking().SingleOrDefaultAsync();
        }

        //Daha Geniş Bir Filtre Kontrolü yapıtğımız override metot oluşturmuş olduk
        public async Task<TEntity> GetAsyncV2(IList<Expression<Func<TEntity, bool>>> predicates, IList<Expression<Func<TEntity, object>>> includeProperties)
        {
            //İstek Database e gitmeden sorguları biriktirmek için bir yapı kurduk
            IQueryable<TEntity> query = _context.Set<TEntity>();

            //Herhangi bir filtre verilmiş mi?
            if (predicates!=null && predicates.Any())
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }
          

            //Include edilmesi istenen bir yapı var ise include işlemleri sağlanacaktır
            if (includeProperties!=null && includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            //AsNoTracking(). ile sadece include ettiğimiz yapılar gelecek o yapıların içerisindekileri de include etmeyecek
            return await query.AsNoTracking().SingleOrDefaultAsync();
        }

        public async Task<IList<TEntity>> SearchAsync(IList<Expression<Func<TEntity, bool>>> predicates, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            //Filtrelerle ilgili işlemler
            if (predicates.Any())
            {
                //Yapılan filtreleri && operatörü ile değilde || ile birleştirmek için LinqKit kullandık.Diğer türlü verilen şartların tamamının gerçekleşmesi gerekirdi ki biz bunu istemiyoruz
                var predicateChain = PredicateBuilder.New<TEntity>();
                foreach (var predicate in predicates)
                {
                    predicateChain.Or(predicate);
                }

                query = query.Where(predicateChain);
            }

            //Join işlemleri
            if (includeProperties.Any())
            {
                foreach (var include in includeProperties)
                {
                    query = query.Include(include);
                }
            }

            //AsNoTracking() ile sadece include ettiğimiz yapılar gelecek o yapıların içerisindekileri de include etmeyecek
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            //Update metodunun asenkron hali olmadığı için böyle bir çözüm bulduk
            await Task.Run(() => { _context.Set<TEntity>().Update(entity); });
            return entity;
        }
    }
}
