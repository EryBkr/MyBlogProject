using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MyBlog.Entities.ComplexTypes;
using MyBlog.Entities.Dtos.FileDtos;
using MyBlog.Mvc.Helpers.Abstract;
using MyBlog.Mvc.Utilities;
using MyBlog.Shared.Utilities.Results.Abtracts;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using MyBlog.Shared.Utilities.Results.Concrete;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Helpers.Concrete
{
    public class ImageHelper : IImageHelper
    {

        private readonly IWebHostEnvironment _env; //Dosya yoluna dinamik ulaşabilmek için ekledik 
        private readonly string _wwwRoot;
        private readonly string imgFolder = "img";
        private const string userImagesFolder = "userImages";
        private const string postImagesFolder = "postImages";

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
                    FullName = pictureName,
                    Extension = fileInfo.Extension,
                    FullPath = fileInfo.FullName,
                    Size = fileInfo.Length
                };

                System.IO.File.Delete(fileToDelete);//dosya siliniyor
                return new DataResult<ImageDeleteDto>(ResultStatus.Success, imageDeletedDto);
            }
            else
            {
                return new DataResult<ImageDeleteDto>(ResultStatus.Error, null, $"{pictureName} adlı resim bulunamadı");
            }
        }

        public async Task<IDataResult<ImageUploadDto>> UploadImage(string name, PictureType
             pictureType, IFormFile pictureFile, string folderName = null)
        {
            //Verilen Resim tipine göre klasör ismi ataması yapıyoruz
            folderName ??= pictureType == PictureType.User ? userImagesFolder : postImagesFolder;

            //Verilen klasör ismine ait bir klasör var mı?
            if (!Directory.Exists($"{_wwwRoot}/{imgFolder}/{folderName}"))
                Directory.CreateDirectory($"{_wwwRoot}/{imgFolder}/{folderName}"); //verilen klasör yok ise oluşturuyoruz

            string oldFileName = Path.GetFileNameWithoutExtension(pictureFile.FileName);
            string fileExtension = Path.GetExtension(pictureFile.FileName); //resmin uzantısını aldık

            //Makale ismindeki özel karakterler bizler için problem oluşturuyordu.Bundan kaynaklı bu problemi regex ile çözeceğiz.Özel karakterler yerine artık boş string (white space değil) gelecektir.
            Regex regex = new Regex("[*'\",._&#^@]");
            name = regex.Replace(name,string.Empty);

            var dateTime = DateTime.Now; //O an ki zamanı isimlendirme için aldık

            string newFileName = $"{name}_{dateTime.FullDateAndTimeStringUnderscore()}{fileExtension}"; //UserName_DateTimeNow.jpg gibi

            var path = Path.Combine($"{_wwwRoot}/{imgFolder}/{folderName}", newFileName);

            //Uzantısı ile birlikte dosya ve o dosyaya uygulanacak işlemi belirtiyoruz
            await using (var stream = new FileStream(path, FileMode.Create))
                await pictureFile.CopyToAsync(stream);

            //Resim Tipine göre başarı mesajı değişkenlik gösterecektir
            string message = pictureType == PictureType.User ? $"{name} adlı kullanıcının resmi başarıyla yüklenmiştir" : $"{name} adlı makalenin resmi başarıyla yüklenmiştir";

            return new DataResult<ImageUploadDto>(ResultStatus.Success, new ImageUploadDto
            {
                FullName = $"{folderName}/{newFileName}",
                OldName = oldFileName,
                Extension = fileExtension,
                FolderName = folderName,
                Path = path,
                Size = pictureFile.Length
            }, message);
        }
    }
}
