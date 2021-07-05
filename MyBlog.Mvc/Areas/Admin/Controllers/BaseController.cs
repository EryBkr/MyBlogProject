using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Entities.Concrete;
using MyBlog.Mvc.Helpers.Abstract;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Areas.Admin.Controllers
{
    //SÜrekli yapılacak olan bir çok işlemi ve yine sürekli kullanılacak olan bir çok yapıyı BaseController da toplamış olduk
    public class BaseController : Controller
    {
        public BaseController(UserManager<User> userManager, IMapper mapper, IImageHelper ımageHelper, IToastNotification toastNotification)
        {
            UserManager = userManager;
            Mapper = mapper;
            ImageHelper = ımageHelper;
            ToastNotification = toastNotification;
        }

        protected UserManager<User> UserManager { get; }
        protected IMapper Mapper { get; }
        protected IImageHelper ImageHelper { get; }
        protected User LoggedInUser => UserManager.GetUserAsync(HttpContext.User).Result;

        //Toastr mesajlarını MVC tarafında oluşturabilmek için eklediğimiz bir kütüphane
        protected IToastNotification ToastNotification { get; }
    }
}
