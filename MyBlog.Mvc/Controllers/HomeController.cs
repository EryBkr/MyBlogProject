using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos.EmailDtos;
using MyBlog.Services.Abstract;
using MyBlog.Shared.Utilities.Helpers.Abstract;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using NToastNotify;
using System;
using System.Data.SqlTypes;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Controllers
{
    [Route("/")] //Domain.com/ --> slash ı tanımlar
    public class HomeController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly AboutUsPageInfo _aboutUsPageInfo;
        private readonly IMailService _mailService;
        private readonly IToastNotification _toastNotfy;

        //Appsettings dosyamız üzerinde gerekli section üzerinde değişiklik yapabilmek için ekledik
        private readonly IWritableOptions<AboutUsPageInfo> _aboutUsPageWritable;

        //IOptionsSnapshot kullanımı,eğer projemiz ayaktayken json file üzerinde yapılan değişikliklerin projemize yansımasını sağlar
        public HomeController(IArticleService articleService, IOptionsSnapshot<AboutUsPageInfo> aboutUsPageInfo, IMailService mailService, IToastNotification toastNotfy, IWritableOptions<AboutUsPageInfo> aboutUsPageWritable)
        {
            _articleService = articleService;
            _aboutUsPageInfo = aboutUsPageInfo.Value; //Startup a eklediğimiz Configuration sayesinde IOptions aracılığıyla modelimizi appsettings ten dolduruyoruz
            _mailService = mailService;
            _toastNotfy = toastNotfy;
            _aboutUsPageWritable = aboutUsPageWritable;
        }

        [HttpGet]
        [Route("index")] //SEO için gözükecek URL
        [Route("anasayfa")]
        [Route("")]
        public async Task<IActionResult> Index(int? categoryId, int currentPage = 1, int pageSize = 5,bool isAscending=false)
        {
            //Eğer kategori bilgisi bize verilirse ona uygun olan makaleleri sayfada göstereceğiz.Diğer türlü bütün makaleleri göstereceğiz sayfalamaya uygun olarak getireceğiz
            var articleListDto = await (categoryId == null
                ? _articleService.GetAllByPagingAsync(null, currentPage, pageSize,isAscending)
                : _articleService.GetAllByPagingAsync(categoryId.Value, currentPage, pageSize,isAscending));

            return View(articleListDto.Data);
        }


        public IActionResult ComingSoonIndex()
        {
            return View();
        }

        [HttpGet]
        [Route("hakkimizda")]
        [Route("hakkkinda")]
        public IActionResult About()
        {
            return View(_aboutUsPageInfo);
        }


        [HttpGet]
        [Route("iletisim")]
        public IActionResult Contact()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Contact(EmailSendDto model)
        {
            if (ModelState.IsValid)
            {
                var result=_mailService.SendContactEmail(model);

                if (result.ResultStatus==ResultStatus.Success)
                {
                    _toastNotfy.AddSuccessToastMessage(result.Message, new ToastrOptions { Title = "Başarılı İşlem" });
                }
                return View();
            }
            
            return View(model);
        }
    }
}
