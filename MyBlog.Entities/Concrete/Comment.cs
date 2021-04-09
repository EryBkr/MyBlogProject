using MyBlog.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Entities.Concrete
{
    public class Comment:EntityBase,IEntity
    {
        public string Text { get; set; }

        //Navigation Property and FK
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
