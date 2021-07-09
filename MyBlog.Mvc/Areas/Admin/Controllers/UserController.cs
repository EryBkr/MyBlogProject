using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Entities.ComplexTypes;
using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos.UserDtos;
using MyBlog.Mvc.Areas.Admin.Models;
using MyBlog.Mvc.Helpers.Abstract;
using MyBlog.Mvc.Utilities;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using NToastNotify;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : BaseController
    {
        private readonly SignInManager<User> _signInManager;

        public UserController(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, IImageHelper imageHelper, IToastNotification toastNotification) : base(userManager, mapper, imageHelper, toastNotification)
        {
            _signInManager = signInManager;
        }

        [Authorize(Roles = "SuperAdmin,User.Read")]
        public async Task<IActionResult> Index()
        {
            var users = await UserManager.Users.ToListAsync();
            return View(new UserListDto
            {
                Users = users,
                ResultStatus = ResultStatus.Success
            });
        }

      
        [HttpGet]
        [Authorize(Roles = "SuperAdmin,User.Read")]
        public async Task<JsonResult> GetAllUsers()
        {
            var users = await UserManager.Users.ToListAsync();

            var userListDto = JsonSerializer.Serialize(new UserListDto
            {
                Users = users,
                ResultStatus = ResultStatus.Success
            }, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });
            //İç içe oluşan datalardan kaynaklı Options tanımladık

            return Json(userListDto);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,User.Create")]
        public IActionResult Add()
        {
            return PartialView("_UserAddPartial");
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,User.Create")]
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {
            if (ModelState.IsValid)
            {
                var uploadedImageDtoResult = await ImageHelper.UploadImage(userAddDto.UserName, PictureType.User, userAddDto.PictureFile); //Resim kaydedilip gerekli DTO yu dönüyoruz

                //Resim yükleme işlemi başarılı değilse default bir resim gösteriyoruz
                userAddDto.Picture = uploadedImageDtoResult.ResultStatus == ResultStatus.Success ? uploadedImageDtoResult.Data.FullName : "userImages/defaultUser.png";

                var user = Mapper.Map<User>(userAddDto);

                var result = await UserManager.CreateAsync(user, userAddDto.Password);
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
        [Authorize(Roles = "SuperAdmin,User.Delete")]
        public async Task<JsonResult> Delete(int userId)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            var result = await UserManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                //Kullanıcı silme işlemi başarılı ve kullanıcı bizlerin verdiği default resmi kullanmıyorsa kişinin resmini siliyoruz
                if (user.Picture!="userImages/defaultUser.png")
                        ImageHelper.Delete(user.Picture);


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
        [Authorize(Roles = "SuperAdmin,User.Update")]
        public async Task<PartialViewResult> Update(int userId)
        {
            var user = await UserManager.Users.FirstOrDefaultAsync(i => i.Id == userId);
            var userUpdateDto = Mapper.Map<UserUpdateDto>(user);
            return PartialView("_UserUpdatePartial", userUpdateDto);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,User.Update")]
        public async Task<IActionResult> Update(UserUpdateDto model)
        {
            if (ModelState.IsValid)
            {
                bool isNewPictureUploaded = false; //Resim ekleme işlemi başarılı mı ? Diğer türlü mevcut resmi kullanıcı resmi kaydedemeden silmiş oluruz
                var oldUser = await UserManager.FindByIdAsync(model.Id.ToString());
                var oldUserPicture = oldUser.Picture;//Silinecek Resmi aldık
                if (model.PictureFile != null) //Kullanıcı Yeni Resim Eklemiş
                {
                    var uploadedImageDtoResult = await ImageHelper.UploadImage(model.UserName, PictureType.User, model.PictureFile); //Resim kaydedilip gerekli DTO yu dönüyoruz

                    //Resim yükleme işlemi başarılı değilse default bir resim gösteriyoruz
                    model.Picture = uploadedImageDtoResult.ResultStatus == ResultStatus.Success ? uploadedImageDtoResult.Data.FullName : "userImages/defaultUser.png";

                    if (oldUserPicture != "userImages/defaultUser.png")
                    {
                        isNewPictureUploaded = true;
                    }
                }
                var updatedUser = Mapper.Map<UserUpdateDto, User>(model, oldUser);
                var result = await UserManager.UpdateAsync(updatedUser);

                if (result.Succeeded) //Güncelleme işlemi başarılı mı 
                {
                    if (isNewPictureUploaded) //Yeni Resim ekleme işlemi başarılı mı?
                    {
                        ImageHelper.Delete(oldUserPicture);
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

        [HttpGet]
        [Authorize]
        public async Task<ViewResult> ChangeDetails()
        {
            var user = await UserManager.GetUserAsync(HttpContext.User);
            var updateDto = Mapper.Map<UserUpdateDto>(user);
            return View(updateDto);
        }

        [HttpPost]
        [Authorize]
        public async Task<ViewResult> ChangeDetails(UserUpdateDto model)
        {
            if (ModelState.IsValid)
            {
                bool isNewPictureUploaded = false; //Resim ekleme işlemi başarılı mı ? Diğer türlü mevcut resmi kullanıcı resmi kaydedemeden silmiş oluruz
                var oldUser = await UserManager.GetUserAsync(HttpContext.User);
                var oldUserPicture = oldUser.Picture;//Silinecek Resmi aldık
                if (model.PictureFile != null) //Kullanıcı Yeni Resim Eklemiş
                {
                    var uploadedImageDtoResult = await ImageHelper.UploadImage(model.UserName, PictureType.User, model.PictureFile); //Resim kaydedilip gerekli DTO yu dönüyoruz

                    //Resim yükleme işlemi başarılı değilse default bir resim gösteriyoruz
                    model.Picture = uploadedImageDtoResult.ResultStatus == ResultStatus.Success ? uploadedImageDtoResult.Data.FullName : "userImages/defaultUser.png";

                    //Kullanıcının mevcut resmi,sistemin default olarak atadığı resim ise silmiyoruz.Diğer default resim kullanıcıları resimsiz kalır diğer türlü
                    if (oldUserPicture != "userImages/defaultUser.png")
                        isNewPictureUploaded = true;
                }

                var updatedUser = Mapper.Map<UserUpdateDto, User>(model, oldUser);
                var result = await UserManager.UpdateAsync(updatedUser);

                if (result.Succeeded) //Güncelleme işlemi başarılı mı 
                {
                    if (isNewPictureUploaded) //Yeni Resim ekleme işlemi başarılı mı?
                        ImageHelper.Delete(oldUserPicture);

                    //Toastr mesajımızı oluşturduk
                    ToastNotification.AddSuccessToastMessage($"{updatedUser.UserName} adlı kullanıcı başarıyla güncellenmiştir");


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
                var user = await UserManager.GetUserAsync(HttpContext.User);

                //Kullanıcı şifresini hatırlıyor mu kontrol ediyoruz
                var isVerified = await UserManager.CheckPasswordAsync(user, model.CurrentPassword);

                //Şifresini Hatırlıyor ise
                if (isVerified)
                {
                    //şifre değişikliği işlemi yapılıyor
                    var result = await UserManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        //kullanıcı önemli bir değeri değiştirdi.Diğer cihazlarda ki cookie leri iptal etmek vb... durumlar için stamp değerini değiştiriyoruz.
                        await UserManager.UpdateSecurityStampAsync(user);

                        //Şifre değiştirildiği için çıkış işlemi yaptırıyoruz
                        await _signInManager.SignOutAsync();

                        //Tekrar giriş işlemi yaptırıyoruz
                        await _signInManager.PasswordSignInAsync(user, model.NewPassword, true, false);

                        //Toastr mesajımızı oluşturduk
                        ToastNotification.AddSuccessToastMessage("Şifre Değişikliği Başarılı bir şekilde gerçekleşti");
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Lütfen girmiş olduğunuz şifrenizi kontrol ediniz");

                    //Toastr mesajımızı oluşturduk
                    ToastNotification.AddErrorToastMessage("Şifre Değişikliği sırasında bir hata gerçekleşti");

                    return View(model);
                }
            }
            return View(model);
        }


        [Authorize(Roles = "SuperAdmin,User.Read")]
        [HttpGet]
        public async Task<PartialViewResult> GetDetail(int userId)
        {
            var user = await UserManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
            return PartialView("_GetDetailPartial", new UserDto { User = user });
        }
    }
}
