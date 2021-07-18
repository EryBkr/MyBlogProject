using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Services.Utilities
{
    public static class Messages
    {
       
        public static class General
        {
            public static string ValidationError()
            {
                return "Bir veya daha fazla validasyon hatası ile karşılaşıldı";
            }
        }


        public static class Users
        {
            /// <summary>
            /// Verilen Id e ait kullanıcı var mı
            /// </summary>
            /// <param name="userId"></param>
            /// <returns></returns>
            public static string NotFoundById(int userId)
            {
                return $"{userId} user koduna ait bir user bulunamadı";
            }
        }

        public static class Category
        {
            /// <summary>
            /// Kategorinin çoğul mu tekil mi olduğuna göre dönen mesaj değişecektir
            /// </summary>
            /// <param name="isPlural"></param>
            /// <returns></returns>
            public static string NotFound(bool isPlural)
            {
                return isPlural ? "Hiç bir kategori bulunamadı" : "Böyle bir kategori bulunamadı";
            }


            /// <summary>
            /// Verilen Id e ait kategori var mı
            /// </summary>
            /// <param name="categoryId"></param>
            /// <returns></returns>
            public static string NotFoundById(int categoryId)
            {
                return $"{categoryId} kategori koduna ait bir kategori bulunamadı";
            }


            /// <summary>
            /// Kategori ekleme işlemi başarılı ise...
            /// </summary>
            /// <param name="categoryName"></param>
            /// <returns></returns>
            public static string Add(string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla eklenmiştir";
            }

            /// <summary>
            /// Kategori silme işlemi başarılı ise...
            /// </summary>
            /// <param name="categoryName"></param>
            /// <returns></returns>
            public static string Delete(string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla silinmiştir";
            }

            /// <summary>
            /// Kategori silme işlemi başarılı ise...
            /// </summary>
            /// <param name="categoryName"></param>
            /// <returns></returns>
            public static string NonDelete(string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla geri alınmıştır";
            }


            /// <summary>
            /// Kategori silme işlemi başarısız ise...
            /// </summary>
            /// <returns></returns>
            public static string DeleteError()
            {
                return "Silinmek istenen kategori bulunamadı";
            }

            /// <summary>
            /// Kategori silme işlemi başarısız ise...
            /// </summary>
            /// <returns></returns>
            public static string NonDeleteError()
            {
                return "Geri getirilmek istenen kategori bulunamadı";
            }

            /// <summary>
            /// Günceleme işlemi başarılı ise
            /// </summary>
            /// <param name="categoryName"></param>
            /// <returns></returns>
            public static string Update(string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla güncellenmiştir";
            }


        }

        public static class Article
        {
            /// <summary>
            /// Makalenin çoğul mu tekil mi olduğuna göre dönen mesaj değişecektir
            /// </summary>
            /// <param name="isPlural"></param>
            /// <returns></returns>
            public static string NotFound(bool isPlural)
            {
                return isPlural ? "Hiç bir makale bulunamadı" : "Böyle bir makale bulunamadı";
            }

            /// <summary>
            ///  Verilen Id e ait makale var mı
            /// </summary>
            /// <param name="articleId"></param>
            /// <returns></returns>
            public static string NotFoundById(int articleId)
            {
                return  $"{articleId} makale koduna ait bir makale bulunamadı";
            }

            /// <summary>
            /// Makalenin çoğul mu tekil mi olduğuna göre dönen mesaj değişecektir
            /// </summary>

            /// <returns></returns>
            public static string NotFoundByCategory()
            {
                return "Bu kategoriye ait hiç bir makale bulunamadı";
            }

            /// <summary>
            /// Makale ekleme işlemi başarılı ise...
            /// </summary>
            /// <param name="articleName"></param>
            /// <returns></returns>
            public static string Add(string articleName)
            {
                return $"{articleName} adlı makale başarıyla eklenmiştir";
            }

            /// <summary>
            /// Kategori silme işlemi başarılı ise...
            /// </summary>
            /// <param name="articleName"></param>
            /// <returns></returns>
            public static string Delete(string articleName)
            {
                return $"{articleName} adlı makale başarıyla silinmiştir";
            }

            /// <summary>
            /// Kategori silme işlemi başarılı ise...
            /// </summary>
            /// <param name="articleName"></param>
            /// <returns></returns>
            public static string NonDelete(string articleName)
            {
                return $"{articleName} adlı makale başarıyla geri getirilmiştir";
            }


            /// <summary>
            /// Kategori silme işlemi başarılı ise...
            /// </summary>
            /// <param name="articleName"></param>
            /// <returns></returns>
            public static string Update(string articleName)
            {
                return $"{articleName} adlı makale başarıyla güncellenmiştir";
            }

            public static string IncreaseViewCount(string title)
            {
                return $"{title} başlıklı makalenin okunma sayısı başarıyla arttırılmıştır.";
            }
        }

        public static class Comment
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Hiç bir yorum bulunamadı.";
                return "Böyle bir yorum bulunamadı.";
            }

            public static string Add(string createdByName)
            {
                return $"Sayın {createdByName}, yorumunuz başarıyla eklenmiştir.";
            }

            public static string Approve(int commentId)
            {
                return $"{commentId} numaralı yorum başarıyla onaylanmıştır.";
            }

            public static string Update(string createdByName)
            {
                return $"{createdByName} tarafından eklenen yorum başarıyla güncellenmiştir.";
            }
            public static string Delete(string createdByName)
            {
                return $"{createdByName} tarafından eklenen yorum başarıyla silinmiştir.";
            }
            public static string NonDelete(string createdByName)
            {
                return $"{createdByName} tarafından eklenen yorumun geri alınma işlemi başarıyla tamamlanmıştır.";
            }
            public static string HardDelete(string createdByName)
            {
                return $"{createdByName} tarafından eklenen yorum başarıyla veritabanından silinmiştir.";
            }
        }
    }
}
