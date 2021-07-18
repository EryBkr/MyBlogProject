using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Abstract;
using MyBlog.Entities.ComplexTypes;
using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos.ArticleDtos;
using MyBlog.Services.Abstract;
using MyBlog.Services.Utilities;
using MyBlog.Shared.Entities.Concrete;
using MyBlog.Shared.Utilities.Results.Abtracts;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using MyBlog.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Services.Concrete
{
    public class ArticleManager : ManagerBase, IArticleService
    {
        private readonly UserManager<User> _userManager;

        //Dolu halleri Base Class ımızdan gelecektir
        public ArticleManager(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
        }

        public async Task<IResult> AddAsync(ArticleAddDto articleAddDto, string createdByName, int userId)
        {
            var article = Mapper.Map<Article>(articleAddDto);
            article.CreatedByName = createdByName;
            article.ModifiedByName = createdByName;
            article.UserId = userId; //Tablo ilişkisi için ekledik

            await UnitOfWork.Articles.AddAsync(article);//.ContinueWith(t=>UnitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
            await UnitOfWork.SaveAsync();

            return new Result(ResultStatus.Success, Messages.Article.Add(article.Title));
        }

        public async Task<IResult> DeleteAsync(int articleId, string modifiedByName)
        {
            var result = UnitOfWork.Articles.AnyAsync(i => i.Id == articleId);

            if (result != null) //Bu Id ye sahip article var ise
            {
                var article = await UnitOfWork.Articles.GetAsync(i => i.Id == articleId);
                article.ModifiedByName = modifiedByName;
                article.IsDeleted = true;
                article.IsActive = false;
                article.ModifiedDate = DateTime.Now;
                await UnitOfWork.Articles.UpdateAsync(article);//.ContinueWith(t=>UnitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
                await UnitOfWork.SaveAsync();

                return new Result(ResultStatus.Success, Messages.Article.Delete(article.Title));
            }

            return new Result(ResultStatus.Error, Messages.Article.NotFound(false));
        }

        public async Task<IDataResult<ArticleDto>> GetAsync(int articleId)
        {
            //İki farklı tabloyu include ettik
            var article = await UnitOfWork.Articles.GetAsync(i => i.Id == articleId, i => i.User, i => i.Category);

            if (article != null)
            {
                article.Comments = await UnitOfWork.Comments.GetAllAsync(i => i.ArticleId == articleId && !i.IsDeleted && i.IsActive);

                return new DataResult<ArticleDto>(ResultStatus.Success, new ArticleDto
                {
                    Article = article,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleDto>(ResultStatus.Error, null, Messages.Article.NotFound(false));
        }

        public async Task<IDataResult<ArticleListDto>> GetAllAsync()
        {
            var articles = await UnitOfWork.Articles.GetAllAsync(null, i => i.User, i => i.Category);
            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, null, Messages.Article.NotFound(true));
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByCategoryAsync(int categoryId)
        {
            var result = await UnitOfWork.Categories.AnyAsync(i => i.Id == categoryId); //Böyle bir kategori var mı?

            if (result) //Varsa...
            {
                var articles = await UnitOfWork.Articles.GetAllAsync(i => i.CategoryId == categoryId && !i.IsDeleted && i.IsActive, i => i.Category, i => i.User);
                if (articles.Count > -1)
                {
                    return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                    {
                        Articles = articles,
                        ResultStatus = ResultStatus.Success
                    });
                }
                else
                {
                    return new DataResult<ArticleListDto>(ResultStatus.Error, null, Messages.Article.NotFoundByCategory());
                }
            }
            else
            {
                return new DataResult<ArticleListDto>(ResultStatus.Error, null, Messages.Article.NotFound(false));
            }
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAsync()
        {
            var articles = await UnitOfWork.Articles.GetAllAsync(i => !i.IsDeleted, i => i.User, i => i.Category);
            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, null, Messages.Article.NotFound(true));
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActiveAsync()
        {
            var articles = await UnitOfWork.Articles.GetAllAsync(i => !i.IsDeleted && i.IsActive, i => i.User, i => i.Category);
            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, null, Messages.Article.NotFound(true));
        }

        public async Task<IResult> HardDeleteAsync(int articleId)
        {
            var result = UnitOfWork.Articles.AnyAsync(i => i.Id == articleId);

            if (result != null) //Bu Id ye sahip article var ise
            {
                var article = await UnitOfWork.Articles.GetAsync(i => i.Id == articleId);

                await UnitOfWork.Articles.DeleteAsync(article);//.ContinueWith(t=>UnitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
                await UnitOfWork.SaveAsync();

                return new Result(ResultStatus.Success, Messages.Article.Delete(article.Title));
            }

            return new Result(ResultStatus.Error, Messages.Article.NotFound(false));
        }

        public async Task<IResult> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedByName)
        {
            var oldArticle = await UnitOfWork.Articles.GetAsync(i => i.Id == articleUpdateDto.Id);

            //Güncelleme esnasında bütün propertyler için giriş yapmıyoruz.Bundan dolayı eski makale üzerine yeni bilgileri yazmak ve değişmeyen bilgileri kaybetmemek için eski makalenin üzerine map ediyoruz
            var article = Mapper.Map<ArticleUpdateDto, Article>(articleUpdateDto, oldArticle);
            article.ModifiedByName = modifiedByName;

            await UnitOfWork.Articles.UpdateAsync(article);//.ContinueWith(t=>UnitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
            await UnitOfWork.SaveAsync();

            return new Result(ResultStatus.Success, Messages.Article.Update(article.Title));
        }

        public async Task<IDataResult<int>> CountAsync()
        {
            var articlesCount = await UnitOfWork.Articles.CountAsync();
            if (articlesCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, articlesCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Error, 0, "Beklenmeyen bir hata ile karşılaşıldı");
            }

        }

        public async Task<IDataResult<int>> CountByIsNonDeletedAsync()
        {
            var articlesCount = await UnitOfWork.Articles.CountAsync(i => !i.IsDeleted);
            if (articlesCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, articlesCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Error, 0, "Beklenmeyen bir hata ile karşılaşıldı");
            }
        }

        public async Task<IDataResult<ArticleUpdateDto>> GetArticleUpdateDtoAsync(int articleId)
        {
            //Verilen  Id ye ait kategori var mı?
            var result = await UnitOfWork.Articles.AnyAsync(i => i.Id == articleId);
            if (result)
            {
                var article = await UnitOfWork.Articles.GetAsync(c => c.Id == articleId);
                var articleUpdateDto = Mapper.Map<ArticleUpdateDto>(article);
                return new DataResult<ArticleUpdateDto>(ResultStatus.Success, articleUpdateDto);
            }
            return new DataResult<ArticleUpdateDto>(ResultStatus.Error, null, Messages.Article.NotFound(false));
        }

        //silinmiş makaleleri geri getirdik
        public async Task<IDataResult<ArticleListDto>> GetAllByDeletedAsync()
        {
            var articles = await UnitOfWork.Articles.GetAllAsync(i => i.IsDeleted, i => i.User, i => i.Category);
            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, null, Messages.Article.NotFound(true));
        }

        //silinmiş makaleyi geri getirdik
        public async Task<IResult> UndoDeleteAsync(int articleId, string modifiedByName)
        {
            var result = UnitOfWork.Articles.AnyAsync(i => i.Id == articleId);

            if (result != null) //Bu Id ye sahip article var ise
            {
                var article = await UnitOfWork.Articles.GetAsync(i => i.Id == articleId);
                article.ModifiedByName = modifiedByName;
                article.IsDeleted = false;
                article.IsActive = true;
                article.ModifiedDate = DateTime.Now;
                await UnitOfWork.Articles.UpdateAsync(article);//.ContinueWith(t=>UnitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
                await UnitOfWork.SaveAsync();

                return new Result(ResultStatus.Success, Messages.Article.NonDelete(article.Title));
            }

            return new Result(ResultStatus.Error, Messages.Article.NotFound(false));
        }

        /// <summary>
        /// En çok okunan makaleleri alacağız
        /// </summary>
        /// <param name="isAscending">Artan ya da azalan sıralamayı belirliyoruz</param>
        /// <param name="takeSize">Kaç adet makalenin bize gelmesini istiyoruz</param>
        /// <returns></returns>
        public async Task<IDataResult<ArticleListDto>> GetAllByViewCountAsync(bool isAscending, int? takeSize)
        {
            var articles = await UnitOfWork.Articles.GetAllAsync(a => a.IsActive && !a.IsDeleted, a => a.Category, a => a.User);

            //Artan mı azalan mı sıralama
            var sortedArticles = isAscending ? articles.OrderBy(i => i.ViewsCount) : articles.OrderByDescending(i => i.ViewsCount);

            return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
            {
                Articles = takeSize == null ? sortedArticles.ToList() : sortedArticles.Take(takeSize.Value).ToList()
            });
        }

        /// <summary>
        /// Sayfalama yapısına uygun Listeleme
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        public async Task<IDataResult<ArticleListDto>> GetAllByPagingAsync(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            //İstenmeyen bir pageSize değeri gelirse bunu kontrol ediyoruz
            pageSize = pageSize > 20 ? 20 : pageSize;

            var articles = categoryId == null
                ? await UnitOfWork.Articles.GetAllAsync(a => a.IsActive && !a.IsDeleted, a => a.Category, a => a.User)
                : await UnitOfWork.Articles.GetAllAsync(a => a.IsActive && !a.IsDeleted && a.CategoryId == categoryId.Value, a => a.Category, a => a.User);

            //Pagination ve Sıralama İşlemleri
            var sortedArticles = isAscending
                ? articles.OrderBy(i => i.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : articles.OrderByDescending(i => i.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
            {
                Articles = sortedArticles,
                CategoryId = categoryId,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count,
                IsAscending = isAscending
            });
        }



        public async Task<IDataResult<ArticleListDto>> SearchAsync(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            //İstenmeyen bir pageSize değeri gelirse bunu kontrol ediyoruz
            pageSize = pageSize > 20 ? 20 : pageSize;

            //Arama kısmı dolu mu , bir harf ya da kelime var mı
            if (string.IsNullOrWhiteSpace(keyword))
            {
                var articles = await UnitOfWork.Articles.GetAllAsync(a => a.IsActive && !a.IsDeleted, a => a.Category, a => a.User);

                //Pagination ve Sıralama İşlemleri
                var sortedArticles = isAscending
                    ? articles.OrderBy(i => i.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                    : articles.OrderByDescending(i => i.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = sortedArticles,
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    TotalCount = articles.Count,
                    IsAscending = isAscending
                });
            }

            //Arama Filtreleri uygulandı
            var searchedArticles = await UnitOfWork.Articles.SearchAsync(new List<Expression<Func<Article, bool>>>
            {
                (a)=>a.Title.Contains(keyword),
                (a)=>a.Category.Name.Contains(keyword),
                (a)=>a.SeoDescription.Contains(keyword),
                (a)=>a.SeoTags.Contains(keyword)
            }, a => a.Category, a => a.User);

            //Pagination ve Sıralama İşlemleri Uygunlandı
            var searchedSortedArticles = isAscending
                ? searchedArticles.OrderBy(i => i.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : searchedArticles.OrderByDescending(i => i.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
            {
                Articles = searchedSortedArticles,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = searchedArticles.Count,
                IsAscending = isAscending
            });
        }


        /// <summary>
        /// Makalenin Görüntülenme sayısını arttırır
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<IResult> IncreaseViewCountAsync(int articleId)
        {
            var article = await UnitOfWork.Articles.GetAsync(i => i.Id == articleId);
            if (article == null)
                return new Result(ResultStatus.Error, Messages.Article.NotFound(false));

            article.ViewsCount += 1;
            await UnitOfWork.Articles.UpdateAsync(article);
            await UnitOfWork.SaveAsync();

            return new Result(ResultStatus.Success, Messages.Article.IncreaseViewCount(article.Title));
        }

        /// <summary>
        /// Sidebar tarafında görüntülenecek olan makalelerin özelliklerini belirliyoruz.Filter kısmı gelecek makaleleri belirli standartlara göre filtrelerken orderby kısmı gelen makaleleri istediğimiz sırada
        /// bize getiriyor
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="filterBy"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAscending"></param>
        /// <param name="takeSize"></param>
        /// <param name="categoryId"></param>
        /// <param name="startAt"></param>
        /// <param name="endAt"></param>
        /// <param name="minViewCount"></param>
        /// <param name="maxViewCount"></param>
        /// <param name="minCommentsCount"></param>
        /// <param name="maxCommentsCount"></param>
        /// <returns></returns>
        public async Task<IDataResult<ArticleListDto>> GetAllByUserIdOnFilter(int userId, FilterBy filterBy, OrderBy orderBy, bool isAscending, int takeSize, int categoryId, DateTime startAt, DateTime endAt, int minViewCount, int maxViewCount, int minCommentsCount, int maxCommentsCount)
        {
            var anyUser = await _userManager.Users.AnyAsync(i => i.Id == userId);
            if (!anyUser)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Error, null, $"{userId} numaralı kullanıcı bulunamadı.");
            }

            //Kullanıcıya ait makaleleri aldık
            var userArticles = await UnitOfWork.Articles.GetAllAsync(a => a.IsActive && !a.IsDeleted && a.UserId == userId);
            var sortedArticles = new List<Article>();

            //Neye göre filtrelediğimizi belirlediğimiz gibi filtreleme seçeneğimizin azalan mı artan mı sıralama olması gerektiğine karar vermemiz gerekiyor
            switch (filterBy)
            {
                case FilterBy.Category: //Kategori temelli filtreleme
                    switch (orderBy)
                    {
                        case OrderBy.Date: //Tarihe göre sıralama
                            sortedArticles = isAscending ? userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderBy(a => a.Date).ToList() : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderByDescending(a => a.Date).ToList();
                            break;
                        case OrderBy.ViewCount: //Görüntülenme sayısına göre sıralama
                            sortedArticles = isAscending ? userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderBy(a => a.ViewsCount).ToList() : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderByDescending(a => a.ViewsCount).ToList();
                            break;
                        case OrderBy.CommentCount: //Yorum sayısına göre sıralama
                            sortedArticles = isAscending ? userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderBy(a => a.CommentsCount).ToList() : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderByDescending(a => a.CommentsCount).ToList();
                            break;
                        default:
                            break;
                    }
                    break;
                case FilterBy.Date: //Tarih aralıklı filtreleme
                    switch (orderBy)
                    {
                        case OrderBy.Date:
                            sortedArticles = isAscending ? userArticles.Where(a => a.Date >= startAt && a.Date <= endAt).Take(takeSize).OrderBy(a => a.Date).ToList() : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderByDescending(a => a.Date).ToList();
                            break;
                        case OrderBy.ViewCount:
                            sortedArticles = isAscending ? userArticles.Where(a => a.Date >= startAt && a.Date <= endAt).Take(takeSize).OrderBy(a => a.ViewsCount).ToList() : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderByDescending(a => a.ViewsCount).ToList();
                            break;
                        case OrderBy.CommentCount:
                            sortedArticles = isAscending ? userArticles.Where(a => a.Date >= startAt && a.Date <= endAt).Take(takeSize).OrderBy(a => a.CommentsCount).ToList() : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderByDescending(a => a.CommentsCount).ToList();
                            break;
                        default:
                            break;
                    }
                    break;
                case FilterBy.ViewCount: //Görüntülenme sayısı aralıklı filtreleme
                    switch (orderBy)
                    {
                        case OrderBy.Date:
                            sortedArticles = isAscending ? userArticles.Where(a => a.ViewsCount >= minViewCount && a.ViewsCount <= maxViewCount).Take(takeSize).OrderBy(a => a.ViewsCount).ToList() : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderByDescending(a => a.ViewsCount).ToList();
                            break;
                        case OrderBy.ViewCount:
                            sortedArticles = isAscending ? userArticles.Where(a => a.ViewsCount >= minViewCount && a.ViewsCount <= maxViewCount).Take(takeSize).OrderBy(a => a.ViewsCount).ToList() : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderByDescending(a => a.ViewsCount).ToList();
                            break;
                        case OrderBy.CommentCount:
                            sortedArticles = isAscending ? userArticles.Where(a => a.ViewsCount >= minViewCount && a.ViewsCount <= maxViewCount).Take(takeSize).OrderBy(a => a.CommentsCount).ToList() : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderByDescending(a => a.CommentsCount).ToList();
                            break;
                        default:
                            break;
                    }
                    break;
                case FilterBy.CommentCount: //Yorum sayısı aralıklı filtreleme
                    switch (orderBy)
                    {
                        case OrderBy.Date:
                            sortedArticles = isAscending ? userArticles.Where(a => a.CommentsCount >= minCommentsCount && a.CommentsCount <= maxCommentsCount).Take(takeSize).OrderBy(a => a.Date).ToList() : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderByDescending(a => a.Date).ToList();
                            break;
                        case OrderBy.ViewCount:
                            sortedArticles = isAscending ? userArticles.Where(a => a.CommentsCount >= minCommentsCount && a.CommentsCount <= maxCommentsCount).Take(takeSize).OrderBy(a => a.Date).ToList() : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderByDescending(a => a.Date).ToList();
                            break;
                        case OrderBy.CommentCount:
                            sortedArticles = isAscending ? userArticles.Where(a => a.CommentsCount >= minCommentsCount && a.CommentsCount <= maxCommentsCount).Take(takeSize).OrderBy(a => a.Date).ToList() : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderByDescending(a => a.Date).ToList();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            if (sortedArticles != null)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto { Articles = sortedArticles });
            }
            else
            {
                return new DataResult<ArticleListDto>(ResultStatus.Error, null, Messages.Article.NotFound(true));
            }

        }


        /// <summary>
        /// Include işlemleri için daha merkezi bir kontrol oluşturmaya çalıştık.
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="includeCategory"></param>
        /// <param name="includeComments"></param>
        /// <param name="includeUser"></param>
        /// <returns></returns>
        public async Task<IDataResult<ArticleDto>> GetByIdAsync(int articleId, bool includeCategory, bool includeComments, bool includeUser)
        {
            //Filtreler
            List<Expression<Func<Article, bool>>> predicates = new List<Expression<Func<Article, bool>>>();

            //Joinler
            List<Expression<Func<Article, object>>> includes = new List<Expression<Func<Article, object>>>();

            //Join İşlemleri
            if (includeCategory) includes.Add(i => i.Category);
            if (includeComments) includes.Add(i => i.Comments);
            if (includeUser) includes.Add(i => i.User);

            //Filtre Uygulandı
            predicates.Add(i => i.Id == articleId);

            //Database'e gitti
            var article = await UnitOfWork.Articles.GetAsyncV2(predicates, includes);

            if (article == null)
            {
                return new DataResult<ArticleDto>(ResultStatus.Warning, null, new List<ValidationError>
                {
                    new ValidationError{PropertyName="articleId",Message=Messages.Article.NotFoundById(articleId)}
                });
            }

            return new DataResult<ArticleDto>(ResultStatus.Success, new ArticleDto { Article = article });

        }


        /// <summary>
        /// Daha genel bir getAll metodudur.Bir çok filtremizi tek bir metot üzerinde uygulamamızı sağlar
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="userId"></param>
        /// <param name="isActive"></param>
        /// <param name="isDeleted"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAscending"></param>
        /// <param name="includeCategory"></param>
        /// <param name="includeComments"></param>
        /// <param name="includeUser"></param>
        /// <returns></returns>
        public async Task<IDataResult<ArticleListDto>> GetAllAsyncV2(int? categoryId, int? userId, bool? isActive, bool? isDeleted, int currentPage, int pageSize, OrderByGeneral orderBy, bool isAscending, bool includeCategory, bool includeComments, bool includeUser)
        {
            //Filtreler
            List<Expression<Func<Article, bool>>> predicates = new List<Expression<Func<Article, bool>>>();

            //Joinler
            List<Expression<Func<Article, object>>> includes = new List<Expression<Func<Article, object>>>();

            //Join İşlemleri
            if (includeCategory) includes.Add(i => i.Category);
            if (includeComments) includes.Add(i => i.Comments);
            if (includeUser) includes.Add(i => i.User);

            //Filtre İşlemleri
            if (categoryId.HasValue)
            {
                //Verilen CategoryId e ait bir kayıt var mı
                if (!await UnitOfWork.Categories.AnyAsync(i => i.Id == categoryId))
                {
                    return new DataResult<ArticleListDto>(ResultStatus.Warning, null, new List<ValidationError>
                {
                    new ValidationError{PropertyName="categoryId",Message=Messages.Category.NotFoundById(categoryId.Value)}
                });
                }
                predicates.Add(a => a.CategoryId == categoryId.Value);
            }
            if (userId.HasValue)
            {
                //Verilen userId e ait bir kayıt var mı
                if (!await _userManager.Users.AnyAsync(u => u.Id == userId.Value))
                {
                    return new DataResult<ArticleListDto>(ResultStatus.Warning, null, new List<ValidationError>
                {
                    new ValidationError{PropertyName="categoryId",Message=Messages.Users.NotFoundById(userId.Value)}
                });
                }
                predicates.Add(a => a.UserId == userId.Value);
            }
            if (isActive.HasValue) predicates.Add(a => a.IsActive == isActive.Value);
            if (isDeleted.HasValue) predicates.Add(a => a.IsDeleted == isDeleted.Value);

            //Joinlerin ve Filtrelerin sonucuna uygun makaleleri alıyoruz
            var articles = await UnitOfWork.Articles.GetAllAsyncV2(predicates, includes);

            //Sıralanmış Tanım
            IOrderedEnumerable<Article> sortedArticles;

            //verilen orderBy parametresine uygun olarak bir sıralama yapılacaktır
            switch (orderBy)
            {
                case OrderByGeneral.Id:
                    sortedArticles = isAscending ? articles.OrderBy(a => a.Id) : articles.OrderByDescending(i => i.Id);
                    break;
                case OrderByGeneral.AZ:
                    sortedArticles = isAscending ? articles.OrderBy(a => a.Title) : articles.OrderByDescending(i => i.Title);
                    break;
                case OrderByGeneral.CreatedDate:
                    sortedArticles = isAscending ? articles.OrderBy(a => a.CreatedDate) : articles.OrderByDescending(i => i.CreatedDate);
                    break;
                default:
                    sortedArticles = isAscending ? articles.OrderBy(a => a.CreatedDate) : articles.OrderByDescending(i => i.CreatedDate);
                    break;
            }

            return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
            {
                Articles = sortedArticles.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList(),
                CategoryId = categoryId.HasValue ? categoryId.Value : null,
                CurrentPage = currentPage,
                PageSize = pageSize,
                IsAscending = isAscending,
                TotalCount = articles.Count,
                ResultStatus = ResultStatus.Success
            });
        }
    }
}
