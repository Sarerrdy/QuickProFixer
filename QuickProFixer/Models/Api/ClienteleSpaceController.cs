using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuickProFixer.Services;
using QuickProFixer.ViewModels;
using static QuickProFixer.ViewModels.FSRequestViewModel;

namespace QuickProFixer.Models.Api
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ClienteleSpaceController : Controller
    {
        private readonly IQuickProFixerRepository _repo;
        public ClienteleSpaceController(IQuickProFixerRepository repo)
        {
            _repo = repo;
        }

        //Change ResponseCodeStatuses
        // GET auxapi/requestId/senderId/recieversId

        //[HttpPost("{notificationId}/{fixerId}/{flagsStringify}/{msg}")]
        //public JsonResult POST(int notificationId, int fixerId, string flagsStringify, string msg)
        //{

        //    var flag = JsonConvert.DeserializeObject<ResponseCode>(flagsStringify.ToString());
        //    var a = fixerId;
        //    // Fixer: 1 == QuoteSubmitted; 2 == RequestRejected; 3 == RequestNoResponseYet / Default; 4 == FixCompleted
        //    //Client: 1 ==QuoteSubmitted/Default;  2== QuoteRejected; 3==QuoteNoResponseYet; 4==FixCompleted 5==QuoteAccepted;
        //    //ActiveFixes: fixer = 1; client=5; //RejectedRequest: fixer = 2; client=1; 


        //    switch (flag)
        //    {

        //        case FSRequestViewModel.ResponseCode.ClienteleMarksFixAsActive:  //Fixer == 1 && Client == 5
        //            var response = _repo.UpdateSentNotification(notificationId, 1, 5);
        //            if (int.Parse(response) > 0)
        //            {
        //                return Json(msg);
        //            }
        //            else
        //            {
        //                return Json("Update failed...Something went wrong!!");
        //            }
        //            break;

        //        case FSRequestViewModel.ResponseCode.RejectQuote:
        //            response = _repo.UpdateSentNotification(notificationId, 1, 2); //Fixer == 1 && Client == 2
        //            if (int.Parse(response) > 0)
        //            {

        //                return Json(msg);
        //            }
        //            else
        //            {
        //                return Json("Update failed...Something went wrong!!");
        //            }
        //            break;

        //        case FSRequestViewModel.ResponseCode.ClieteleMarksAsCompleted:
        //            response = _repo.UpdateSentNotification(notificationId, 1, 4); //Fixer == 1 && Client == 4
        //            if (int.Parse(response) > 0)
        //            {

        //                return Json(msg);
        //            }
        //            else
        //            {
        //                return Json("Update failed...Something went wrong!!");
        //            }
        //            break;

        //        default:
        //            return Json("Update failed...Something went wrong!!");
        //            break;
        //    }

        //}






        [HttpPost("{notificationId}/{fixerId}/{flagsStringify}/{msg}/{requestId}/{quoteId}")]
        public JsonResult POST(int notificationId, int fixerId, string flagsStringify, string msg, int requestId, int quoteId)
        {

            var flag = JsonConvert.DeserializeObject<ResponseCode>(flagsStringify.ToString());
            //var a = fixerId;
            // Fixer: 1 == QuoteSubmitted; 2 == RequestRejected; 3 == RequestNoResponseYet / Default; 4 == FixCompleted
            //Client: 1 ==QuoteSubmitted/Default;  2== QuoteRejected; 3==QuoteNoResponseYet; 4==FixCompleted 5==QuoteAccepted;
            //ActiveFixes: fixer = 1; client=5; //RejectedRequest: fixer = 2; client=1; 


            switch (flag)
            {

                case FSRequestViewModel.ResponseCode.ClienteleMarksFixAsActive:  //Fixer == 1 && Client == 5
                    var response = _repo.UpdateSentNotification(notificationId, 1, 5);
                    var isContracted = _repo.UpdateRequestIsContractedStatus(true, false, requestId, fixerId, quoteId);
                   

                    if (int.Parse(response) > 0 && int.Parse(isContracted) > 0)
                    {
                        return Json(msg);
                    }
                    else
                    {
                        return Json("Update failed...Something went wrong!!");
                    }
                    break;

                case FSRequestViewModel.ResponseCode.RejectQuote:
                    response = _repo.UpdateSentNotification(notificationId, 1, 2); //Fixer == 1 && Client == 2
                    if (int.Parse(response) > 0)
                    {

                        return Json(msg);
                    }
                    else
                    {
                        return Json("Update failed...Something went wrong!!");
                    }
                    break;

                case FSRequestViewModel.ResponseCode.ClieteleMarksAsCompleted:
                    response = _repo.UpdateSentNotification(notificationId, 1, 4); //Fixer == 1 && Client == 4
                    isContracted = _repo.UpdateRequestIsContractedStatus(true, true, requestId, fixerId,quoteId);

                    if (int.Parse(response) > 0 && int.Parse(isContracted) > 0)
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
