﻿using AutoMapper;
using MyBlog.Data.Abstract;
using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos.ArticleDtos;
using MyBlog.Services.Abstract;
using MyBlog.Services.Utilities;
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

        public async Task<IResult> AddAsync(ArticleAddDto articleAddDto, string createdByName, int userId)
        {
            var article = _mapper.Map<Article>(articleAddDto);
            article.CreatedByName = createdByName;
            article.ModifiedByName = createdByName;
            article.UserId = userId; //Tablo ilişkisi için ekledik

            await _unitOfWork.Articles.AddAsync(article);//.ContinueWith(t=>_unitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
            await _unitOfWork.SaveAsync();

            return new Result(ResultStatus.Success,Messages.Article.Add(article.Title));
        }

        public async Task<IResult> DeleteAsync(int articleId, string modifiedByName)
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

                return new Result(ResultStatus.Success, Messages.Article.Delete(article.Title));
            }

            return new Result(ResultStatus.Error, Messages.Article.NotFound(false));
        }

        public async Task<IDataResult<ArticleDto>> GetAsync(int articleId)
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
            return new DataResult<ArticleDto>(ResultStatus.Error, null, Messages.Article.NotFound(false));
        }

        public async Task<IDataResult<ArticleListDto>> GetAllAsync()
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
            return new DataResult<ArticleListDto>(ResultStatus.Error, null, Messages.Article.NotFound(true));
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByCategoryAsync(int categoryId)
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
            var articles = await _unitOfWork.Articles.GetAllAsync(i => !i.IsDeleted, i => i.User, i => i.Category);
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
            var articles = await _unitOfWork.Articles.GetAllAsync(i => !i.IsDeleted && i.IsActive, i => i.User, i => i.Category);
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
            var result = _unitOfWork.Articles.AnyAsync(i => i.Id == articleId);

            if (result != null) //Bu Id ye sahip article var ise
            {
                var article = await _unitOfWork.Articles.GetAsync(i => i.Id == articleId);
              
                await _unitOfWork.Articles.DeleteAsync(article);//.ContinueWith(t=>_unitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
                await _unitOfWork.SaveAsync();

                return new Result(ResultStatus.Success, Messages.Article.Delete(article.Title));
            }

            return new Result(ResultStatus.Error, Messages.Article.NotFound(false));
        }

        public async Task<IResult> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedByName)
        {
            var oldArticle = await _unitOfWork.Articles.GetAsync(i=>i.Id==articleUpdateDto.Id);

            //Güncelleme esnasında bütün propertyler için giriş yapmıyoruz.Bundan dolayı eski makale üzerine yeni bilgileri yazmak ve değişmeyen bilgileri kaybetmemek için eski makalenin üzerine map ediyoruz
            var article = _mapper.Map<ArticleUpdateDto,Article>(articleUpdateDto,oldArticle);
            article.ModifiedByName = modifiedByName;

            await _unitOfWork.Articles.UpdateAsync(article);//.ContinueWith(t=>_unitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
            await _unitOfWork.SaveAsync();

            return new Result(ResultStatus.Success, Messages.Article.Update(article.Title));
        }

        public async Task<IDataResult<int>> CountAsync()
        {
            var articlesCount = await _unitOfWork.Articles.CountAsync();
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
            var articlesCount = await _unitOfWork.Articles.CountAsync(i => !i.IsDeleted);
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
            var result = await _unitOfWork.Articles.AnyAsync(i => i.Id == articleId);
            if (result)
            {
                var article = await _unitOfWork.Articles.GetAsync(c => c.Id == articleId);
                var articleUpdateDto = _mapper.Map<ArticleUpdateDto>(article);
                return new DataResult<ArticleUpdateDto>(ResultStatus.Success, articleUpdateDto);
            }
            return new DataResult<ArticleUpdateDto>(ResultStatus.Error, null, Messages.Article.NotFound(false));
        }
    }
}
