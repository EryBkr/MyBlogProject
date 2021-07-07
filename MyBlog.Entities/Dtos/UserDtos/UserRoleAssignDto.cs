using MyBlog.Entities.Dtos.RoleDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Entities.Dtos.UserDtos
{
    public class UserRoleAssignDto
    {
        public UserRoleAssignDto()
        {
            //IList bir interface olduğundan kendini otomatik initilaizer etmez.Bunu burada çözmemiz gerekiyordu
            RoleAssignDtos = new List<RoleAssignDto>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public IList<RoleAssignDto> RoleAssignDtos { get; set; }
    }
}
