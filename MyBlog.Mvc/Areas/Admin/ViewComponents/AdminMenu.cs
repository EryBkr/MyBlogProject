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
    public class AdminMenu:ViewComponent
    {
        private readonly UserManager<User> _userManager;

        public AdminMenu(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        //Side bar ı yetkiye göre dinamik olarak oluşturacak bir ViewModel oluşturduk
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //Giriş Yapmış Kullanıcıyı elde ettik.Senkron bir işlem olması için .Result son ekini kullandık
            var user =await _userManager.GetUserAsync(HttpContext.User);

            //Giriş Yapmış kullanıcının Rollerini elde ettik
            var roles = _userManager.GetRolesAsync(user).Result;

            if (user == null)
            {
                return Content("Kullanıcı Bulunamadı");
            }

            if (roles == null)
            {
                return Content("Roller Bulunamadı");
            }

            var model = new UserWithRolesViewModels 
            {
                User=user,
                Roles=roles
            };

            return View(model);

        }
    }
}
