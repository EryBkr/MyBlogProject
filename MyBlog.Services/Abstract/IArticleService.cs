using MyBlog.Entities.ComplexTypes;
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
        Task<IDataResult<ArticleDto>> GetByIdAsync(int articleId,bool includeCategory,bool includeComments,bool includeUser);

        Task<IDataResult<ArticleUpdateDto>> GetArticleUpdateDtoAsync(int articleId);

        Task<IDataResult<ArticleListDto>> GetAllAsync();
        Task<IDataResult<ArticleListDto>> GetAllAsyncV2(int? categoryId,int? userId,bool? isActive,bool? isDeleted,int currentPage,int pageSize,OrderByGeneral orderBy,bool isAscending, bool includeCategory, bool includeComments, bool includeUser);

        //Pagination yapısına uygun Metot
        Task<IDataResult<ArticleListDto>> GetAllByPagingAsync(int? categoryId,int currentPage=1,int pageSize=5,bool isAscending=false);


        Task<IDataResult<ArticleListDto>> GetAllByUserIdOnFilter(int userId,FilterBy filterBy,OrderBy orderBy,bool isAscending,int takeSize,int categoryId,DateTime startAt,DateTime endAt,int minViewCount,int maxViewCount,int minCommentsCount,int maxCommentsCount);

        //Search İşlemi
        Task<IDataResult<ArticleListDto>> SearchAsync(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false);

        //En Çok okunan Makaleler
        Task<IDataResult<ArticleListDto>> GetAllByViewCountAsync(bool isAscending,int? takeSize);

        Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAsync();

        //silinen makaleleri getirir
        Task<IDataResult<ArticleListDto>> GetAllByDeletedAsync();
        Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActiveAsync();
        Task<IDataResult<ArticleListDto>> GetAllByCategoryAsync(int categoryId);
        Task<IResult> AddAsync(ArticleAddDto articleAddDto, string createdByName,int userId);
        Task<IResult> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedByName);
        //IsDeleted True
        Task<IResult> DeleteAsync(int articleId, string modifiedByName);
        //IsDeleted False
        Task<IResult> UndoDeleteAsync(int articleId, string modifiedByName);
        //Database Remove Yapılır
        Task<IResult> HardDeleteAsync(int articleId);

        Task<IDataResult<int>> CountAsync();

        Task<IDataResult<int>> CountByIsNonDeletedAsync();

        //Makalenin Görüntülenme sayısı
        Task<IResult> IncreaseViewCountAsync(int articleId);
    }
}
