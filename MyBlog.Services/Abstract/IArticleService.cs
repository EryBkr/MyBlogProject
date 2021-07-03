using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos.ArticleDtos;
using MyBlog.Shared.Utilities.Results.Abtracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Services.Abstract
{
    public interface IArticleService
    {
        Task<IDataResult<ArticleDto>> GetAsync(int articleId);
        Task<IDataResult<ArticleListDto>> GetAllAsync();
        Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAsync();
        Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActiveAsync();
        Task<IDataResult<ArticleListDto>> GetAllByCategoryAsync(int categoryId);
        Task<IResult> AddAsync(ArticleAddDto articleAddDto, string createdByName);
        Task<IResult> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedByName);
        //IsDeleted True
        Task<IResult> DeleteAsync(int articleId, string modifiedByName);
        //Database Remove Yapılır
        Task<IResult> HardDeleteAsync(int articleId);

        Task<IDataResult<int>> CountAsync();

        Task<IDataResult<int>> CountByIsNonDeletedAsync();
    }
}
