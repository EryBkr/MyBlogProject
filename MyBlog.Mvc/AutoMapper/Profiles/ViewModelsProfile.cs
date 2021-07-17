using AutoMapper;
using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos.ArticleDtos;
using MyBlog.Mvc.Areas.Admin.Models;
using MyBlog.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Mvc.AutoMapper.Profiles
{
    public class ViewModelsProfile:Profile
    {
        public ViewModelsProfile()
        {
            CreateMap<ArticleAddViewModel, ArticleAddDto>();

            //Tam Tersi dönüşüm de geçerli olsun diye ReverseMap metodunu kullandık
            CreateMap<ArticleUpdateViewModel, ArticleUpdateDto>().ReverseMap();
            CreateMap<ArticleRightSideBarWidgetOptions, ArticleRightSideBarWidgetOptionsViewModel>().ReverseMap();
        }
    }
}
