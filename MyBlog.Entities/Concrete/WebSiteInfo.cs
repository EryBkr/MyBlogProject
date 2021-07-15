using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Entities.Concrete
{
    //Layout sayfasına ekleyeceğimiz meta ve static yazıların model classı
    public class WebSiteInfo
    {
        public string Title { get; set; }
        public string MenuTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTags { get; set; }
        public string SeoAuthor { get; set; }
    }
}
