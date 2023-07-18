using QuickProFixer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickProFixer.Models.Entities;
using QuickProFixer.Models.UtilityModels;
using QuickProFixer.Services;
using QuickProFixer.ViewModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace QuickProFixer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQuickProFixerRepository _repo;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<User> _userManager;

        SelectList slCategory;
        SelectList slCountry;

        string feedback="";
        bool isReloading = true;
        public HomeController(ILogger<HomeController> logger, IQuickProFixerRepository repo, IWebHostEnvironment environment, UserManager<User> userManager)
        {
            _logger = logger;
            _repo = repo;
            _environment = environment;
            _userManager = userManager;


            slCategory = new SelectList(_repo.GetCategories(null), nameof(ServiceType.ServiceTypeId), nameof(ServiceType.ServiceTypeName));
            slCountry = new SelectList(_repo.GetCountries(), nameof(Country.CountryId), nameof(Country.CountryName));
        }
     
        public IActionResult Index()
        {
            HomeIndexVm vm = new();
            vm.IsPageReloading = isReloading;
            vm.Feedback = feedback;
            vm.SelectCategories = slCategory;
            vm.SelectCountries = slCountry;
           // string url = $"%2Home%2FIndex%2Fmodel?={vm}";
            vm.ReturnUrl = Url.Content($"~/home/Index/model?={vm}");
            return View(vm);
        }

        [HttpGet]
        public IActionResult Index(string feedback, bool isReload)
        {
            HomeIndexVm vm = new();
            vm.IsPageReloading = isReload;
            vm.Feedback = feedback;
            vm.SelectCategories = slCategory;
            vm.SelectCountries = slCountry;

            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Index(HomeIndexVm model)
        {
           
            try
            {
                if (!ModelState.IsValid)
                {
                    model.SelectCategories = slCategory;
                    model.SelectCountries = slCountry;
                    return View(model);
                }

                var AttachedResourcesUriList = new List<IFormFile>
                {
                    model.ImgUrl_1,
                    model.ImgUrl_2,
                    model.ImgUrl_3,
                    model.PdfUrl,
                    model.VideoUrl,
                    model.AudioUrl                       
                };

                string newName, ImgUrl_1="", ImgUrl_2="", ImgUrl_3="", PdfUrl="", VideoUrl="", AudioUrl="";
                foreach (var file in AttachedResourcesUriList)
                {
                    if(file != null)
                    {
                        newName = ResourceUploader.GetUniqueFileName(file.FileName);

                        if (file != null && file.ContentType.Contains("image"))
                        {
                            await ResourceUploader.UploadFile(file, newName, _environment, FileCategory.RequestImages);
                            //  var fileIndex = AttachedResourcesUriList.IndexOf(file);
                            if (file.Name.Equals("ImgUrl_1"))
                            {
                                ImgUrl_1 = newName;
                            }
                            else if (file.Name.Equals("ImgUrl_2"))
                            {
                                ImgUrl_2 = newName;
                            }
                            else if (file.Name.Equals("ImgUrl_3"))
                            {
                                ImgUrl_3 = newName;
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


                var currUser = await _userManager.GetUserAsync(User);

                var newRequest = new Request
                {
                    ClienteleId = currUser.Id, ///current client making the request
                    ClientName = currUser.FirstName + " " + currUser.LastName,
                    Title = model.Request.Title,
                    Description = model.Request.Description,
                    ServiceTypeId = model.Request.ServiceTypeId,
                    ServiceType = model.Request.ServiceType,
                    DateOfRequest = DateTime.Now.Date,
                    PreferredStartDate = model.PreferredStartDate.ToShortDateString(),
                    AudioUrl = AudioUrl,
                    VideoUrl = VideoUrl,
                    PdfUrl = PdfUrl,
                    CountryId = model.Request.CountryId,
                    CountryName = model.Request.CountryName,
                    StateId = model.Request.StateId,
                    StateName = model.Request.StateName,
                    LGAId = model.Request.LGAId,
                    LGAName = model.Request.LGAName,
                    Town = model.Request.Town,
                    Landmarks = model.Request.Landmarks,
                    Address = model.Request.Address,
                    Location = model.Request.Address + ", " + model.Request.Landmarks + ", " + model.Request.LGAName + ", " + model.Request.StateName + ", " + model.Request.CountryName,
                    ImgUrl_1 = ImgUrl_1,
                    ImgUrl_2 = ImgUrl_2,
                    ImgUrl_3 = ImgUrl_3

                };

                _repo.PostRequest(newRequest);               

                ///////Redirect Request to the SendRequest page that contains a collection of fixer 
                ///closed to the request Loaction
                ///
                return RedirectToAction("SendRequest",new { lgaName = model.Request.LGAName, serviceId = model.Request.ServiceTypeId,
                    requestId = newRequest.RequestId, requestTitle = newRequest.Title, senderId = newRequest.ClienteleId });

            }
            catch (Exception ex)
            {
                model.Feedback = "Something went wrong" + Environment.NewLine + ex.Message;
                model.IsPageReloading = true;
               
                return View(model);
            }
        }

        ////Fetch Fixers closed to the Fix/Request Location so that Request notification can be sent to them
        public IActionResult SendRequest(string lgaName, int serviceId, int requestId, string requestTitle, int senderId)
        {
            var fsVmodel = _repo.GetFixersFromClosedProximity(lgaName, serviceId, requestId, requestTitle, senderId);

            return View(fsVmodel);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}