using AutoMapper;
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
    public class CategoryManager : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Verilen CategoryAddDto yu veritabanına Category tipinde kaydeder
        /// </summary>
        /// <param name="categoryAddDto">Category tipini elde edeceğimiz parametredir</param>
        /// <param name="createdByName">Category i kimin eklediğini belirten parametredir</param>
        /// <returns>Asenkron bir operasyon ile Task olarak ekleme işleminin sonucunu DataResult tipinde geri döner</returns>
        public async Task<IDataResult<CategoryDto>> AddAsync(CategoryAddDto categoryAddDto, string createdByName)
        {
            var category = _mapper.Map<Category>(categoryAddDto);
            category.CreatedByName = createdByName;
            category.ModifiedByName = createdByName;

            var addedCategory = await _unitOfWork.Categories.AddAsync(category);//.ContinueWith(t=>_unitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
            await _unitOfWork.SaveAsync();

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
            var categoriesCount = await _unitOfWork.Categories.CountAsync();
            if (categoriesCount>-1)
            {
                return new DataResult<int>(ResultStatus.Success,categoriesCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Error, 0,"Beklenmeyen bir hata ile karşılaşıldı");
            }
         
        }

        public async Task<IDataResult<int>> CountByIsNonDeletedAsync()
        {
            var categoriesCount = await _unitOfWork.Categories.CountAsync(i => !i.IsDeleted);
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
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category != null)
            {
                category.IsDeleted = true;
                category.ModifiedByName = modifiedByName;
                category.ModifiedDate = DateTime.Now;
                var deletedCategory = await _unitOfWork.Categories.UpdateAsync(category);//.ContinueWith(t=>_unitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
                await _unitOfWork.SaveAsync();

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

        public async Task<IDataResult<CategoryDto>> GetAsync(int categoryId)
        {

            var category = await _unitOfWork.Categories.GetAsync(i => i.Id == categoryId, i => i.Articles);
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
            var categories = await _unitOfWork.Categories.GetAllAsync();
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
            var categories = await _unitOfWork.Categories.GetAllAsync(i => !i.IsDeleted);
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
            var categories = await _unitOfWork.Categories.GetAllAsync(i => !i.IsDeleted && i.IsActive);
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
            var result = await _unitOfWork.Categories.AnyAsync(i => i.Id == categoryId);
            if (result)
            {
                var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
                var categoryUpdateDto = _mapper.Map<CategoryUpdateDto>(category);
                return new DataResult<CategoryUpdateDto>(ResultStatus.Success, categoryUpdateDto);
            }
            return new DataResult<CategoryUpdateDto>(ResultStatus.Error, null, Messages.Category.NotFound(false));

        }

        public async Task<IResult> HardDeleteAsync(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category != null)
            {

                await _unitOfWork.Categories.DeleteAsync(category);//.ContinueWith(t=>_unitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
                await _unitOfWork.SaveAsync();

                return new Result(ResultStatus.Success, $"{category.Name} adlı kategori başarıyla silinmiştir (Hard)");
            }

            return new Result(ResultStatus.Error, Messages.Category.NotFound(false));
        }

        public async Task<IDataResult<CategoryDto>> UpdateAsync(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
        {
            //Güncelleme işlemine tabi tutmadığımız ama eski değerleri bize lazım olan (CreatedDate gibi)propertyleri kaybetmemek için dB den eski kategoriyi alıyoruz 
            var oldCategory = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryUpdateDto.Id);

            //Her iki değeri birleştirip bize category tipinde property leri tam olan bir entity verecektir
            var category = _mapper.Map<CategoryUpdateDto, Category>(categoryUpdateDto, oldCategory);
            category.ModifiedByName = modifiedByName;

            if (category != null)
            {
                var updatedCategory = await _unitOfWork.Categories.UpdateAsync(category);//.ContinueWith(t=>_unitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
                await _unitOfWork.SaveAsync();

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
    }
}
