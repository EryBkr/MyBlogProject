using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyBlog.Shared.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Filters
{
    //Uygulama kısmında hata oluştuğu zaman devreye girecek yapıdır
    public class MvcExceptionFilter : IExceptionFilter
    {
        //Production ortamında mıyız yoksa development ortamındayız bunu anlamak için kullanacağız
        private readonly IHostEnvironment _environment;

        private readonly IModelMetadataProvider _metadataProvider;
        private readonly ILogger _logger;

        public MvcExceptionFilter(IHostEnvironment environment, IModelMetadataProvider metadataProvider, ILogger<MvcExceptionFilter> logger)
        {
            _environment = environment;
            _metadataProvider = metadataProvider;
            _logger = logger;
        }

        //Hata fırlatıldığında devreye girecektir
        public void OnException(ExceptionContext context)
        {
            //Hangi ortamda çalışmasını istiyorsak
            if (_environment.IsDevelopment())
            {
                ViewResult result;

                //Hata kontrolünü biz devralıyoruz
                context.ExceptionHandled = true;

                //View e gidecek modelimiz
                var mvcErrorModel = new MvcErrorModel();

                //Exception Türlerine özel hata kontrolü
                switch (context.Exception)
                {
                    case SqlNullValueException:
                        mvcErrorModel.Message = $"Üzgünüz işlemini sırasında beklenmedik bir veritabanı hatası oluştu.Sorunu en kısa sürede çözeceğiz";
                        mvcErrorModel.Detail = context.Exception.Message;
                        //Hata durumunda dönmek istediğimiz View in adı
                        result = new ViewResult { ViewName = "Error" };
                        result.StatusCode = 500;
                        mvcErrorModel.StatusCode = 500;
                        _logger.LogError(context.Exception, context.Exception.Message); //Merkezi bir konumda log lama işlemi yapıyoruz
                        break;
                    case NullReferenceException:
                        mvcErrorModel.Message = $"Üzgünüz işlemini sırasında beklenmedik bir null veri hatası oluştu.Sorunu en kısa sürede çözeceğiz";
                        mvcErrorModel.Detail = context.Exception.Message;
                        //Hata durumunda dönmek istediğimiz View in adı
                        result = new ViewResult { ViewName = "Error" };
                        result.StatusCode = 403;
                        mvcErrorModel.StatusCode = 403;
                        _logger.LogError(context.Exception, context.Exception.Message);
                        break;
                    default:
                        mvcErrorModel.Message = $"Üzgünüz işlemini sırasında beklenmedik bir hata oluştu.Sorunu en kısa sürede çözeceğiz";
                        //Hata durumunda dönmek istediğimiz View in adı
                        result = new ViewResult { ViewName = "Error" };
                        mvcErrorModel.StatusCode = 500;
                        _logger.LogError(context.Exception, "Custom Log Error");
                        break;
                }


                result.ViewData = new ViewDataDictionary(_metadataProvider, context.ModelState);

                //View e gidecek olan Data yı tanımladık
                result.ViewData.Add("MvcErrorModel", mvcErrorModel);

                context.Result = result;
            }
        }
    }
}
