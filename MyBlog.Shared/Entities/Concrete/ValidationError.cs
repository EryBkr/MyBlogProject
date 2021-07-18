using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Shared.Entities.Concrete
{
    public class ValidationError
    {
        public string PropertyName { get; set; } //Hangi inputta invalid bir durum var
        public string Message { get; set; }
    }
}
