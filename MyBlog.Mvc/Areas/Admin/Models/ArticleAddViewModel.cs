using Microsoft.AspNetCore.Http;
using MyBlog.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Areas.Admin.Models
{
    public class ArticleAddViewModel
    {
        [DisplayName("Başlık")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        [MaxLength(100, ErrorMessage = "{0} {1} karakterden büyük olamaz")]
        [MinLength(3, ErrorMessage = "{0} {1} karakterden az olamaz")]
        public string Title { get; set; }

        [DisplayName("İçerik")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        [MinLength(3, ErrorMessage = "{0} {1} karakterden az olamaz")]
        public string Content { get; set; }


        [DisplayName("Küçük Resim")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        public IFormFile ThumbnailFile { get; set; }


        [DisplayName("Tarih")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/YYYY}")] //Tarihin hangi formatta olması gerektiğini belirledik
        public DateTime Date { get; set; }

        [DisplayName("Yazar Adı")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        [MaxLength(50, ErrorMessage = "{0} {1} karakterden büyük olamaz")]
        [MinLength(0, ErrorMessage = "{0} {1} karakterden az olamaz")]
        public string SeoAuthor { get; set; }

        [DisplayName("Makale Açıklaması")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        [MaxLength(150, ErrorMessage = "{0} {1} karakterden büyük olamaz")]
        [MinLength(5, ErrorMessage = "{0} {1} karakterden az olamaz")]
        public string SeoDescription { get; set; }

        [DisplayName("Makale Etiketleri")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        [MaxLength(70, ErrorMessage = "{0} {1} karakterden büyük olamaz")]
        [MinLength(5, ErrorMessage = "{0} {1} karakterden az olamaz")]
        public string SeoTags { get; set; }

        [DisplayName("Kategori")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        public int CategoryId { get; set; }

        public IList<Category> Categories { get; set; }

        [DisplayName("Aktif mi?")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        public bool IsActive { get; set; }
    }
}
