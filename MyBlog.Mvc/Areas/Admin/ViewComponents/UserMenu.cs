using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Entities.Concrete;
using MyBlog.Mvc.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Areas.Admin.ViewComponents
{
    public class UserMenu:ViewComponent
    {
        private readonly UserManager<User> _userManager;

        public UserMenu(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //Giriş yapmış kullanıcıyı elde ediyoruz
            var user =await _userManager.GetUserAsync(HttpContext.User);

            if (user==null)
            {
                return Content("Kullanıcı Bulunamadı");
            }

            return View(
                new UserViewModel
            {
                User = user
            });
        }
    }
}
