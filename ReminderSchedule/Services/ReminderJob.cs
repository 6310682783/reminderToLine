using Microsoft.Extensions.Logging;
using Quartz;
using ReminderSchedule.Models;
using ReminderSchedule.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReminderSchedule.Services
{
    public class  ReminderJob : IJob
    {
        private readonly ILogger<ReminderJob> _logger;
        //private readonly IReminderRepository _reminderRepository;
        public ReminderJob(ILogger<ReminderJob> logger/* ,IReminderRepository reminderRepository*/)
        {
            _logger = logger;
            //_reminderRepository = reminderRepository;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Hello world!");

            
            //var testget = _reminderRepository.GetAll();
            var testline = SentNotify();
            

            return Task.CompletedTask;
        }
        public async Task<bool> SentNotify()
        {
            bool isSendSuscess = false;
            try
            {
                string message = "hellotest";
                string strMessage = System.Web.HttpUtility.UrlEncode(message, Encoding.UTF8);
                var request = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");

                //setting proxy
                //WebProxy webProxy = new WebProxy(PROXY_URL, true)
                //{
                //    Credentials = new NetworkCredential(PROXY_USERNAME, PROXY_PASSWORD),
                //    UseDefaultCredentials = true
                //};
                //request.Proxy = webProxy;

                var postData = string.Format("message={0}", strMessage);
                var data = Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + "xWZmw6Ei6c4NLNHQkFvsXyxx0J7DgUtn6z5lakL3yYa");
                var stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                var response = await request.GetResponseAsync() as HttpWebResponse;
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                    isSendSuscess = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return isSendSuscess;
        }
    }
}
