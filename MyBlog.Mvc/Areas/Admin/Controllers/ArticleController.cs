using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Entities.ComplexTypes;
using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos.ArticleDtos;
using MyBlog.Mvc.Areas.Admin.Models;
using MyBlog.Mvc.Helpers.Abstract;
using MyBlog.Services.Abstract;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using NToastNotify;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;

        //base aracılığıyla bu yapıları Kalıtım aldığımız class a gönderdik
        public ArticleController(IArticleService articleService, IMapper mapper, ICategoryService categoryService, IImageHelper imageHelper, UserManager<User> userManager, IToastNotification toastNotification) : base(userManager, mapper, imageHelper, toastNotification)
        {
            _articleService = articleService;
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Article.Read")]
        public async Task<IActionResult> Index()
        {
            var result = await _articleService.GetAllByNonDeletedAsync();

            if (result.ResultStatus == ResultStatus.Success) return View(result.Data);
            return NotFound();
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Article.Create")]
        public async Task<IActionResult> Add()
        {
            var result = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            if (result.ResultStatus == ResultStatus.Success)
            {
                return View(new ArticleAddViewModel
                {
                    Categories = result.Data.Categories //Ekleme kısmı için kategorileri gönderiyoruz
                });
            }
            return NotFound();
        }


        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Article.Create")]
        public async Task<IActionResult> Add(ArticleAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Burada ki Mappper ve ImageHelper Base Class tan gelen field lardır
                var articleAddDto = Mapper.Map<ArticleAddDto>(model);
                var imageResult = await ImageHelper.UploadImage(model.Title, PictureType.Post, model.ThumbnailFile);

                articleAddDto.Thumbnail = imageResult.Data.FullName;

                //LoggedInUser Field ı base class aracılığıyla bize gelmektedir
                var result = await _articleService.AddAsync(articleAddDto, LoggedInUser.UserName, LoggedInUser.Id);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    //Toastr mesajımızı oluşturduk
                    ToastNotification.AddSuccessToastMessage($"{result.Message}");
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }

            //Bir hata durumunda Kategorileri sayfaya göndermezsek hata fırlatacaktır
            var categories = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            model.Categories = categories.Data.Categories;
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Article.Update")]
        public async Task<IActionResult> Update(int articleId)
        {
            var articleResult = await _articleService.GetArticleUpdateDtoAsync(articleId);
            var categoriesResult = await _categoryService.GetAllByNonDeletedAndActiveAsync();

            if (articleResult.ResultStatus == ResultStatus.Success && categoriesResult.ResultStatus == ResultStatus.Success)
            {
                var articleUpdateViewModel = Mapper.Map<ArticleUpdateViewModel>(articleResult.Data);
                articleUpdateViewModel.Categories = categoriesResult.Data.Categories;

                return View(articleUpdateViewModel);
            }
            else
                return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Article.Update")]
        public async Task<IActionResult> Update(ArticleUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Yeni Resim Yüklendi mi
                bool isNewThumbnailUploaded = false;

                var oldThumbnail = model.Thumbnail;

                //Yeni Bir Resim Yüklendi mi
                if (model.ThumbnailFile != null)
                {
                    //Yeni Resmi Yüklüyoruz
                    var uploadedImageResult = await ImageHelper.UploadImage(model.Title, PictureType.Post, model.ThumbnailFile);

                    //Resim yükleme işlemi başarı ile yeni resmi modele veriyoruz değil ise default resmimizi tanımlıyoruz
                    model.Thumbnail = uploadedImageResult.ResultStatus == ResultStatus.Success ? uploadedImageResult.Data.FullName : "postImages/defaultThumbnail.jpg";

                    //Yeni resim ekleme işleminin başarılı olup olmadığını sorguluyoruz, default resmimizi sistemden silmek istemeyiz
                    if (oldThumbnail != "postImages/defaultThumbnail.jpg")
                        isNewThumbnailUploaded = true;

                }

                var articleUpdateDto = Mapper.Map<ArticleUpdateDto>(model);

                    var result = await _articleService.UpdateAsync(articleUpdateDto,LoggedInUser.UserName);

                    if (result.ResultStatus==ResultStatus.Success)
                    {
                        //Resmi silip silemeyeceğimize karar veriyoruz
                        if (isNewThumbnailUploaded)
                            ImageHelper.Delete(oldThumbnail);

                    //Toastr mesajımızı oluşturduk
                    ToastNotification.AddSuccessToastMessage($"{result.Message}");
                    return RedirectToAction("Index");
                    }
                    else
                    {

                    //Toastr mesajımızı oluşturduk
                    ToastNotification.AddErrorToastMessage($"{result.Message}");
                    ModelState.AddModelError("",result.Message);
                    }
                
            }

            //Tekrar Update sayfasına dönmemiz gerekiyorsa Kategori Listesi boş gelmemesi için dolduruyoruz
            var categories = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            model.Categories = categories.Data.Categories;

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Article.Delete")]
        public async Task<JsonResult> Delete(int articleId)
        {
            var result = await _articleService.DeleteAsync(articleId, LoggedInUser.UserName);
            var articleResult = JsonSerializer.Serialize(result);

            return Json(articleResult);
        }


        //Article Refresh Button
        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Article.Read")]
        public async Task<JsonResult> GetAllArticles()
        {
            var article = await _articleService.GetAllByNonDeletedAndActiveAsync();

            var articleResult = JsonSerializer.Serialize(article, new JsonSerializerOptions 
            {
                ReferenceHandler=ReferenceHandler.Preserve //Data ilişkilerinden kaynaklı hata almamak adına ekledik
            });

            return Json(articleResult);

        }

        [Authorize(Roles = "SuperAdmin,Article.Read")]
        [HttpGet]
        public async Task<IActionResult> DeletedArticles()
        {
            var result = await _articleService.GetAllByDeletedAsync();
            return View(result.Data);

        }

        [Authorize(Roles = "SuperAdmin,Article.Read")]
        [HttpGet]
        public async Task<JsonResult> GetAllDeletedArticles()
        {
            var result = await _articleService.GetAllByDeletedAsync();
            var articles = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(articles);
        }

        [Authorize(Roles = "SuperAdmin,Article.Update")]
        [HttpPost]
        public async Task<JsonResult> UndoDelete(int articleId)
        {
            var result = await _articleService.UndoDeleteAsync(articleId, LoggedInUser.UserName);
            var undoDeleteArticleResult = JsonSerializer.Serialize(result);
            return Json(undoDeleteArticleResult);
        }

        [Authorize(Roles = "SuperAdmin,Article.Delete")]
        [HttpPost]
        public async Task<JsonResult> HardDelete(int articleId)
        {
            var result = await _articleService.HardDeleteAsync(articleId);
            var hardDeletedArticleResult = JsonSerializer.Serialize(result);
            return Json(hardDeletedArticleResult);
        }

        //CHART.JS
        [Authorize(Roles = "SuperAdmin,Article.Read")]
        [HttpGet]
        public async Task<JsonResult> GetAllByViewCount(bool isAscending,int takeSize)
        {
            var result = await _articleService.GetAllByViewCountAsync(isAscending,takeSize);
            var articles = JsonSerializer.Serialize(result.Data.Articles, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(articles);
        }
    }
}
