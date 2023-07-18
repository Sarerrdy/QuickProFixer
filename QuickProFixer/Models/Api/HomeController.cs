using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using NuGet.Protocol.Plugins;
using QuickProFixer.Models.Entities;
using QuickProFixer.Models.UtilityModels;
using QuickProFixer.Services;
using QuickProFixer.ViewModels;
using System.Text.Json;
using static QuickProFixer.ViewModels.FSRequestViewModel;

namespace QuickProFixer.Models.Api
{
    [Route("Home/api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly IQuickProFixerRepository _repo;
        public HomeController(IQuickProFixerRepository repo)
        {
            _repo = repo;
        }



        ///Send request notification to individual fixers 
        // GET auxapi/requestId/senderId/recieversId

        [HttpPost("{requestId}/{fixer}/{notificationStringify}")]
        public async Task<JsonResult> POST(int requestId, string fixer, string notificationStringify)
        {

            var sentNotification = JsonConvert.DeserializeObject<SentNotification>(notificationStringify);
            var request = _repo.GetSingleRequest(requestId);


            if (sentNotification != null && request != null)
            {
                sentNotification.Request = request;

                try
                {
                    var SentRequestFeedBack = _repo.CreateSingleSentNotification(sentNotification);
                    if (int.Parse(SentRequestFeedBack) > 0)
                    {
                        return Json("Request was sent successfully sent to " + fixer);
                    }
                    else
                    {
                        _repo.DeleteRequest(request);
                        return Json("Something went wrong... Request Notification could not be sent");

                    }
                }
                catch
                {
                    _repo.DeleteRequest(request);
                    return Json("Something went wrong... Request Notification could not be sent"); ;
                }
            }
            return Json("Something went wrong... Request Notification could not be sent"); ;
        }



        ///Send request notification to many fixers 
        // GET auxapi/requestId/senderId/responseCodesStringify/fixer

        [HttpPost("{requestId}/{sentNotificationsStringify}")]
        public JsonResult POST(int requestId, string sentNotificationsStringify)
        {      


            var sentNotificationsList = JsonConvert.DeserializeObject<List<SentNotification>>(sentNotificationsStringify);
            var request = _repo.GetSingleRequest(requestId);

            if(sentNotificationsList!= null &&  sentNotificationsList.Count > 0 && request!=null)
            {
                foreach (var notification in sentNotificationsList)
                {
                    notification.Request = request; 
                    //request.SentNotifications.Append().ToList().Add(notification);
                }

                try
                {
                    var SentRequestFeedBack = _repo.CreateSentNotifications(sentNotificationsList);
                    if (int.Parse(SentRequestFeedBack) > 0)
                    {
                        return Json("Request was sent successfully");
                    }
                    else
                    {
                        _repo.DeleteRequest(request);
                        return Json("Something went wrong... Request Notification could not be sent");                        

                    }
                }
                catch {
                    _repo.DeleteRequest(request);
                    return Json("Something went wrong... Request Notification could not be sent"); ;
                }
            }
            return Json("Something went wrong... Request Notification could not be sent"); ;
        }


        

    }
}
