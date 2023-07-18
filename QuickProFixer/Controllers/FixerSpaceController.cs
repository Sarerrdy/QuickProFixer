using Microsoft.AspNetCore.Mvc;
using QuickProFixer.Models.Entities;
using QuickProFixer.Services;
using QuickProFixer.ViewModels;
using QuickProFixer.Models.UtilityModels;
using Newtonsoft.Json;
using QuickProFixer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace QuickProFixer.Controllers
{

   
    public class FixerSpaceController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IQuickProFixerRepository _repo;
        private readonly ILogger<HomeController> _logger;       
        private readonly IWebHostEnvironment _environment;

        string feedback = "";
        bool isReloading = true;

        public FixerSpaceController(ILogger<HomeController> logger, IQuickProFixerRepository repo, IWebHostEnvironment environment, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _logger = logger;           
            _environment = environment;
            _userManager = userManager;
            _signInManager = signInManager;
            _repo = repo;            
        }

        

        ///Populate Fixer space index page
        [Authorize(Roles = "Fixer")]       
        public IActionResult Index(int FixerId)
        {
            if (FixerId == 0){ FixerId = _userManager.GetUserAsync(User).Result.Id; }

            var vm = _repo.GetFSDashboardByFixerId(FixerId);

            
            return View(vm);
        }


        [Authorize]
        public ViewResult FixerDetails(int FixerId)
        {
            if (FixerId == 0)
            {
                FixerId = _userManager.GetUserAsync(User).Result.Id;
            }
            var user = _repo.GetUser(FixerId);
            FSFixerDetailsViewModel vm = new FSFixerDetailsViewModel();
            vm.FixerId = user.FirstOrDefault().Id;
            vm.FirstName = user.FirstOrDefault().FirstName;
            return View(vm);
        }


        ///Populate page of Quotes with opened request
        [Authorize(Roles = "Fixer")]       
        public IActionResult QuotesWithOpenRequest(int FixerId)
        {
            if (FixerId == 0) { FixerId = _userManager.GetUserAsync(User).Result.Id; }

            var vm = _repo.GetFSQuotesWithOpenedRequest(FixerId);        
            
           
            return View(vm);
        }


        ///Populate page of Quotes with closed request
        [Authorize(Roles = "Fixer")]
        public IActionResult QuotesWithClosedRequest(int FixerId)
        {
            if (FixerId == 0){ FixerId = _userManager.GetUserAsync(User).Result.Id; }

            var vm = _repo.GetFSQuotesWithClosedRequest(FixerId);

            
            return View(vm);
        }


        ///Populate Fixer space Active Fixes page
        [Authorize(Roles = "Fixer")]        
        public IActionResult ActiveFixes(int FixerId)
        {
            if (FixerId == 0) { FixerId = _userManager.GetUserAsync(User).Result.Id; }

            var vm = _repo.GetFSActiveFixesByFixerId(FixerId);         
            return View(vm);
        }

        [Authorize(Roles = "Fixer")]
        ///Populate Fixer space with list of completed Fixes
        public IActionResult CompletedFixes(int FixerId)
        {
            if (FixerId == 0) { FixerId = _userManager.GetUserAsync(User).Result.Id; }

            var vm = _repo.GetFSFixesCompletedFixesByFixerId(FixerId);
           
            return View(vm);
        }


        [Authorize(Roles = "Fixer")]
        ///Populate Fixer space with Rejected Quotes
        public IActionResult RejectedQuotes(int FixerId)
        {
            if (FixerId == 0) { FixerId = _userManager.GetUserAsync(User).Result.Id; }

            var vm = _repo.GetFSFixesRejectedQuotesByFixerId(FixerId);           
            return View(vm);
        }


        [Authorize(Roles = "Fixer")]
        ///Populate Fixer space with Rejected Request
        public IActionResult RejectedRequest(int FixerId)
        {
            if (FixerId == 0) { FixerId = _userManager.GetUserAsync(User).Result.Id; }

            var vm = _repo.GetFSFixesRejectedRequestByFixerId(FixerId);
           
           
            return View(vm);
        }


        [Authorize(Roles = "Fixer")]
        ///Load the createQuote empty form
        public IActionResult CreateQuote(int RequestId)
        {
           
            var FixerId = _userManager.GetUserAsync(User).Result.Id;
            

            var vm = _repo.CreateQuoteViewModel(RequestId, FixerId);
            return View(vm);
        }


        [Authorize(Roles = "Fixer")]
        ///Submit the createQuote form
        [HttpPost]
        public async Task<IActionResult> CreateQuote(CreateQuoteViewModel model)
        {
            try
            {
                var AttachedResourcesUriList = new List<IFormFile>
                {
                    model.QuoteImgUrl_1,
                    model.QuoteImgUrl_2,
                    model.QuoteImgUrl_3,
                    model.QuotePdfUrl,
                    model.QuoteVideoUrl,
                    model.QuoteAudioUrl
                };


                string newName, QuoteImgUrl_1 = "", QuoteImgUrl_2 = "", QuoteImgUrl_3 = "", PdfUrl = "", VideoUrl = "", AudioUrl = "";
                foreach (var file in AttachedResourcesUriList)
                {
                    if (file != null)
                    {
                        newName = ResourceUploader.GetUniqueFileName(file.FileName);

                        if (file != null && file.ContentType.Contains("image"))
                        {
                            await ResourceUploader.UploadFile(file, newName, _environment, FileCategory.RequestImages);
                            //  var fileIndex = AttachedResourcesUriList.IndexOf(file);
                            if (file.Name.Equals("QuoteImgUrl_1"))
                            {
                                QuoteImgUrl_1 = newName;
                            }
                            else if (file.Name.Equals("QuoteImgUrl_2"))
                            {
                                QuoteImgUrl_2 = newName;
                            }
                            else if (file.Name.Equals("QuoteImgUrl_3"))
                            {
                                QuoteImgUrl_3 = newName;
                            }
                        }
                        else if (file != null && file.ContentType.Contains("video"))
                        {
                            await ResourceUploader.UploadFile(file, newName, _environment, FileCategory.RequestVideos);
                            VideoUrl = newName;
                        }

                        else if (file != null && file.ContentType.Contains("audio"))
                        {
                            await ResourceUploader.UploadFile(file, newName, _environment, FileCategory.RequestAudio);
                            AudioUrl = newName;
                        }

                        else if (file != null && file.ContentType.Contains("pdf"))
                        {
                            await ResourceUploader.UploadFile(file, newName, _environment, FileCategory.RequestPDF);
                            PdfUrl = newName;

                        }
                    }
                }


                List<Material> materiallist = JsonConvert.DeserializeObject<List<Material>>(model.Materials);
                var firstMaterial = new Material
                {
                    Name = model.ItemName,
                    Quantity = model.ItemQuantity,
                    CostPerItem = model.ItemCostPerItem,
                    Cost = model.ItemCost
                };
                materiallist.Add(firstMaterial);


                // FixerResponsecode == 1 -- Quote ubmitted && ClientResponseCode == 1 ---Request subited
                var req = _repo.GetSingleRequest(model.RequestId);

                var sentnotification = req.SentNotifications.FirstOrDefault(p => p.ResponseStatuses.RecieverId == model.FixerId);
                sentnotification.ResponseStatuses.FixerStatuseCode = 1;
                sentnotification.ResponseStatuses.ClientStatuseCode = 1;



                var newQuote = new Quote
                {
                    FixerId = model.FixerId,
                    FixerName = model.FixerFirstNames + "  " + model.FixerLastName,
                    RequestId = model.RequestId,// 1,
                    ClienteleId = model.ClienteleId,//1,
                    Title = "RE: " + model.Title,
                    Description = model.QuoteDescription,
                    DateofQuote = DateTime.Now.Date,
                    ProposedStartDate = model.QuoteProposedStartDate,
                    EstimatedFixDuration = model.QuoteEstimatedFixDuration,
                    LabourRate = model.LabourRate,
                    LabourCost = model.QuoteEstimatedLabourCost,
                    EstimatedCostOfMaterials = model.QuoteEstimatedCostOfMaterials,
                    EstimatedOverallCost = model.QuoteEstimatedOverallCost,
                    Request = req,
                    SentNotification = sentnotification,


                    StartUrgency = "Urgent",//model.Urgency,

                    Materials = materiallist,

                    AudioUrl = AudioUrl,
                    VideoUrl = VideoUrl,
                    PdfUrl = PdfUrl,
                    ImgUrl_1 = QuoteImgUrl_1,
                    ImgUrl_2 = QuoteImgUrl_2,
                    ImgUrl_3 = QuoteImgUrl_3,

                   

                };

                _repo.PostQuote(newQuote);

                ///////Redirect Request to the Fixers Index page that contains a collection of Fix Request awaiting the fixer 
                ///to submit a quote
                ///
                return RedirectToAction("QuotePreview", new { newQuote.QuoteId });

            }
            catch (Exception ex)
            {
                model.Feedback = "Something went wrong" + Environment.NewLine + ex.Message;
                model.IsPageReloading = true;

                return View(model);
            }
        }
       
        
        [Authorize]
        public async Task<IActionResult> QuotePreview (int QuoteId)
        {
            var vm = _repo.CreateQuotePreview(QuoteId);

            //if (_userManager.GetUserAsync(User).Result.Id == vm.ClienteleId)
            //{
            //    vm.IsCurrentUserClientele = true;
            //}
            return View(vm);
        }

    }
}
