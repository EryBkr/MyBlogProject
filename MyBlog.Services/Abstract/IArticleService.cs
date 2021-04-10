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
        Task<IDataResult<ArticleDto>> Get(int articleId);
        Task<IDataResult<ArticleListDto>> GetAll();
        Task<IDataResult<ArticleListDto>> GetAllByNonDeleted();
        Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActive();
        Task<IDataResult<ArticleListDto>> GetAllByCategory(int categoryId);
        Task<IResult> Add(ArticleAddDto articleAddDto, string createdByName);
        Task<IResult> Update(ArticleUpdateDto articleUpdateDto, string modifiedByName);
        //IsDeleted True
        Task<IResult> Delete(int articleId, string modifiedByName);
        //Database Remove Yapılır
        Task<IResult> HardDelete(int articleId);
    }
}
