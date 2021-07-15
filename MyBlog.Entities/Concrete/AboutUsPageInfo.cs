using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Entities.Concrete
{
    /// <summary>
    /// AppSettings ten değerleri alıp View e vermemizi sağlayan modelimiz.DB yi kullanmamıza gerek yok.Hakkımızda sayfasında ki bilgileri oluşturacaktır
    /// </summary>
    public class AboutUsPageInfo
    {
        public string Header { get; set; }
        public string Content { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTags { get; set; }
        public string SeoAuthor { get; set; }
    }
}
