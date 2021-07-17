using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyBlog.Entities.Concrete;
using MyBlog.Mvc.Models;
using MyBlog.Services.Abstract;
using MyBlog.Shared.Utilities.Helpers.Abstract;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OptionsController : Controller
    {
        private readonly IToastNotification _toastNotification;
        private readonly WebSiteInfo _webSiteInfo;
        private readonly AboutUsPageInfo _aboutUsPageInfo;
        private readonly SmtpSettings _smtpSettings;
        private readonly ArticleRightSideBarWidgetOptions _articleOptions;

        //Appsettings dosyamız üzerinde gerekli section üzerinde değişiklik yapabilmek için ekledik
        private readonly IWritableOptions<AboutUsPageInfo> _aboutUsPageWritable;
        private readonly IWritableOptions<WebSiteInfo> _webSiteInfoWritable;
        private readonly IWritableOptions<SmtpSettings> _smtpSettingsWritable;
        private readonly IWritableOptions<ArticleRightSideBarWidgetOptions> _articleWritable;

        //Önerilen makale ayarını belirleyip appsettings e kaydedeceğiz bu işlem sırasında kategori seçimi de yapılacağı için onları listelememiz gerekmektedir.
        private readonly ICategoryService _categoryService;

        private readonly IMapper _mapper;


        public OptionsController(IOptionsSnapshot<AboutUsPageInfo> aboutUsPageInfo, IWritableOptions<AboutUsPageInfo> aboutUsPageWritable, IToastNotification toastNotification, IOptionsSnapshot<WebSiteInfo> webSiteInfo, IWritableOptions<WebSiteInfo> webSiteInfoWritable, IWritableOptions<SmtpSettings> smtpSettingsWritable, IOptionsSnapshot<SmtpSettings> smtpSettings, IOptionsSnapshot<ArticleRightSideBarWidgetOptions> articleOptions, IWritableOptions<ArticleRightSideBarWidgetOptions> articleWritable, ICategoryService categoryService, IMapper mapper)
        {
            //appsettings json daki section ı modele çevirdik
            _aboutUsPageInfo = aboutUsPageInfo.Value;
            _aboutUsPageWritable = aboutUsPageWritable;
            _toastNotification = toastNotification;
            _webSiteInfo = webSiteInfo.Value;
            _webSiteInfoWritable = webSiteInfoWritable;
            _smtpSettingsWritable = smtpSettingsWritable;
            _smtpSettings = smtpSettings.Value;
            _articleOptions = articleOptions.Value;
            _articleWritable = articleWritable;
            _categoryService = categoryService;
            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult About()
        {
            return View(_aboutUsPageInfo);
        }

        [HttpPost]
        public IActionResult About(AboutUsPageInfo model)
        {
            if (ModelState.IsValid)
            {
                //View den değerler appsettings json da gerekli değişiklikleri yapacaktır
                _aboutUsPageWritable.Update(x =>
                {
                    x.Header = model.Header;
                    x.Content = model.Content;
                    x.SeoAuthor = model.SeoAuthor;
                    x.SeoDescription = model.SeoDescription;
                    x.SeoTags = model.SeoTags;
                });

                _toastNotification.AddSuccessToastMessage("Hakkımızda sayfa içerikleri başarıyla güncellenmiştir", new ToastrOptions { Title = "Başarılı İşlem" });
                return View(model);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult GeneralSettings()
        {
            return View(_webSiteInfo);
        }

        [HttpPost]
        public IActionResult GeneralSettings(WebSiteInfo model)
        {
            if (ModelState.IsValid)
            {
                //View den değerler appsettings json da gerekli değişiklikleri yapacaktır
                _webSiteInfoWritable.Update(x =>
                {
                    x.Title = model.Title;
                    x.MenuTitle = model.MenuTitle;
                    x.SeoAuthor = model.SeoAuthor;
                    x.SeoDescription = model.SeoDescription;
                    x.SeoTags = model.SeoTags;
                });

                _toastNotification.AddSuccessToastMessage("Sitenizin genel ayarları başarıyla güncellenmiştir", new ToastrOptions { Title = "Başarılı İşlem" });
                return View(model);
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult EmailSettings()
        {
            return View(_smtpSettings);
        }

        [HttpPost]
        public IActionResult EmailSettings(SmtpSettings model)
        {
            if (ModelState.IsValid)
            {
                //View den değerler appsettings json da gerekli değişiklikleri yapacaktır
                _smtpSettingsWritable.Update(x =>
                {
                    x.Server = model.Server;
                    x.Port = model.Port;
                    x.SenderEmail = model.SenderEmail;
                    x.SenderName = model.SenderName;
                    x.UserName = model.UserName;
                });

                _toastNotification.AddSuccessToastMessage("Sitenizin e-mail ayarları başarıyla güncellenmiştir", new ToastrOptions { Title = "Başarılı İşlem" });
                return View(model);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ArticleRightSideBarWidgetSettings()
        {
            var categoryResult = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            var articleRightSideBarViewModel = _mapper.Map<ArticleRightSideBarWidgetOptionsViewModel>(_articleOptions);
            articleRightSideBarViewModel.Categories = categoryResult.Data.Categories;

            return View(articleRightSideBarViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ArticleRightSideBarWidgetSettings(ArticleRightSideBarWidgetOptionsViewModel model)
        {
            //Post action na gelindiği zaman da kategori selectbox ının dolu olması için tekrardan kategorileri çağırdık
            var categoryResult = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            model.Categories = categoryResult.Data.Categories;


            if (ModelState.IsValid)
            {
                //View den değerler appsettings json da gerekli değişiklikleri yapacaktır
                _articleWritable.Update(x =>
                {
                    x.Header = model.Header;
                    x.TakeSize = model.TakeSize;
                    x.CategoryId = model.CategoryId;
                    x.FilterBy = model.FilterBy;
                    x.OrderBy = model.OrderBy;
                    x.IsAscending = model.IsAscending;
                    x.StartAt = model.StartAt;
                    x.EndAt = model.EndAt;
                    x.MaxViewCount = model.MaxViewCount;
                    x.MinViewCount = model.MinViewCount;
                    x.MaxCommentCount = model.MaxCommentCount;
                    x.MinCommentCount = model.MinCommentCount;
                });

                _toastNotification.AddSuccessToastMessage("Makale sayfasının widget ayarları başarıyla güncellenmiştir", new ToastrOptions { Title = "Başarılı İşlem" });
                return View(model);
            }

            return View(model);
        }

    }
}
