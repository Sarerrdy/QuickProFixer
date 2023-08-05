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


        /// //Detailed star Rating Properties

        int fiveTotal = 0;
        int fourTotal = 0;
        int threeTotal = 0;
        int twoTotal = 0;
        int oneTotal = 0;

        int fivePercent;
        int fourPercent;
        int threePercent;
        int twoPercent;
        int onePercent;


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
        public async Task<IActionResult> FixerDetails(string msg, int FixerId)
        {
          
            var vm = _repo.GetUserDatails(FixerId);
            
            //Clientele info
            vm.ClienteleUser = _userManager.GetUserAsync(User).Result;
            if (_signInManager.IsSignedIn(User))
            {
                vm.IsClienteleLogin = true;
                vm.IsRated = _repo.IsExpertRatedByCurrentUser(FixerId, vm.ClienteleUser.Id);
            }

           
            if (!string.IsNullOrEmpty(msg))
            {
                vm.CommentAddedNotifier = msg;
            }


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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostStarRatingAndReview(FSFixerDetailsViewModel model, int Id)
        {
            var commenter = await _userManager.GetUserAsync(User);
            string RatingFeedback = "";
            string ReviewFeedBack = "";


            //////////Add Star Rating to dataBase////
            /// 
            if (model.RatingRadio > 0)
            {
                ////Get starRating frm db 
                var StarRatingResult = _repo.GetStarRatingFromDb(Id).ToList();

                Rating starRating = new Rating();
                starRating.UserId = Id;
                starRating.RatingFor = "Experts";
                starRating.RaterId = commenter.Id;
                switch (model.RatingRadio)
                {
                    case 1:
                        starRating.One += 1;
                        break;

                    case 2:
                        starRating.Two += 1;
                        break;

                    case 3:
                        starRating.Three += 1;
                        break;

                    case 4:
                        starRating.Four += 1;
                        break;

                    case 5:
                        starRating.Five += 1;
                        break;
                }

                DetailedStarRatingProcessor(StarRatingResult);
                _repo.AddStarRatingToDB(starRating, oneTotal, twoTotal, threeTotal, fourTotal, fiveTotal);
                RatingFeedback = "Rating";

            }


            //////////Add Comments to dataBase////
            /// 
            if (!string.IsNullOrWhiteSpace(model.Comment))
            {

                Review comment = new Review
                {
                    CommentDate = DateTime.Now.Date,
                    Content = model.Comment,
                    RevieweeID = Id,
                    ReviewerID = commenter.Id
                };

                _repo.AddUserReviewComment(comment);

                ReviewFeedBack = "Comments";
            }

            if (!string.IsNullOrEmpty(RatingFeedback) && !string.IsNullOrEmpty(ReviewFeedBack))
            {
                model.CommentAddedNotifier = "your Rating and Comments were added sucessfully";
            }
            else if (string.IsNullOrEmpty(RatingFeedback) && !string.IsNullOrEmpty(ReviewFeedBack))
            {
                model.CommentAddedNotifier = "your Comments were added sucessfully";
            }
            else if (!string.IsNullOrEmpty(RatingFeedback) && string.IsNullOrEmpty(ReviewFeedBack))
            {
                model.CommentAddedNotifier = "your Rating was summited sucessfully";
            }
            else
            {
                model.CommentAddedNotifier = "Something went wrong!!!";
            }


            return RedirectToAction("FixerDetails", new { msg = model.CommentAddedNotifier, FixerId = Id });
        }

        public void DetailedStarRatingProcessor(List<Rating> StarRatings)
        {
            // var Ratings = repo.GetStarRatingFromDb(RevieweeId).ToList();
            if (StarRatings != null && StarRatings.Count > 0)
            {
                foreach (var p in StarRatings)
                {
                    fiveTotal += p.Five;
                    fourTotal += p.Four;
                    threeTotal += p.Three;
                    twoTotal += p.Two;
                    oneTotal += p.One;
                }

                var total = fiveTotal + fourTotal + threeTotal + twoTotal + oneTotal;

                fivePercent = fiveTotal * 100 / total;
                fourPercent = fourTotal * 100 / total;
                threePercent = threeTotal * 100 / total;
                twoPercent = twoTotal * 100 / total;
                onePercent = oneTotal * 100 / total;
            }
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostMessage(FSFixerDetailsViewModel model, int Id)
        {
            if (ModelState.IsValid)
            {
                var sender = await _userManager.GetUserAsync(User);
                var reciever = _repo.GetUser(Id).FirstOrDefault();

                Message msg = new Message
                {
                    MsgTitle = model.Message.MsgTitle,
                    MsgContent = model.Message.MsgContent,
                    MsgDate = DateTime.Now.Date,
                    RecieverId = reciever.Id,
                    RecieverUserName = reciever.Email,
                    SenderId = sender.Id,
                    SenderUserName = sender.Email
                };



                /////add message to dataBase////
                _repo.AddMessageToDB(msg);

                model.CommentAddedNotifier = "your message was sent sucessfully";


                return RedirectToAction("ExpertDetails", new { msg = model.CommentAddedNotifier, RevieweeId = Id });
            }
            else
            {
                model.CommentAddedNotifier = "something went wrong || your message could not be sent";

                return RedirectToAction("ExpertDetails", new { msg = model.CommentAddedNotifier, RevieweeId = Id });
            }

        }

    }
}
