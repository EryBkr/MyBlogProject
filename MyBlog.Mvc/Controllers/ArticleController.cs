using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyBlog.Entities.ComplexTypes;
using MyBlog.Entities.Concrete;
using MyBlog.Mvc.Attributes;
using MyBlog.Mvc.Models;
using MyBlog.Services.Abstract;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        //Detay sayfamızda ki makaleyle alakalı önerilen makalelerin hangi şart ve sırayla geleceğini belirleyen parametreleri appsettings ten almamızı sağlayan yapıdır
        private readonly ArticleRightSideBarWidgetOptions _articleOptions;


        public ArticleController(IArticleService articleService, IOptionsSnapshot<ArticleRightSideBarWidgetOptions> articleOptions)
        {
            _articleService = articleService;
            _articleOptions = articleOptions.Value;
        }

        [HttpGet]
        [ViewCountFilterAttribute] //Cookie kontrollü olarak makale okunma sayısı arttırılıyor
        public async Task<IActionResult> Detail(int articleId)
        {
            var articleResult =await _articleService.GetAsync(articleId);

            if (articleResult.ResultStatus == ResultStatus.Success)
            {
                //Okunan makaleyle alakalı kullanıcıya ait diğer makaleleri aldık.Alma işleminde appsettings json dosyamızda ki parametrelerden faydalandık
                var userArticles = await _articleService.GetAllByUserIdOnFilter(articleResult.Data.Article.UserId, _articleOptions.FilterBy, _articleOptions.OrderBy, _articleOptions.IsAscending, _articleOptions.TakeSize, _articleOptions.CategoryId, _articleOptions.StartAt, _articleOptions.EndAt, _articleOptions.MinViewCount, _articleOptions.MaxViewCount,_articleOptions.MinCommentCount, _articleOptions.MaxCommentCount);


                //Side Bar için olan modelimizi oluşturduk
                var articleDetailRightSideBarViewModel = new ArticleDetailRightSideBarViewModel 
                {
                    ArticleListDto=userArticles.Data,
                    Header=_articleOptions.Header,
                    User=articleResult.Data.Article.User
                };


                return View(new ArticleDetailViewModel 
                {
                    ArticleDto=articleResult.Data,
                    ArticleDetailRightSideBarViewModel= articleDetailRightSideBarViewModel
                });
            }
                
            else
                return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Search(string keyword,int currentPage=1,int pageSize=5,bool isAscending=false)
        {
            var searchResult = await _articleService.SearchAsync(keyword, currentPage, pageSize, isAscending);

            if (searchResult.ResultStatus==ResultStatus.Success)
            {
                return View(new ArticleSearchViewModel 
                {
                    ArticleListDto=searchResult.Data,
                    Keyword=keyword //Arama işlemi sonucunda sayfalar arasında dolaşmamız gerekebilir.O yüzden keyword ü modele veriyoruz ki o aramaya uygun olarak sayfalarda dolaşalım
                });
            }
            return NotFound();
        }

      
    }
}
