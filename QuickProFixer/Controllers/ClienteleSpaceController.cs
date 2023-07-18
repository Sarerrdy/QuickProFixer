using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using QuickProFixer.Models.Entities;
using QuickProFixer.Services;
using QuickProFixer.ViewModels;

namespace QuickProFixer.Controllers
{
    [Authorize]
    public class ClienteleSpaceController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IQuickProFixerRepository _repo;

        public ClienteleSpaceController(IQuickProFixerRepository repo, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repo = repo;            
        }

       
        public IActionResult Index(int clienteleId)
        {
            if(clienteleId == 0) { clienteleId = _userManager.GetUserAsync(User).Result.Id; }

            var vm = _repo.GetRequestByClientId(clienteleId);
            return View(vm);
        }

       
        //public IActionResult ReceivedQuotes(int clienteleId)
        //{
        //    if (clienteleId == 0)
        //    {
        //        clienteleId = _userManager.GetUserAsync(User).Result.Id;
        //    }

        //    var vm = _repo.GetCSReceivedQuotesByClienteleId(clienteleId);
        //    return View(vm);

        //    //var vm = _repo.GetRequestByClientId(clienteleId);
        //    //return View(vm);
        //}


        public IActionResult RequestDetails(int RequestId)
        {
            var vm = _repo.GetRequestById(RequestId);
            return View(vm);
        }

        public IActionResult RequestPreview(int RequestId)
        {
            var vm = _repo.GetRequestPreviewById(RequestId);
            return View(vm);
        }

        public IActionResult ActiveRequests(int clienteleId)
        {
            if (clienteleId == 0) { clienteleId = _userManager.GetUserAsync(User).Result.Id; }

            var vm = _repo.GetActiveRequestByClientId(clienteleId);
            return View(vm);
        }

        
       


        ///Populate clientele space with list of completed Fixes
        public IActionResult CompletedRequest(int clienteleId)
        {
            if (clienteleId == 0){ clienteleId = _userManager.GetUserAsync(User).Result.Id; }

            var vm = _repo.GetCompletedFixesByClientId(clienteleId);            
            return View(vm);
        }

        public IActionResult ActiveRequestDetails(int RequestId)
        {
            var vm = _repo.GetActiveRequestDetailsById(RequestId);
            return View(vm);
        }

    }
}
