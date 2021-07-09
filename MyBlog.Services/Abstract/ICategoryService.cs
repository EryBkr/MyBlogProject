using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos.CategoryDtos;
using MyBlog.Shared.Utilities.Results.Abtracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Services.Abstract
{
    public interface ICategoryService
    {
        Task<IDataResult<CategoryDto>> GetAsync(int categoryId);

        /// <summary>
        /// Verilen Id parametresine ait kategorinin CategoryUpdateDto temsilini geriye döner
        /// </summary>
        /// <param name="categoryId">0 dan büyük integer bir ID değeri</param>
        /// <returns>Asenkron bir operasyon ile Task olarak işlem sonucu DataResult tipinde geri döner</returns>
        Task<IDataResult<CategoryUpdateDto>> GetUpdateDtoAsync(int categoryId);
        Task<IDataResult<CategoryListDto>> GetAllAsync();
        Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAsync();
        //Silinmiş olanları getirecektir
        Task<IDataResult<CategoryListDto>> GetAllByDeletedAsync();
        Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActiveAsync();

        //Ekleme ve güncelleme işlemlerinde gerekli datayı geri dönmemiz gerekiyor.
        /// <summary>
        /// Verilen CategoryAddDto yu veritabanına Category tipinde kaydeder
        /// </summary>
        /// <param name="categoryAddDto">Category tipini elde edeceğimiz parametredir</param>
        /// <param name="createdByName">Category i kimin eklediğini belirten parametredir</param>
        /// <returns>Asenkron bir operasyon ile Task olarak işlem sonucu DataResult tipinde geri döner</returns>
        Task<IDataResult<CategoryDto>> AddAsync(CategoryAddDto categoryAddDto,string createdByName);


        Task<IDataResult<CategoryDto>> UpdateAsync(CategoryUpdateDto categoryUpdateDto,string modifiedByName);


        //IsDeleted True
        Task<IDataResult<CategoryDto>> DeleteAsync(int categoryId, string modifiedByName);
        //Silme işlemini geri alacaktır
        Task<IDataResult<CategoryDto>> UndoDeleteAsync(int categoryId, string modifiedByName);
        //Database Remove Yapılır
        Task<IResult> HardDeleteAsync(int categoryId);

        Task<IDataResult<int>> CountAsync();
        Task<IDataResult<int>> CountByIsNonDeletedAsync();
    }
}
