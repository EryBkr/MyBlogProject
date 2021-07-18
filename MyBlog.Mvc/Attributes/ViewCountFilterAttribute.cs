using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using MyBlog.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MyBlog.Mvc.Attributes
{
    //Makale okunma sayısını cookie bazlı kontrol edeceğiz bunu attribute filter yapacağız
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)] //Bu attribute nerelerde çalışabilir
    public class ViewCountFilterAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Detail Action a gelen articleId parametresinde ki değeri alıyoruz
            var articleId = context.ActionArguments["articleId"];

            if (articleId is not null)
            {
                //Aldığımız bu değere ait Cookie var mı kontrol ediyoruz.Var ise okunma sayısını arttırmayacağız yok ise okunma sayısını arttırıp cookie oluşturma işlemi yapacağız
                string articleValue = context.HttpContext.Request.Cookies[$"article{articleId}"];

                if (string.IsNullOrEmpty(articleValue))
                {
                    //Cookie atanma işlemini gerçekleştiriyoruz
                    Set($"article{articleId}", articleId.ToString(), 1,context.HttpContext.Response);

                    //DI ile contexten servisi talep ediyoruz
                    var articleService = context.HttpContext.RequestServices.GetService<IArticleService>();

                    //articleId ye sahip makalenin okunma sayısını arttırıyoruz
                    await articleService.IncreaseViewCountAsync(Convert.ToInt32(articleId));

                    //Attribute olarak eklediğimiz fonksiyonun işlemini devam ettirmesini sağlıyoruz
                    await next();
                }
            }
            await next();
        }

        /// <summary>  
        /// set the cookie  
        /// </summary>  
        /// <param name="key">cookie adını belirtiyoruz</param>  
        /// <param name="value">article ID</param>  
        /// <param name="expireTime">cookie barındırma süremiz</param>  
        public void Set(string key, string value, int? expireTime, HttpResponse response)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddYears(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMonths(6);

            response.Cookies.Append(key, value, option);
        }

        /// <summary>  
        /// Delete the key  
        /// </summary>  
        /// <param name="key">Key</param>  
        public void Remove(string key, HttpResponse response)
        {
            response.Cookies.Delete(key);
        }
    }
}
