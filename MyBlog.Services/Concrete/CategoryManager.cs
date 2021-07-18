using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Abstract;
using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos.CategoryDtos;
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
    public class CategoryManager : ManagerBase, ICategoryService
    {

        //Dolu halleri Base Class ımızdan gelecektir
        public CategoryManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        /// <summary>
        /// Verilen CategoryAddDto yu veritabanına Category tipinde kaydeder
        /// </summary>
        /// <param name="categoryAddDto">Category tipini elde edeceğimiz parametredir</param>
        /// <param name="createdByName">Category i kimin eklediğini belirten parametredir</param>
        /// <returns>Asenkron bir operasyon ile Task olarak ekleme işleminin sonucunu DataResult tipinde geri döner</returns>
        public async Task<IDataResult<CategoryDto>> AddAsync(CategoryAddDto categoryAddDto, string createdByName)
        {
            var category = Mapper.Map<Category>(categoryAddDto);
            category.CreatedByName = createdByName;
            category.ModifiedByName = createdByName;

            var addedCategory = await UnitOfWork.Categories.AddAsync(category);//.ContinueWith(t=>UnitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
            await UnitOfWork.SaveAsync();

            return new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto
            {
                Category = addedCategory,
                ResultStatus = ResultStatus.Success,
                Message = Messages.Category.Add(addedCategory.Name)
            }
            , Messages.Category.Add(addedCategory.Name));
        }

        public async Task<IDataResult<int>> CountAsync()
        {
            var categoriesCount = await UnitOfWork.Categories.CountAsync();
            if (categoriesCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, categoriesCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Error, 0, "Beklenmeyen bir hata ile karşılaşıldı");
            }

        }

        public async Task<IDataResult<int>> CountByIsNonDeletedAsync()
        {
            var categoriesCount = await UnitOfWork.Categories.CountAsync(i => !i.IsDeleted);
            if (categoriesCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, categoriesCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Error, 0, "Beklenmeyen bir hata ile karşılaşıldı");
            }
        }

        public async Task<IDataResult<CategoryDto>> DeleteAsync(int categoryId, string modifiedByName)
        {
            var category = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category != null)
            {
                category.IsDeleted = true;
                category.IsActive = false;
                category.ModifiedByName = modifiedByName;
                category.ModifiedDate = DateTime.Now;
                var deletedCategory = await UnitOfWork.Categories.UpdateAsync(category);//.ContinueWith(t=>UnitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
                await UnitOfWork.SaveAsync();

                return new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto
                {
                    Category = deletedCategory,
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Category.Delete(deletedCategory.Name)
                }
       , Messages.Category.Delete(deletedCategory.Name));
            }

            return new DataResult<CategoryDto>(ResultStatus.Error, new CategoryDto
            {
                Category = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Category.DeleteError()
            }
       , Messages.Category.DeleteError());
        }

        /// <summary>
        /// AsQueryable Kullanımına örnek olarak ekledim
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<IDataResult<CategoryDto>> GetAsync(int categoryId)
        {
            
            //var query = UnitOfWork.Categories.GetAsQueryable();
            //query.Include(i => i.Articles).ThenInclude(a => a.Comments);

            var category = await UnitOfWork.Categories.GetAsync(i => i.Id == categoryId, i => i.Articles);
            if (category != null)
            {
                return new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto
                {
                    Category = category,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<CategoryDto>(ResultStatus.Error, new CategoryDto
            {
                Category = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Category.NotFound(false)
            }, Messages.Category.NotFound(false));
        }

        public async Task<IDataResult<CategoryListDto>> GetAllAsync()
        {
            var categories = await UnitOfWork.Categories.GetAllAsync();
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, new CategoryListDto
            {
                Categories = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Category.NotFound(true)
            }, Messages.Category.NotFound(true));
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAsync()
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(i => !i.IsDeleted);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, new CategoryListDto
            {
                Categories = null,
                ResultStatus = ResultStatus.Error
            }, Messages.Category.NotFound(true));
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActiveAsync()
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(i => !i.IsDeleted && i.IsActive);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, new CategoryListDto
            {
                Categories = null,
                ResultStatus = ResultStatus.Error
            }, Messages.Category.NotFound(true));
        }

        /// <summary>
        /// Verilen Id parametresine ait kategorinin CategoryUpdateDto temsilini geriye döner
        /// </summary>
        /// <param name="categoryId">0 dan büyük integer bir ID değeri</param>
        /// <returns>Asenkron bir operasyon ile Task olarak işlem sonucu DataResult tipinde geri döner</returns>
        public async Task<IDataResult<CategoryUpdateDto>> GetUpdateDtoAsync(int categoryId)
        {
            //Verilen  Id ye ait kategori var mı?
            var result = await UnitOfWork.Categories.AnyAsync(i => i.Id == categoryId);
            if (result)
            {
                var category = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryId);
                var categoryUpdateDto = Mapper.Map<CategoryUpdateDto>(category);
                return new DataResult<CategoryUpdateDto>(ResultStatus.Success, categoryUpdateDto);
            }
            return new DataResult<CategoryUpdateDto>(ResultStatus.Error, null, Messages.Category.NotFound(false));

        }

        public async Task<IResult> HardDeleteAsync(int categoryId)
        {
            var category = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category != null)
            {

                await UnitOfWork.Categories.DeleteAsync(category);//.ContinueWith(t=>UnitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
                await UnitOfWork.SaveAsync();

                return new Result(ResultStatus.Success, $"{category.Name} adlı kategori başarıyla silinmiştir (Hard)");
            }

            return new Result(ResultStatus.Error, Messages.Category.NotFound(false));
        }

        public async Task<IDataResult<CategoryDto>> UpdateAsync(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
        {
            //Güncelleme işlemine tabi tutmadığımız ama eski değerleri bize lazım olan (CreatedDate gibi)propertyleri kaybetmemek için dB den eski kategoriyi alıyoruz 
            var oldCategory = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryUpdateDto.Id);

            //Her iki değeri birleştirip bize category tipinde property leri tam olan bir entity verecektir
            var category = Mapper.Map<CategoryUpdateDto, Category>(categoryUpdateDto, oldCategory);
            category.ModifiedByName = modifiedByName;

            if (category != null)
            {
                var updatedCategory = await UnitOfWork.Categories.UpdateAsync(category);//.ContinueWith(t=>UnitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
                await UnitOfWork.SaveAsync();

                return new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto
                {
                    Category = updatedCategory,
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Category.Update(updatedCategory.Name)
                }
           , Messages.Category.Update(updatedCategory.Name));

            }

            return new DataResult<CategoryDto>(ResultStatus.Error, new CategoryDto
            {
                Category = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Category.NotFound(false)
            }
            , Messages.Category.NotFound(false));
        }

        //Silinmiş kategorileri listeleyecektir
        public async Task<IDataResult<CategoryListDto>> GetAllByDeletedAsync()
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(i => i.IsDeleted);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, new CategoryListDto
            {
                Categories = null,
                ResultStatus = ResultStatus.Error
            }, Messages.Category.NotFound(true));
        }

        //Silme işlemini tersine çeviriyoruz
        public async Task<IDataResult<CategoryDto>> UndoDeleteAsync(int categoryId, string modifiedByName)
        {
            var category = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category != null)
            {
                category.IsDeleted = false;
                category.IsActive = true;
                category.ModifiedByName = modifiedByName;
                category.ModifiedDate = DateTime.Now;
                var nonDeletedCategory = await UnitOfWork.Categories.UpdateAsync(category);//.ContinueWith(t=>UnitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
                await UnitOfWork.SaveAsync();

                return new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto
                {
                    Category = nonDeletedCategory,
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Category.NonDelete(nonDeletedCategory.Name)
                }
       , Messages.Category.Delete(nonDeletedCategory.Name));
            }

            return new DataResult<CategoryDto>(ResultStatus.Error, new CategoryDto
            {
                Category = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Category.NonDeleteError()
            }
       , Messages.Category.NonDeleteError());
        }
    }
}
