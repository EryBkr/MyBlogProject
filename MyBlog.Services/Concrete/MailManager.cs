using Microsoft.Extensions.Options;
using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos.EmailDtos;
using MyBlog.Services.Abstract;
using MyBlog.Shared.Utilities.Results.Abtracts;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using MyBlog.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Services.Concrete
{
    public class MailManager : IMailService
    {
        private readonly SmtpSettings _smptSettings;

        //appsettings.json dosyamızda saklamış olduğumuz smtp bilgilerini IOptions yardımıyla alıyoruz
        public MailManager(IOptions<SmtpSettings> smtpSettings)
        {
            _smptSettings = smtpSettings.Value;
        }

        /// <summary>
        /// Client tarafına ulaşması gereken maillerden sorumlu metot (şifremi unuttum,abonelik bilgileri vs...)
        /// </summary>
        /// <param name="emailSendDto"></param>
        /// <returns></returns>
        public IResult Send(EmailSendDto emailSendDto)
        {
            MailMessage message = new MailMessage
            {
                From = new MailAddress(_smptSettings.SenderEmail),
                To = { new MailAddress(emailSendDto.Email) },
                Subject = emailSendDto.Subject, //Şifremi unuttum
                IsBodyHtml = true, //Template ile gelmesini kabul ediyor muyuz ?
                Body =emailSendDto.Message //Yeni Şifreniz : bla bla bla
            };

            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smptSettings.Server,
                Port = _smptSettings.Port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smptSettings.UserName, _smptSettings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            smtpClient.Send(message);

            return new Result(resultStatus: ResultStatus.Success, "E-Postanız Başarıyla Gönderilmiştir");
        }

        /// <summary>
        /// İletişim kısmından dolduran maillerin bize gelmesini sağlar 
        /// </summary>
        /// <param name="emailSendDto"></param>
        /// <returns></returns>
        public IResult SendContactEmail(EmailSendDto emailSendDto)
        {
            MailMessage message = new MailMessage
            {
                From = new MailAddress(_smptSettings.SenderEmail),
                To = { new MailAddress("crazyeray94@gmail.com") },
                Subject = emailSendDto.Subject,
                IsBodyHtml = true, //Template ile gelmesini kabul ediyor muyuz ?
                Body = $"Gönderen Kişi: {emailSendDto.Name}, Gönderen E-Postası: {emailSendDto.Email} <br/> {emailSendDto.Message}"
            };

            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smptSettings.Server,
                Port = _smptSettings.Port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smptSettings.UserName, _smptSettings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            smtpClient.Send(message);

            return new Result(resultStatus: ResultStatus.Success, "E-Postanız Başarıyla Gönderilmiştir");
        }
    }
}
