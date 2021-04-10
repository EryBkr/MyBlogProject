using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Shared.Utilities.Results.Abtracts
{
    //Bir tipin farklı kullanımları için out parametresi ekledik
    public interface IDataResult<out T>:IResult
    {
        public T Data { get; }
    }
}
