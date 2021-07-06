using MyBlog.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyBlog.Entities.Dtos.CommentDtos
{
    public class CommentListDto
    {
        public IList<Comment> Comments { get; set; }
    }
}
