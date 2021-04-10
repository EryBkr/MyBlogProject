using MyBlog.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Entities.Concrete
{
    public class Article:EntityBase,IEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        public DateTime Date { get; set; }

        //Bu iki değer oluşturulurken 0 olarak atanacaktır
        public int ViewsCount { get; set; } = 0;
        public int CommentsCount { get; set; } = 0;

        public string SeoAuthor { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTags { get; set; }

        //Navigation Property and FK
        public int CategoryId { get; set; }
        public Category Category { get; set; }


        //Navigation Property and FK
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
