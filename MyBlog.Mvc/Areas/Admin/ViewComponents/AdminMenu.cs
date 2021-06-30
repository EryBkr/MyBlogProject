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
        public IViewComponentResult Invoke()
        {
            //Giriş Yapmış Kullanıcıyı elde ettik.Senkron bir işlem olması için .Result son ekini kullandık
            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            //Giriş Yapmış kullanıcının Rollerini elde ettik
            var roles = _userManager.GetRolesAsync(user).Result;

            var model = new UserWithRolesViewModels 
            {
                User=user,
                Roles=roles
            };

            return View(model);

        }
    }
}
