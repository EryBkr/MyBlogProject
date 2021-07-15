using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos.CategoryDtos;
using MyBlog.Mvc.Areas.Admin.Models;
using MyBlog.Mvc.Helpers.Abstract;
using MyBlog.Mvc.Utilities;
using MyBlog.Services.Abstract;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using NToastNotify;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Areas.Admin.Controllers
{
    [Area(areaName: "Admin")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService, IMapper mapper, IImageHelper imageHelper, UserManager<User> userManager,IToastNotification toastNotification) : base(userManager, mapper, imageHelper, toastNotification)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles = "SuperAdmin,Category.Read")]
        public async Task<IActionResult> Index()
        {
            //IsDeleted==false olan kategorileri alacağız
            var result = await _categoryService.GetAllByNonDeletedAsync();
            //İşlemin hatalı mı , hatalıysa mesajını data içerisinde barındırdığımız için ekstra bir kontrole gerek duymuyoruz
            return View(result.Data);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Category.Create")]
        public IActionResult Add()
        {
            return PartialView("_CategoryAddPartial");
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,CoCategorymment.Create")]
        public async Task<IActionResult> Add(CategoryAddDto categoryAddDto)
        {
            if (ModelState.IsValid)
            {
                //LoggedInUser Field ı base class aracılığıyla bize gelmektedir
                var result = await _categoryService.AddAsync(categoryAddDto, LoggedInUser.UserName);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    var categoryAjaxModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel
                    {
                        CategoryDto = result.Data,
                        CategoryAddPartial = await this.RenderViewToStringAsync("_CategoryAddPartial", categoryAddDto)
                    });

                    return Json(categoryAjaxModel);
                }
            }

            var categoryAjaxInvalidrModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel
            {
                CategoryAddPartial = await this.RenderViewToStringAsync("_CategoryAddPartial", categoryAddDto)
            });

            return Json(categoryAjaxInvalidrModel);
        }

        [Authorize(Roles = "SuperAdmin,Category.Read")]
        public async Task<JsonResult> GetAllCategories()
        {
            //IsDeleted==false olan kategorileri alacağız
            var result = await _categoryService.GetAllByNonDeletedAsync();
            var categories = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });

            return Json(categories);

        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Category.Delete")]
        public async Task<JsonResult> Delete(int categoryId)
        {
            var result = await _categoryService.DeleteAsync(categoryId, LoggedInUser.UserName);
            var deletedCategory = JsonSerializer.Serialize(result.Data);

            return Json(deletedCategory);
        }


        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Category.Update")]
        public async Task<IActionResult> Update(int categoryId)
        {
            var result = await _categoryService.GetUpdateDtoAsync(categoryId: categoryId);
            if (result.ResultStatus == ResultStatus.Success)
            {
                return PartialView("_CategoryUpdatePartial", result.Data);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Category.Update")]
        public async Task<IActionResult> Update(CategoryUpdateDto categoryUpdateDto)
        {
            if (ModelState.IsValid)
            { 
                //LoggedInUser Field ı base class aracılığıyla bize gelmektedir
                var result = await _categoryService.UpdateAsync(categoryUpdateDto, LoggedInUser.UserName);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    var categoryAjaxModel = JsonSerializer.Serialize(new CategoryUpdateAjaxViewModel
                    {
                        CategoryDto = result.Data,
                        CategoryUpdatePartial = await this.RenderViewToStringAsync("_CategoryUpdatePartial", categoryUpdateDto)
                    });

                    return Json(categoryAjaxModel);
                }
            }

            var categoryAjaxInvalidrModel = JsonSerializer.Serialize(new CategoryUpdateAjaxViewModel
            {
                CategoryUpdatePartial = await this.RenderViewToStringAsync("_CategoryUpdatePartial", categoryUpdateDto)
            });

            return Json(categoryAjaxInvalidrModel);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Category.Read")]
        public async Task<IActionResult> DeletedCategories()
        {
            //IsDeleted==true olan kategorileri alacağız
            var result = await _categoryService.GetAllByDeletedAsync();
            //İşlemin hatalı mı , hatalıysa mesajını data içerisinde barındırdığımız için ekstra bir kontrole gerek duymuyoruz
            return View(result.Data);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Category.Read")]
        public async Task<JsonResult> GetAllDeletedCategories()
        {
            //IsDeleted==true olan kategorileri alacağız
            var result = await _categoryService.GetAllByDeletedAsync();
            var categories = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });

            return Json(categories);

        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Category.Delete")]
        public async Task<JsonResult> UndoDelete(int categoryId)
        {
            var result = await _categoryService.UndoDeleteAsync(categoryId, LoggedInUser.UserName);
            var undoDeletedCategory = JsonSerializer.Serialize(result.Data);

            return Json(undoDeletedCategory);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Category.Delete")]
        public async Task<JsonResult> HardDelete(int categoryId)
        {
            //Veritabanından fiziksel olarak siliyoruz
            var result = await _categoryService.HardDeleteAsync(categoryId);
            var deletedCategory = JsonSerializer.Serialize(result);

            return Json(deletedCategory);
        }
    }
}
