using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using chic_lighting.Models;
using Microsoft.EntityFrameworkCore;

namespace chic_lighting.Services.CommonService
{
    public class CommonService : ICommonService
    {
        private readonly chic_lightingContext _context;

        public CommonService(chic_lightingContext context)
        {
            _context = context;
        }
        public async Task<string> CreateRandomString(int length)
        {
            Random RNG = new Random();
            var rString = "";
            for (var i = 0; i < length; i++)
            {
                rString += ((char)(RNG.Next(1, 26) + 64)).ToString().ToLower();
            }
            return rString;
        }

        public async Task<string> Hash(string value)
        {
            using (SHA512 shaM = new SHA512Managed())
            {
                if (value == null)
                {
                    return null;
                }
                byte[] bytes = shaM.ComputeHash(Encoding.UTF8.GetBytes(value));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


        public async Task sendEmail(string subject, string body, string to)
        {
            string fromMail = "chicandlighting@gmail.com";
            string fromPassword = "fwvaedqwhkyrefmv";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(to));
            message.Body = body;
            message.IsBodyHtml = false;

            using (var smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential(fromMail, fromPassword);
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(message);
            }
        }

        public async Task updateBestSeller()
        {
            await (from o in _context.Orders
                   join od in _context.OrderDetails
                   on o.Id equals od.OrderId
                   join p in _context.Products
                   on od.ProductId equals p.Id
                   where o.OrderStatusId != 5
                   group p by new { p.Id, p.ProductName }
                                  into g
                   orderby g.Sum(od => od.Quantity)
                   descending
                   select new
                   {
                       Count = g.Sum(od => od.Quantity),
                       ProductName = g.Key.ProductName,
                       ProductId = g.Key.Id
                   })
                                  .Take(4)
                                  .ForEachAsync(async o =>
                                  {
                                      var product = await _context.Products
                                      .Where(p => p.Id == o.ProductId)
                                      .SingleOrDefaultAsync();
                                      product.ProductStatusId = 3;
                                  });
            await _context.SaveChangesAsync();

        }
    }
}
