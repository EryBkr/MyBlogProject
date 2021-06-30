using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Entities.Concrete
{
    //Çoka çok ilişki için ara tablomuzu oluşturduk
    public class UserRole:IdentityUserRole<int>
    {
    }
}
