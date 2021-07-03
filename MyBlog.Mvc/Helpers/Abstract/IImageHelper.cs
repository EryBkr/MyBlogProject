using Microsoft.AspNetCore.Http;
using MyBlog.Entities.Dtos.FileDtos;
using MyBlog.Shared.Utilities.Results.Abtracts;
using System.Threading.Tasks;

namespace MyBlog.Mvc.Helpers.Abstract
{
    public interface IImageHelper
    {
        Task<IDataResult<ImageUploadDto>> UploadUserImage(string userName,IFormFile pictureFile,string folderName="userImages");

        IDataResult<ImageDeleteDto> Delete(string pictureName);
    }
}
