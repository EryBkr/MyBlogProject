using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos.ArticleDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int CategoriesCount { get; set; }
        public int ArticlesCount { get; set; }
        public int CommentsCount { get; set; }
        public int UsersCount { get; set; }
        public ArticleListDto Articles { get; set; }
    }
}
