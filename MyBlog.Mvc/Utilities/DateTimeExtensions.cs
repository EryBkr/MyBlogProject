using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Utilities
{
    public static class DateTimeExtensions
    {
        //Bu Extension metot dosyalar için unique bir isim oluşturacaktır.Böylece hiçbir ismin bir öncekinin aynı olmayacağını garanti etmiş olacağız
        public static string FullDateAndTimeStringUnderscore(this DateTime dateTime)
        {
            return $"{dateTime.Millisecond}_{dateTime.Second}_{dateTime.Minute}_{dateTime.Hour}_{dateTime.Day}_{dateTime.Month}_{dateTime.Year}";
        }
    }
}
