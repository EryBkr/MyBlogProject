using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos.RoleDtos;
using MyBlog.Entities.Dtos.UserDtos;
using MyBlog.Mvc.Areas.Admin.Models;
using MyBlog.Mvc.Helpers.Abstract;
using MyBlog.Mvc.Utilities;
using NToastNotify;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : BaseController
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleController(RoleManager<Role> roleManager, IMapper mapper, UserManager<User> userManager, IImageHelper ımageHelper, IToastNotification toastNotification) : base(userManager, mapper, ımageHelper, toastNotification)
        {
            _roleManager = roleManager;
        }

        [Authorize(Roles = "SuperAdmin,Role.Read")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(new RoleListDto { Roles = roles });
        }

        [Authorize(Roles = "SuperAdmin,Role.Read")]
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            var roleListDto = JsonSerializer.Serialize(new RoleListDto { Roles = roles });

            return Json(roleListDto);
        }


        [Authorize(Roles = "SuperAdmin,User.Update")]
        [HttpGet]
        public async Task<IActionResult> Assign(int userId)
        {
            var user = await UserManager.Users.SingleOrDefaultAsync(i => i.Id == userId);
            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await UserManager.GetRolesAsync(user);

            UserRoleAssignDto userRoleAssignDto = new UserRoleAssignDto
            {
                UserId = user.Id,
                UserName = user.UserName,
            };

            foreach (var role in roles)
            {
                RoleAssignDto roleAssignDto = new RoleAssignDto
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    HasRole = userRoles.Contains(role.Name) //Role kullanıcının rollerine dahil mi?
                };

                //RoleAssingDto modelini View e göndereceğimiz modelin içerisindeki listeye ekliyoruz
                userRoleAssignDto.RoleAssignDtos.Add(roleAssignDto);
            }

            return PartialView("_RoleAssignPartial",userRoleAssignDto);
        }

        [Authorize(Roles = "SuperAdmin,User.Update")]
        [HttpPost]
        public async Task<IActionResult> Assign(UserRoleAssignDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.Users.SingleOrDefaultAsync(i => i.Id == model.UserId);

                foreach (var roleAssign in model.RoleAssignDtos)
                {
                    //Kişi Checkbox a tıklamış ise
                    if (roleAssign.HasRole)
                    {
                        //Rol Ataması yapıyoruz
                        await UserManager.AddToRoleAsync(user, roleAssign.RoleName);
                    }
                    else
                    {
                        //işaretlenmemiş rol kişiden çıkartılıyor
                        await UserManager.RemoveFromRoleAsync(user, roleAssign.RoleName);
                    }
                }

                //Kullanıcya rol atadıktan sonra stamp ı güncellersek Configuration da belirlediğimiz interval süresinde kontrol başlatılır ve kullanıcının tekrar giriş yapması istenir.
                await UserManager.UpdateSecurityStampAsync(user);

                var userRoleAssignAjaxViewModel = JsonSerializer.Serialize(new UserRoleAssignAjaxViewModel 
                {
                    UserDto=new UserDto {User=user,Message=$"{user.UserName} kullanıcısına ait rol atama işlemleri başarıyla tamamlanmıştır." },

                    RoleAssignPartial=await this.RenderViewToStringAsync("_RoleAssignPartial", model),
                });

                return Json(userRoleAssignAjaxViewModel);
            }

            else
            {
                var userRoleAssignAjaxViewErrorModel = JsonSerializer.Serialize(new UserRoleAssignAjaxViewModel
                {
                    UserRoleAssignDto=model,
                    RoleAssignPartial = await this.RenderViewToStringAsync("_RoleAssignPartial", model)
                });

                return Json(userRoleAssignAjaxViewErrorModel);
            }

        }
    }
}
