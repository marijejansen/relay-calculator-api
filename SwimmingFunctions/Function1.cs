using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace SwimmingFunctions
{
    public class ClubRecords
    {
        //[FunctionName("CheckForClubRecords")]
        //public void Run([TimerTrigger("0 0 0 * * *")]TimerInfo myTimer, ILogger log)
        //{
        //    log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        //}

        [FunctionName("CheckForClubRecords")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("marije.wz@zpcamersfoort.nl");
            mailMessage.To.Add("marije.jansen@mailbox.org");
            mailMessage.Subject = "Test";
            mailMessage.Body = "Dit is een test";

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "outlook.office365.com";
            smtpClient.Port = 993;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("marije.wz@zpcamersfoort.nl", "0CeYCs4*b%!KpA");
            smtpClient.EnableSsl = true;
            

            smtpClient.Send(mailMessage);

            return new OkObjectResult("gelukt");
        }
    }
}
