using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Shared.Entities.Concrete
{
    //Global hatalarımızı sarmallayacak modelimiz
    public class MvcErrorModel
    {
        public string Message { get; set; }
        public string Detail { get; set; }
        public int? StatusCode { get; set; }
    }
}
