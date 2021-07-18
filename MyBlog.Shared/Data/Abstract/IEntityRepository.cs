using MyBlog.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Shared.Data.Abstract
{
    public interface IEntityRepository<T> where T:class,IEntity,new()
    {
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        //Daha Geniş Bir Filtre Kontrolü yapıtğımız override metot oluşturmuş olduk
        Task<T> GetAsyncV2(IList<Expression<Func<T, bool>>> predicates,IList<Expression<Func<T, object>>> includeProperties);

        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties);
        //Daha Geniş Bir Filtre Kontrolü yapıtğımız override metot oluşturmuş olduk
        Task<IList<T>> GetAllAsyncV2(IList<Expression<Func<T, bool>>> predicates, IList<Expression<Func<T, object>>> includeProperties);


        //Ekleme güncelleme işlemlerinde data ya erişebilmek için manipüle edilen datayı işlem sonucunda geri dönderiyoruz
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        //Çeşitli parametlere göre Arama işlemi yapılacaktır
        Task<IList<T>> SearchAsync(IList<Expression<Func<T,bool>>> predicates, params Expression<Func<T, object>>[] includeProperties);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate=null);

        //Karmaşık sorgular için oluşturduk
        IQueryable<T> GetAsQueryable();
    }
}
