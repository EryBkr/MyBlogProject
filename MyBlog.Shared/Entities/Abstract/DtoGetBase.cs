using MyBlog.Shared.Utilities.Results.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Shared.Entities.Abstract
{
    public abstract class DtoGetBase
    {
        public virtual ResultStatus ResultStatus { get; set; } //View Tarafında da bazı kontroller yapmak isteyebiliriz.Bunun için ekledik

        public virtual string Message { get; set; } //Sayfaya giden model içerisinde de hata mesajını göstermek isteyebiliriz diye ekledik
    }
}
