using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using QuickProFixer.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Xml.Linq;
using QuickProFixer.Models.UtilityModels;
using QuickProFixer.Models;
using QuickProFixer.Services;
using Castle.Core.Internal;

namespace QuickProFixer.Areas.Identity.Pages.Account
{
    public class RegisterAsFixerModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _environment;
        private readonly IQuickProFixerRepository _repo;

        public RegisterAsFixerModel(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole<int>> roleManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IWebHostEnvironment environment,
            IQuickProFixerRepository repo)
        {
            _userManager = userManager;
            _userStore = userStore;
            _roleManager = roleManager;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _environment = environment;
            _repo = repo;

            //SelectCountry = new SelectList(_repo.GetCountries(), nameof(Country.CountryId), nameof(Country.CountryName));
            //SelectState = new SelectList(_repo.GetStates("1"), nameof(State.StateId), nameof(State.StateName));

        }


        [BindProperty(SupportsGet = true)]
        [Required]
        [Display(Name = "Country")]
        public string CountryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CountryName { get; set; }
        public SelectList SelectCountry { get; set; }


        [BindProperty(SupportsGet = true)]
        [Required]
        [Display(Name = "State")]
        public string StateId { get; set; }


        [BindProperty(SupportsGet = true)]
        public string StateName { get; set; }
        public SelectList SelectState { get; set; }

        [BindProperty(SupportsGet = true)]
        [Required]
        [Display(Name = "Local Gov't Area")]
        public string LGAId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string LGAName { get; set; }


        [BindProperty(SupportsGet = true)]
        [Required]
        [Display(Name = " Select your main service type")]
        public int ServiceTypeId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ServiceTypeName { get; set; }
        public SelectList SelectCategories { get; set; }       



        [BindProperty(SupportsGet = true)]
        [Required]
        [Display(Name = "Choose your pricing rate")]
        public int PriceRateTypeId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PriceRateName { get; set; }
        public SelectList SelectPriceRate { get; set; }


        

        [BindProperty]
        public InputModel Input { get; set; }      
        public string ReturnUrl { get; set; }        
        public IList<AuthenticationScheme> ExternalLogins { get; set; }    
        
        /// <summary>
        /// Input Model
        /// </summary>
        public class InputModel
        {
           

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

           
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }


