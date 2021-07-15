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

        //Pagination
        public virtual int CurrentPage { get; set; } = 1;
        public virtual int PageSize { get; set; } = 5;
        public virtual int TotalCount { get; set; }
        public virtual int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalCount, PageSize));
        public virtual bool ShowPrevious => CurrentPage > 1; //Önceki sayfa butonu gözüksün mü
        public virtual bool ShowNext => CurrentPage < TotalPages; //Sonraki sayfa butonu gözüksün mü
        public virtual bool IsAscending { get; set; } = false; //Sıralama nasıl olsun
    }
}
