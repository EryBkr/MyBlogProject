using Microsoft.AspNetCore.Http;
using MyBlog.Entities.ComplexTypes;
using MyBlog.Entities.Dtos.FileDtos;
using MyBlog.Shared.Utilities.Results.Abtracts;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Helpers.Abstract
{
    public interface IImageHelper
    {
        Task<IDataResult<ImageUploadDto>> UploadImage(string name,PictureType pictureType,IFormFile pictureFile,string folderName=null);

        IDataResult<ImageDeleteDto> Delete(string pictureName);
    }
}
