using MyBlog.Shared.Entities.Concrete;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Shared.Utilities.Results.Abtracts
{
    public interface IResult
    {
        //Sadece Yapıcı Metotta değer ataması olması için set; kısımlarını kaldırdık
        //Diğer türlü gelen datalar manipüle edilebilir
        public ResultStatus ResultStatus { get; }
        public string Message { get; }
        public Exception Exception { get; }

        //iş katmanında aldığımız validasyon hatalarını UI tarafına gönderebilmek için ekledik
        public IEnumerable<ValidationError> ValidationErrors { get; }
    }
}
