using MyBlog.Entities.Dtos.EmailDtos;
using MyBlog.Shared.Utilities.Results.Abtracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Services.Abstract
{
    public interface IMailService
    {
        //Genel Gönderim
        IResult Send(EmailSendDto emailSendDto);

        //İletişim kısmı
        IResult SendContactEmail(EmailSendDto emailSendDto);
    }
}
