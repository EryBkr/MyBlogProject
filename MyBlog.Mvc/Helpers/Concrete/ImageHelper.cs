using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MyBlog.Entities.Dtos.FileDtos;
using MyBlog.Mvc.Helpers.Abstract;
using MyBlog.Mvc.Utilities;
using MyBlog.Shared.Utilities.Results.Abtracts;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using MyBlog.Shared.Utilities.Results.Concrete;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Helpers.Concrete
{
    public class ImageHelper : IImageHelper
    {

        private readonly IWebHostEnvironment _env; //Dosya yoluna dinamik ulaşabilmek için ekledik 
        private readonly string _wwwRoot;
        private readonly string imgFolder = "img";

        public ImageHelper(IWebHostEnvironment env)
        {
            _env = env;
            _wwwRoot = _env.WebRootPath; //Dosya yolunu dinamik olarak aldık
        }

        public IDataResult<ImageDeleteDto> Delete(string pictureName)
        {
            var fileToDelete = Path.Combine($"{_wwwRoot}/{imgFolder}/", pictureName);//domain.com/img/picture.jpg

            if (System.IO.File.Exists(fileToDelete))//Klasörde o dosya mevcut mu?
            {
                var fileInfo = new FileInfo(fileToDelete);

                //Resim silindikten sonra fileInfo değerlerine ulaşamadığımızdan dolayı silinmeden önce gerekli bilgileri alıyoruz
                var imageDeletedDto = new ImageDeleteDto 
                {
                    FullName=pictureName,
                    Extension=fileInfo.Extension,
                    FullPath=fileInfo.FullName,
                    Size=fileInfo.Length
                };

                System.IO.File.Delete(fileToDelete);//dosya siliniyor
                return new DataResult<ImageDeleteDto>(ResultStatus.Success,imageDeletedDto);
            }
            else
            {
                return new DataResult<ImageDeleteDto>(ResultStatus.Error,null,$"{pictureName} adlı resim bulunamadı");
            }
        }

        public async Task<IDataResult<ImageUploadDto>> UploadUserImage(string userName, IFormFile pictureFile, string folderName="userImages")
        {

            //Verilen klasör ismine ait bir klasör var mı?
            if (!Directory.Exists($"{_wwwRoot}/{imgFolder}/{folderName}"))
                    Directory.CreateDirectory($"{_wwwRoot}/{imgFolder}/{folderName}"); //verilen klasör yok ise oluşturuyoruz

            string oldFileName = Path.GetFileNameWithoutExtension(pictureFile.FileName);
            string fileExtension = Path.GetExtension(pictureFile.FileName); //resmin uzantısını aldık

            var dateTime = DateTime.Now; //O an ki zamanı isimlendirme için aldık

            string newFileName = $"{userName}_{dateTime.FullDateAndTimeStringUnderscore()}{fileExtension}"; //UserName_DateTimeNow.jpg gibi

            var path = Path.Combine($"{_wwwRoot}/{imgFolder}/{folderName}", newFileName);

            //Uzantısı ile birlikte dosya ve o dosyaya uygulanacak işlemi belirtiyoruz
            await using (var stream = new FileStream(path, FileMode.Create))
                await pictureFile.CopyToAsync(stream);


            return new DataResult<ImageUploadDto>(ResultStatus.Success, new ImageUploadDto
            {
                FullName = $"{folderName}/{newFileName}",
                OldName = oldFileName,
                Extension = fileExtension,
                FolderName = folderName,
                Path = path,
                Size = pictureFile.Length
            }, $"{userName} adlı kullanıcının resmi başarıyla yüklenmiştir");
        }
    }
}