            [Required]
            [Display(Name = "Title")]
            public string Title { get; set; } = "none";

            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }


            [Display(Name = "Middle Name")]
            public string MiddleName { get; set; } = "none";

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; } = "none";

            [Required]
            [Display(Name = "Are you Employee representing an organization on QuickProFixer?")]
            public bool IsUserCompanyRep { get; set; }


            [Display(Name = "Name of your Organisation || Business Name")]
            public string OrganisationName { get; set; } = "none";

            [Required]
            [Display(Name = "Profile Image")]
            public IFormFile FileUpload { get; set; }


            public bool IsFixer { get; set; }

            [Required]
            [Display(Name = "Phone Number")]
            [DataType(DataType.PhoneNumber)]
            public string PhoneNumber { get; set; } = "none";

            /// <summary>
            /// ///contact model
            /// </summary>

            [Required]
            [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [Display(Name = "Address")]

            public string Address { get; set; } = "none";

            [Required]
            [StringLength(30)]
            [Display(Name = "Town")]
            public string Town { get; set; } = "none";

            [StringLength(30)]
            [Display(Name = "Landmark")]
            public string Landmarks { get; set; } = "none";

            [Required]
            [Display(Name = "National Identity Number")]
            public string NIN { get; set; }

            [Required]
            [Display(Name = "Attach NIN")]
            public IFormFile NINFileUpload { get; set; }


            /// <summary>
            /// Fixers unique Entries
            /// </summary>
            /// 
            [Required]
            [StringLength(150)]
            [Display(Name = "Short Description about yourself")]
            public string ShortDescription { get; set; }

            [Required]           
            [Display(Name = "Detailed Description about yourself and your work")]
            public string DetailedDescription { get; set; }


            [Required]
            [Display(Name = "Are you the manager of your Organization/Business QuickProFixer Account?")]
            public bool IsBusinessAccountManager { get; set; }

            [Required]
            [Display(Name = "Are you the Business Owner/CEO?")]
            public bool IsBusinessOwner { get; set; }



            [Required]
            [Display(Name = "Coperate Affairs Commission (CAC) Registration Number")]
            public string CACNumber { get; set; }

            [Required]
            [Display(Name = "Attach CAC Registration Certificate")]
            public IFormFile CACFileUpload { get; set; }

            [Required]
            [Display(Name = "CAC Registration Expiry Date")]
            [DataType(DataType.Date)]
            public DateTime CACExpiryDate { get; set; } = DateTime.Now.Date;

            [Required]
            [StringLength(50)]
            [Display(Name = "Primary Skill")]
            public string PrimarySkills { get; set; }

            [StringLength(50)]
            [Display(Name = "Other Skills. Seperate multiple skills with a comma(,)")]
            public string OtherSkills { get; set; }

            [Required]
            [Display(Name = "Starting price (₦)")]
            public int StartingPrice { get; set; }


        }




        public void OnGet(string returnUrl = null)
        {

            //// CountryId = "1";        
            SelectCountry = new SelectList(_repo.GetCountries(), nameof(Country.CountryId), nameof(Country.CountryName));
            SelectState = new SelectList(_repo.GetStates("2"), nameof(State.StateId), nameof(State.StateName));
            SelectCategories = new SelectList(_repo.GetCategories(null), nameof(ServiceType.ServiceTypeId), nameof(ServiceType.ServiceTypeName));
            SelectPriceRate = new SelectList(_repo.GetPriceRate(null), nameof(PriceRateType.PriceRateTypeId), nameof(PriceRateType.PriceRateName));


            ReturnUrl = returnUrl;
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public JsonResult OnGetLocalGovtAreas()
        {
            JsonResult result = new JsonResult(_repo.GetLGAs(StateId));
            return result;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {
                returnUrl ??= Url.Content("~/");
                // ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                if (ModelState.IsValid)
                {

                    string profileImgUrl;
                    if (Input.FileUpload != null)
                    {
                        profileImgUrl = ResourceUploader.GetUniqueFileName(Input.FileUpload.FileName);
                        await ResourceUploader.UploadFile(Input.FileUpload, profileImgUrl, _environment, FileCategory.UsersProfile);
                    }
                    else
                    {
                        profileImgUrl = "defaultAvater.png";
                    }


                    string NinImgUrl;
                    if (Input.NINFileUpload != null)
                    {
                        NinImgUrl = ResourceUploader.GetUniqueFileName(Input.NINFileUpload.FileName);
                        await ResourceUploader.UploadFile(Input.NINFileUpload, NinImgUrl, _environment, FileCategory.NinImages);
                    }
                    else
                    {
                        NinImgUrl = "defaultNINAvater.png";
                    }

                    string CACUrl;
                    if (Input.CACFileUpload != null)
                    {
                        CACUrl = ResourceUploader.GetUniqueFileName(Input.CACFileUpload.FileName);
                        await ResourceUploader.UploadFile(Input.CACFileUpload, CACUrl, _environment, FileCategory.CACImages);
                    }
                    else
                    {
                        CACUrl = "defaultNINAvater.png";
                    }



                    var user = CreateUser();

                    user.UserName = Input.Email;
                    user.Email = Input.Email;
                    user.FirstName = Input.FirstName;
                    user.LastName = Input.LastName;
                    user.MiddleName = Input.MiddleName;
                    user.PhoneNumber = Input.PhoneNumber;
                    user.Title = Input.Title;
                    user.JoinDate = DateTime.Now;
                    user.IsFixer = true;
                    user.ProfileImgUrl = profileImgUrl;
                    user.NIN = Input.NIN;
                    user.NINUrl = NinImgUrl;
                    user.IsCompanyRegistrationVerified = false;
                    user.IsEmployeeStatusVerified = false;
                    user.IsNINVerified = false;
                    //user.UserId = user.Id;
                    user.IsUserCompanyRep = Input.IsUserCompanyRep;
                    user.OrganisationName = Input.OrganisationName;
                    user.Contact = new Contact
                    {
                        Address = Input.Address,
                        Town = Input.Town,
                        LgaId = int.Parse(LGAId),
                        LGA = LGAName,
                        StateId = int.Parse(StateId),
                        State = StateName,
                        CountryId = int.Parse(CountryId),   
                        Country = CountryName
                    };

                    user.Profile = new FixerProfile { 
                      IsBusinessAccountManager = Input.IsBusinessAccountManager,
                      IsBusinessOwner = Input.IsBusinessOwner,
                      ShortDescription = Input.ShortDescription,
                      DetailedDescription = Input.DetailedDescription,
                      ServiceTypeId = ServiceTypeId,
                      ServiceType = ServiceTypeName,
                      CACNumber = Input.CACNumber,
                      CACUrl = CACUrl,
                      CACExpiryDate = Input.CACExpiryDate,
                      OtherSkills = Input.OtherSkills,
                      StartingPrice = Input.StartingPrice,
                      PriceRateTypeId = PriceRateTypeId,
                      PriceRateName = PriceRateName,
                     // PrimarySkills = Input.PrimarySkills
                    };




                    await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                    var result = await _userManager.CreateAsync(user, Input.Password);

                    ///////Create Role//// 
                    
                    var role = new IdentityRole<int>{Name = "Fixer" };
                    //bool checkRole = _roleManager.Roles.Where(p => p.Name == role.Name).IsNullOrEmpty();
                    //await _roleManager.CreateAsync(role);
                    //if (_roleManager.Roles.Where(p => p.Name == role.Name).IsNullOrEmpty())
                    //{
                    //   await _roleManager.CreateAsync(role);
                    //}



                    if (result.Succeeded)
                    {

                        await _userManager.AddToRoleAsync(user, role.Name); 
                        _logger.LogInformation("User created a new account with password.");

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Page(
                            "/Account/RegistrationConfirmation",
                            pageHandler: null,
                            values: new { userId = user.Id, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }

                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                //If we got this far, something failed, redisplay form
                return Page();
            }
            catch
            {
                //If we got this far, something failed, redisplay form
                SelectCountry = new SelectList(_repo.GetCountries(), nameof(Country.CountryId), nameof(Country.CountryName));
                SelectState = new SelectList(_repo.GetStates("2"), nameof(State.StateId), nameof(State.StateName));
                return Page();
            }

        }

        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                    $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
    }
}