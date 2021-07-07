using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Entities.Concrete;
using MyBlog.Mvc.Areas.Admin.Models;
using MyBlog.Services.Abstract;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Areas.Admin.Controllers
{
    [Area(areaName:"Admin")]
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;

        public HomeController(ICategoryService categoryService, IArticleService articleService, ICommentService commentService, UserManager<User> userManager)
        {
            _categoryService = categoryService;
            _articleService = articleService;
            _commentService = commentService;
            _userManager = userManager;
        }

        [Authorize(Roles = "SuperAdmin,AdminArea.Home.Read")]
        public async Task<IActionResult> Index()
        {
            var categoriesCountResult = await _categoryService.CountByIsNonDeletedAsync();
            var articlesCountResult = await _articleService.CountByIsNonDeletedAsync();
            var commentsCountResult = await _commentService.CountByIsNonDeletedAsync();
            var usersCount = await _userManager.Users.CountAsync();
            var articleResult = await _articleService.GetAllAsync();

            if (categoriesCountResult.ResultStatus==ResultStatus.Success&&articleResult.ResultStatus==ResultStatus.Success&&commentsCountResult.ResultStatus==ResultStatus.Success&&usersCount>-1&&articleResult.ResultStatus==ResultStatus.Success)
            {
                return View(new DashboardViewModel 
                {
                    CategoriesCount=categoriesCountResult.Data,
                    ArticlesCount=articlesCountResult.Data,
                    CommentsCount=commentsCountResult.Data,
                    UsersCount=usersCount,
                    Articles=articleResult.Data
                });
            }

            return NotFound();
        }
    }
}
