using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuickProFixer.Services;
using QuickProFixer.ViewModels;
using static QuickProFixer.ViewModels.FSRequestViewModel;

namespace QuickProFixer.Models.Api
{
    [Route("/api/[controller]")]
    [ApiController]
    public class FixerSpaceController : Controller
    {
        private readonly IQuickProFixerRepository _repo;
        public FixerSpaceController(IQuickProFixerRepository repo)
        {
            _repo = repo;
        }


        //Change ResponseCodeStatuses
        // GET auxapi/requestId/senderId/recieversId

        [HttpPost("{notificationId}/{fixerId}/{flagsStringify}/{msg}")]
        public JsonResult POST(int notificationId, int fixerId, string flagsStringify, string msg)
        {

            var flag = JsonConvert.DeserializeObject<ResponseCode>(flagsStringify.ToString());
            var a = fixerId;
            // Fixer: 1 == QuoteSubmitted; 2 == RequestRejected; 3 == RequestNoResponseYet / Default; 4 == FixCompleted
            //Client: 1 ==QuoteSubmitted/Default;  2== QuoteRejected; 3==QuoteNoResponseYet; 4==FixCompleted 5==QuoteAccepted;
            //ActiveFixes: fixer = 1; client=5; //RejectedRequest: fixer = 2; client=1; 

            
            switch (flag)
            {

                case FSRequestViewModel.ResponseCode.FixerMarksAsCompleted:  //Fixer == 4 && Client == 5
                    var response = _repo.UpdateSentNotification(notificationId, 4, 5);
                    if (int.Parse(response) > 0)
                    {
                        return Json(msg);
                    }
                    else
                    {
                        return Json("Update failed...Something went wrong!!");
                    }
                    break;

                case FSRequestViewModel.ResponseCode.RejectRequest:
                    response = _repo.UpdateSentNotification(notificationId, 2, 1); //Fixer == 2 && Client == 1
                    if (int.Parse(response) > 0)
                    {

                        return Json(msg);
                    }
                    else
                    {
                        return Json("Update failed...Something went wrong!!");
                    }
                    break;

                default:
                    return Json("Update failed...Something went wrong!!");
                    break;
            }

        }

    }
}
