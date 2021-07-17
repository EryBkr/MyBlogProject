using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [DisplayName("Başlık")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        [MaxLength(150, ErrorMessage = "{0} {1} karakterden büyük olamaz")]
        [MinLength(3, ErrorMessage = "{0} {1} karakterden az olamaz")]
        public string Header { get; set; }

        [DisplayName("İçerik")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        [MaxLength(1500, ErrorMessage = "{0} {1} karakterden büyük olamaz")]
        [MinLength(3, ErrorMessage = "{0} {1} karakterden az olamaz")]
        public string Content { get; set; }

        [DisplayName("Seo Açıklaması")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        [MaxLength(100, ErrorMessage = "{0} {1} karakterden büyük olamaz")]
        [MinLength(3, ErrorMessage = "{0} {1} karakterden az olamaz")]
        public string SeoDescription { get; set; }

        [DisplayName("Seo Etiketleri")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        [MaxLength(100, ErrorMessage = "{0} {1} karakterden büyük olamaz")]
        [MinLength(3, ErrorMessage = "{0} {1} karakterden az olamaz")]
        public string SeoTags { get; set; }

        [DisplayName("Seo Yazar")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        [MaxLength(100, ErrorMessage = "{0} {1} karakterden büyük olamaz")]
        [MinLength(3, ErrorMessage = "{0} {1} karakterden az olamaz")]
        public string SeoAuthor { get; set; }
    }
}
