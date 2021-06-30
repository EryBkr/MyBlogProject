using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos.UserDtos;
using MyBlog.Mvc.Areas.Admin.Models;
using MyBlog.Mvc.Utilities;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env; //Dosya yoluna dinamik ulaşabilmek için ekledik 
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;



        public UserController(UserManager<User> userManager, IMapper mapper, IWebHostEnvironment env, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _env = env;
            _signInManager = signInManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(new UserListDto
            {
                Users = users,
                ResultStatus = ResultStatus.Success
            });
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("UserLogin");
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null) //Kullanıcı mevcut mu
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                    if (result.Succeeded) //Giriş Başarılı mı?
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "E-Posta Adresiniz veya Şifreniz yanlıştır");
                        return View("UserLogin");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "E-Posta Adresiniz veya Şifreniz yanlıştır");
                    return View("UserLogin");
                }
            }
            else
            {
                return View("UserLogin");
            }

        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { @area = "" });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            var userListDto = JsonSerializer.Serialize(new UserListDto
            {
                Users = users,
                ResultStatus = ResultStatus.Success
            }, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });
            //İç içe oluşan datalardan kaynaklı Options tanımladık

            return Json(userListDto);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return PartialView("_UserAddPartial");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {
            if (ModelState.IsValid)
            {
                userAddDto.Picture = await ImageUpload(userAddDto.UserName, userAddDto.PictureFile); //Resim kaydedilip ismi dto ya veriliyor
                var user = _mapper.Map<User>(userAddDto);

                var result = await _userManager.CreateAsync(user, userAddDto.Password);
                if (result.Succeeded)
                {
                    var userAddAjaxModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserDto = new UserDto
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"{user.UserName} adlı kullanıcı başarıyla eklenmiştir",
                            User = user
                        },
                        UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
                    });
                    return Json(userAddAjaxModel);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    var userAddAjaxModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserAddDto = userAddDto,
                        UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
                    });
                    return Json(userAddAjaxModel);
                }
            }
            var userAddAjaxModelStateErrorsModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
            {
                UserAddDto = userAddDto,
                UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
            });
            return Json(userAddAjaxModelStateErrorsModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> Delete(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                var deletedUser = JsonSerializer.Serialize(new UserDto
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"{user.UserName} adlı kullanıcı başarıyla silinmiştir",
                    User = user
                });
                return Json(deletedUser);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages += $"{error.Description}\n";
                }
                var deletedUserErrorModel = JsonSerializer.Serialize(new UserDto
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{user.UserName} adlı kullanıcını silinememiştir. \n {errorMessages}"
                });
                return Json(deletedUserErrorModel);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<PartialViewResult> Update(int userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(i => i.Id == userId);
            var userUpdateDto = _mapper.Map<UserUpdateDto>(user);
            return PartialView("_UserUpdatePartial", userUpdateDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UserUpdateDto model)
        {
            if (ModelState.IsValid)
            {
                bool isNewPictureUploaded = false; //Resim ekleme işlemi başarılı mı ? Diğer türlü mevcut resmi kullanıcı resmi kaydedemeden silmiş oluruz
                var oldUser = await _userManager.FindByIdAsync(model.Id.ToString());
                var oldUserPicture = oldUser.Picture;//Silinecek Resmi aldık
                if (model.PictureFile != null) //Kullanıcı Yeni Resim Eklemiş
                {
                    model.Picture = await ImageUpload(model.UserName, model.PictureFile);
                    isNewPictureUploaded = true;
                }
                var updatedUser = _mapper.Map<UserUpdateDto, User>(model, oldUser);
                var result = await _userManager.UpdateAsync(updatedUser);

                if (result.Succeeded) //Güncelleme işlemi başarılı mı 
                {
                    if (isNewPictureUploaded) //Yeni Resim ekleme işlemi başarılı mı?
                    {
                        ImageDelete(oldUserPicture);
                    }
                    var userUpdateViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserDto = new UserDto
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"{updatedUser.UserName} adlı kullanıcı başarıyla güncellenmiştir",
                            User = updatedUser
                        },
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", model)
                    });
                    return Json(userUpdateViewModel);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    var userUpdateErrorViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserUpdateDto = model,
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", model)
                    });
                    return Json(userUpdateErrorViewModel);
                }
            }
            else
            {
                var userUpdateModelStateErrorViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                {
                    UserUpdateDto = model,
                    UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", model)
                });
                return Json(userUpdateModelStateErrorViewModel);
            }
        }

        [Authorize(Roles = "Admin,Editor")]
        public async Task<string> ImageUpload(string userName, IFormFile pictureFile)
        {
            string wwwroot = _env.WebRootPath; //Dosya yolunu dinamik olarak aldık

            string fileExtension = Path.GetExtension(pictureFile.FileName); //resmin uzantısını aldık

            var dateTime = DateTime.Now; //O an ki zamanı isimlendirme için aldık

            string fileName = $"{userName}_{dateTime.FullDateAndTimeStringUnderscore()}.{fileExtension}"; //UserName_DateTimeNow.jpg gibi

            var path = Path.Combine($"{wwwroot}/img", fileName);

            //Uzantısı ile birlikte dosya ve o dosyaya uygulanacak işlemi belirtiyoruz
            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await pictureFile.CopyToAsync(stream);
            }
            return fileName; //Oluşan resim adını aldık
        }

        [HttpGet]
        [Authorize]
        public async Task<ViewResult> ChangeDetails()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var updateDto = _mapper.Map<UserUpdateDto>(user);
            return View(updateDto);
        }

        [HttpPost]
        [Authorize]
        public async Task<ViewResult> ChangeDetails(UserUpdateDto model)
        {
            if (ModelState.IsValid)
            {
                bool isNewPictureUploaded = false; //Resim ekleme işlemi başarılı mı ? Diğer türlü mevcut resmi kullanıcı resmi kaydedemeden silmiş oluruz
                var oldUser = await _userManager.GetUserAsync(HttpContext.User);
                var oldUserPicture = oldUser.Picture;//Silinecek Resmi aldık
                if (model.PictureFile != null) //Kullanıcı Yeni Resim Eklemiş
                {
                    model.Picture = await ImageUpload(model.UserName, model.PictureFile);

                    //Kullanıcının mevcut resmi,sistemin default olarak atadığı resim ise silmiyoruz.Diğer default resim kullanıcıları resimsiz kalır diğer türlü
                    if (oldUserPicture != "defaultUser.png")
                        isNewPictureUploaded = true;
                }

                var updatedUser = _mapper.Map<UserUpdateDto, User>(model, oldUser);
                var result = await _userManager.UpdateAsync(updatedUser);

                if (result.Succeeded) //Güncelleme işlemi başarılı mı 
                {
                    if (isNewPictureUploaded) //Yeni Resim ekleme işlemi başarılı mı?
                        ImageDelete(oldUserPicture);


                    TempData.Add("SuccessMessage", $"{updatedUser.UserName} adlı kullanıcı başarıyla güncellenmiştir");

                    return View(model);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }


        [Authorize]
        [HttpGet]
        public ViewResult PasswordChange()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PasswordChange(UserPasswordChangeDto model)
        {
            if (ModelState.IsValid)
            {
                //Login olmuş user ı aldık
                var user = await _userManager.GetUserAsync(HttpContext.User);

                //Kullanıcı şifresini hatırlıyor mu kontrol ediyoruz
                var isVerified = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);

                //Şifresini Hatırlıyor ise
                if (isVerified)
                {
                    //şifre değişikliği işlemi yapılıyor
                    var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        //kullanıcı önemli bir değeri değiştirdi.Diğer cihazlarda ki cookie leri iptal etmek vb... durumlar için stamp değerini değiştiriyoruz.
                        await _userManager.UpdateSecurityStampAsync(user);

                        //Şifre değiştirildiği için çıkış işlemi yaptırıyoruz
                        await _signInManager.SignOutAsync();

                        //Tekrar giriş işlemi yaptırıyoruz
                        await _signInManager.PasswordSignInAsync(user,model.NewPassword,true,false);

                        TempData.Add("SuccessMessage","Şifre Değişikliği Başarılı bir şekilde gerçekleşti");
                    }
                    else
                    {
                        TempData.Add("ErrorMessage", "Şifre Değişikliği sırasında bir hata gerçekleşti");
                    }
                }
                else
                {
                    ModelState.AddModelError("","Lütfen girmiş olduğunuz şifrenizi kontrol ediniz");
                    TempData.Add("ErrorMessage", "Şifre Değişikliği sırasında bir hata gerçekleşti");
                    return View(model);
                }
            }
            return View(model);
        }


        [Authorize(Roles = "Admin,Editor")]
        //Kullanıcı resmini değiştirirse eski resmin server da kalmaması gerekiyor
        public bool ImageDelete(string pictureName)
        {
            string wwwroot = _env.WebRootPath; //Dosya yolunu dinamik olarak aldık
            var fileToDelete = Path.Combine($"{wwwroot}/img", pictureName);//domain.com/img/picture.jpg

            if (System.IO.File.Exists(fileToDelete))//Klasörde o dosya mevcut mu?
            {
                System.IO.File.Delete(fileToDelete);//dosya siliniyor
                return true;
            }
            else
            {
                return false;
            }
        }



        //Kişinin yetkisi yok ise bu sayfaya yönlendireleceğiz
        [HttpGet]
        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
