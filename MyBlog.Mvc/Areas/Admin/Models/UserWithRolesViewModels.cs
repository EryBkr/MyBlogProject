using MyBlog.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Areas.Admin.Models
{
    //Kullanıcı bilgilerini ve rollerini Sidebar View Componente göndermek için oluşturduğumuz bir model
    public class UserWithRolesViewModels
    {
        public User User { get; set; }
        public IList<string> Roles { get; set; }
    }
}
