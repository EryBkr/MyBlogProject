using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Entities.Dtos.CategoryDtos
{
    public class CategoryUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [DisplayName("Kategori Adı")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        [MaxLength(70, ErrorMessage = "{0} {1} karakterden büyük olamaz")]
        [MinLength(3, ErrorMessage = "{0} {1} karakterden az olamaz")]
        public string Name { get; set; }


        [DisplayName("Kategori Açıklamazı")]
        [MaxLength(500, ErrorMessage = "{0} {1} karakterden büyük olamaz")]
        [MinLength(3, ErrorMessage = "{0} {1} karakterden az olamaz")]
        public string Description { get; set; }


        [DisplayName("Kategori Not Alanı")]
        [MaxLength(500, ErrorMessage = "{0} {1} karakterden büyük olamaz")]
        [MinLength(3, ErrorMessage = "{0} {1} karakterden az olamaz")]
        public string Note { get; set; }


        [DisplayName("Aktif Mi?")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        public bool IsActive { get; set; }


        [DisplayName("Silindi Mi?")]
        [Required(ErrorMessage = "{0} Boş Geçilemez")] //{0} display name dir
        public bool IsDeleted { get; set; }
    }
}
