using MyBlog.Shared.Utilities.Results.ComplexTypes;
using MyBlog.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBlog.Shared.Entities.Abstract;

namespace MyBlog.Entities.Dtos.ArticleDtos
{
    public class ArticleDto:DtoGetBase
    {
        public Article Article { get; set; }

        //Hata durumunda Error atamasını biz yaparız default olarak başarılı gözüksün
        public override ResultStatus ResultStatus { get; set; } = ResultStatus.Success;

    }
}
