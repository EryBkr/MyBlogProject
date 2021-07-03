using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Services.Utilities
{
    public static class Messages
    {
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
            /// Kategori silme işlemi başarısız ise...
            /// </summary>
            /// <param name="categoryName"></param>
            /// <returns></returns>
            public static string DeleteError()
            {
                return "Silinmek istenen kategori bulunamadı";
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
            public static string Update(string articleName)
            {
                return $"{articleName} adlı makale başarıyla güncellenmiştir";
            }
        }
    }
}
