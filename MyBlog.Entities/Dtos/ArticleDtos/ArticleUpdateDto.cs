using MyBlog.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Entities.Dtos.ArticleDtos
{
    public class ArticleUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [DisplayName("Başlık")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        [MaxLength(100, ErrorMessage = "{0} {1} karakterden büyük olamaz")]
        [MinLength(3, ErrorMessage = "{0} {1} karakterden az olamaz")]
        public string Title { get; set; }

        [DisplayName("İçerik")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        [MinLength(3, ErrorMessage = "{0} {1} karakterden az olamaz")]
        public string Content { get; set; }


        [DisplayName("Thumbnail")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        [MaxLength(250, ErrorMessage = "{0} {1} karakterden büyük olamaz")]
        [MinLength(5, ErrorMessage = "{0} {1} karakterden az olamaz")]
        public string Thumbnail { get; set; }


        [DisplayName("Tarih")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/YYYY}")] //Tarihin hangi formatta olması gerektiğini belirledik
        public DateTime Date { get; set; }

        [DisplayName("Seo Yazar")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        [MaxLength(50, ErrorMessage = "{0} {1} karakterden büyük olamaz")]
        [MinLength(0, ErrorMessage = "{0} {1} karakterden az olamaz")]
        public string SeoAuthor { get; set; }

        [DisplayName("Seo Açıklama")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        [MaxLength(150, ErrorMessage = "{0} {1} karakterden büyük olamaz")]
        [MinLength(5, ErrorMessage = "{0} {1} karakterden az olamaz")]
        public string SeoDescription { get; set; }

        [DisplayName("Seo Etiketler")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        [MaxLength(70, ErrorMessage = "{0} {1} karakterden büyük olamaz")]
        [MinLength(5, ErrorMessage = "{0} {1} karakterden az olamaz")]
        public string SeoTags { get; set; }

        [DisplayName("Kategori")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [DisplayName("Aktif mi?")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        public bool IsActive { get; set; }

        [DisplayName("Silinsin mi?")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        public bool IsDeleted { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
