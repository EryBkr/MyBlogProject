using AutoMapper;
using MyBlog.Data.Abstract;
using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos.CategoryDtos;
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
    public class CategoryManager : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IDataResult<CategoryDto>> Add(CategoryAddDto categoryAddDto, string createdByName)
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
                Message = $"{addedCategory.Name} adlı kategori başarıyla eklenmiştir"
            }
            , $"{addedCategory.Name} adlı kategori başarıyla eklenmiştir");
        }

        public async Task<IResult> Delete(int categoryId, string modifiedByName)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category != null)
            {
                category.IsDeleted = true;
                category.ModifiedByName = modifiedByName;
                category.ModifiedDate = DateTime.Now;
                await _unitOfWork.Categories.UpdateAsync(category);//.ContinueWith(t=>_unitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
                await _unitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, $"{category.Name} adli, kategori basariyla silinmistir");
            }

            return new Result(ResultStatus.Error, "Böyle bir kategori bulunamadı");
        }

        public async Task<IDataResult<CategoryDto>> Get(int categoryId)
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
                Category=null,
                ResultStatus=ResultStatus.Error,
                Message="Böyle bir kategori bulunamadı"
            }, "Böyle Bir Kategori Bulunamadı");
        }

        public async Task<IDataResult<CategoryListDto>> GetAll()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(null, i => i.Articles);
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
                Message = "Hiç bir kategori bulunamadı"
            }, "Hiç bir kategori bulunamadı");
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeleted()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(i => !i.IsDeleted, i => i.Articles);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, null, "Hiç bir kategori bulunamadı");
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActive()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(i => !i.IsDeleted && i.IsActive, i => i.Articles);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, null, "Hiç bir kategori bulunamadı");
        }

        public async Task<IResult> HardDelete(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category != null)
            {

                await _unitOfWork.Categories.DeleteAsync(category);//.ContinueWith(t=>_unitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
                await _unitOfWork.SaveAsync();

                return new Result(ResultStatus.Success, $"{category.Name} adlı kategori başarıyla silinmiştir (Hard)");
            }

            return new Result(ResultStatus.Error, "Böyle bir kategori bulunamadı");
        }

        public async Task<IDataResult<CategoryDto>> Update(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
        {
            var category = _mapper.Map<Category>(categoryUpdateDto);
            category.ModifiedByName = modifiedByName;

            if (category != null)
            {
                var updatedCategory = await _unitOfWork.Categories.UpdateAsync(category);//.ContinueWith(t=>_unitOfWork.SaveAsync());Hemen ardından save işlemi yapılması için kullandık
                await _unitOfWork.SaveAsync();

                return new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto
                {
                    Category = updatedCategory,
                    ResultStatus = ResultStatus.Success,
                    Message = $"{updatedCategory.Name} adlı kategori başarıyla eklenmiştir"
                }
           , $"{updatedCategory.Name} adlı kategori başarıyla güncellenmiştir");

            }



            return new DataResult<CategoryDto>(ResultStatus.Error, new CategoryDto
            {
                Category = null,
                ResultStatus = ResultStatus.Error,
                Message = "Seçilen kategoriye ait bilgi bulunamamıştır"
            }
            , "Seçilen kategoriye ait bilgi bulunamamıştır");
        }
    }
}
