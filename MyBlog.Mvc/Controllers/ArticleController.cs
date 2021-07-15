using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyBlog.Entities.ComplexTypes;
using MyBlog.Entities.Concrete;
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
       

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int articleId)
        {
            var articleResult =await _articleService.GetAsync(articleId);

            if (articleResult.ResultStatus == ResultStatus.Success)
            {
                //Okunan makaleyle alakalı kullanıcıya ait diğer makaleleri aldık
                var userArticles = await _articleService.GetAllByUserIdOnFilter(articleResult.Data.Article.UserId, FilterBy.Category, OrderBy.Date, isAscending: false, 10, articleResult.Data.Article.CategoryId, DateTime.Now, DateTime.Now, 0, 99999, 0, 99999);

                //Okunma sayısını arttırıyoruz
                await _articleService.IncreaseViewCountAsync(articleId);

                //Side Bar için olan modelimizi oluşturduk
                var articleDetailRightSideBarViewModel = new ArticleDetailRightSideBarViewModel 
                {
                    ArticleListDto=userArticles.Data,
                    Header="Kullanıcının Aynı Kategori Üzerindeki En Çok Okunan Makaleleri",
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
