using AutoMapper;
using MyBlog.Data.Abstract;
using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos.ArticleDtos;
using MyBlog.Services.Abstract;
using MyBlog.Shared.Utilities.Results.Abtracts;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using MyBlog.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Services.Concrete
{
    public class ArticleManager : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ArticleManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult> Add(ArticleAddDto articleAddDto, string createdByName)
        {
            var article = _mapper.Map<Article>(articleAddDto);
            article.CreatedByName = createdByName;
            article.ModifiedByName = createdByName;
            article.UserId = 1;

            await _unitOfWork.Articles.AddAsync(article);//.ContinueWith(t=>_unitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
            await _unitOfWork.SaveAsync();

            return new Result(ResultStatus.Success, $"{article.Title} başlıklı makale başarıyla eklenmiştir");
        }

        public async Task<IResult> Delete(int articleId, string modifiedByName)
        {
            var result = _unitOfWork.Articles.AnyAsync(i => i.Id == articleId);

            if (result != null) //Bu Id ye sahip article var ise
            {
                var article = await _unitOfWork.Articles.GetAsync(i => i.Id == articleId);
                article.ModifiedByName = modifiedByName;
                article.IsDeleted = true;
                article.ModifiedDate = DateTime.Now;
                await _unitOfWork.Articles.UpdateAsync(article);//.ContinueWith(t=>_unitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
                await _unitOfWork.SaveAsync();

                return new Result(ResultStatus.Success, $"{article.Title} başlıklı makale başarıyla silinmiştir");
            }

            return new Result(ResultStatus.Error,"Böyle bir makale bulunamadı");
        }

        public async Task<IDataResult<ArticleDto>> Get(int articleId)
        {
            //İki farklı tabloyu include ettik
            var article = await _unitOfWork.Articles.GetAsync(i => i.Id == articleId, i => i.User, i => i.Category);
            if (article != null)
            {
                return new DataResult<ArticleDto>(ResultStatus.Success, new ArticleDto
                {
                    Article = article,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleDto>(ResultStatus.Error, null, "Makale bulunamadı");
        }

        public async Task<IDataResult<ArticleListDto>> GetAll()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(null, i => i.User, i => i.Category);
            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, null, "Makaleler bulunamadı");
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByCategory(int categoryId)
        {
            var result = await _unitOfWork.Categories.AnyAsync(i => i.Id == categoryId); //Böyle bir kategori var mı?

            if (result) //Varsa...
            {
                var articles = await _unitOfWork.Articles.GetAllAsync(i => i.CategoryId == categoryId && !i.IsDeleted && i.IsActive, i => i.Category);
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
                    return new DataResult<ArticleListDto>(ResultStatus.Error, null, "Bu kategoriye ait makale bulunamadı");
                }
            }
            else
            {
                return new DataResult<ArticleListDto>(ResultStatus.Error, null, "Böyle bir kategori bulunamadı");
            }
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByNonDeleted()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(i => !i.IsDeleted, i => i.User, i => i.Category);
            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, null, "Makaleler bulunamadı");
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActive()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(i => !i.IsDeleted && i.IsActive, i => i.User, i => i.Category);
            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, null, "Makaleler bulunamadı");
        }

        public async Task<IResult> HardDelete(int articleId)
        {
            var result = _unitOfWork.Articles.AnyAsync(i => i.Id == articleId);

            if (result != null) //Bu Id ye sahip article var ise
            {
                var article = await _unitOfWork.Articles.GetAsync(i => i.Id == articleId);
              
                await _unitOfWork.Articles.DeleteAsync(article);//.ContinueWith(t=>_unitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
                await _unitOfWork.SaveAsync();

                return new Result(ResultStatus.Success, $"{article.Title} başlıklı makale başarıyla silinmiştir (Hard)");
            }

            return new Result(ResultStatus.Error, "Böyle bir makale bulunamadı");
        }

        public async Task<IResult> Update(ArticleUpdateDto articleUpdateDto, string modifiedByName)
        {
            var article = _mapper.Map<Article>(articleUpdateDto);
            article.ModifiedByName = modifiedByName;

            await _unitOfWork.Articles.UpdateAsync(article);//.ContinueWith(t=>_unitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
            await _unitOfWork.SaveAsync();

            return new Result(ResultStatus.Success, $"{article.Title} başlıklı makale başarıyla güncellenmiştir");
        }
    }
}
