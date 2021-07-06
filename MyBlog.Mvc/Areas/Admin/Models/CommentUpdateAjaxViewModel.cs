using MyBlog.Entities.Dtos.CommentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Areas.Admin.Models
{
    public class CommentUpdateAjaxViewModel
    {
        public CommentUpdateDto CommentUpdateDto { get; set; }
        public string CommentUpdatePartial { get; set; }
        public CommentDto CommentDto { get; set; }
    }
}
