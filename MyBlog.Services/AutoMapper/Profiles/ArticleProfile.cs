using AutoMapper;
using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos.ArticleDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Services.AutoMapper.Profiles
{
    public class ArticleProfile:Profile
    {
        public ArticleProfile()
        {
            //DTO sınıfımızda olmayan bir property için otomatik değer ekledik
            CreateMap<ArticleAddDto, Article>().ForMember(dest=>dest.CreatedDate,opt=>opt.MapFrom(x=>DateTime.Now));
            CreateMap<ArticleUpdateDto, Article>().ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<Article, ArticleUpdateDto>();
        }
    }
}
