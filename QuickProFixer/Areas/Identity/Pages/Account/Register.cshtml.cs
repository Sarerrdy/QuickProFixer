﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using QuickProFixer.Models.Entities;

namespace QuickProFixer.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        //private readonly SignInManager<User> _signInManager;
        //private readonly UserManager<User> _userManager;
        //private readonly IUserStore<User> _userStore;
        //private readonly IUserEmailStore<User> _emailStore;
        //private readonly IEmailSender _emailSender;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
             //UserManager<User> userManager,
             //IUserStore<User> userStore,
             //SignInManager<User> signInManager,
             //IEmailSender emailSender,
             ILogger<RegisterModel> logger)
        {
            //_userManager = userManager;
            //_userStore = userStore;
            //_emailStore = GetEmailStore();
            //_signInManager = signInManager;
            //_emailSender = emailSender;
            _logger = logger;
        }

        //[BindProperty(SupportsGet = true)]
        //[Required]
        //[Display(Name = "Country")]
        //public string CountryId { get; set; }
        //public SelectList SelectCountry { get; set; }


        //[BindProperty(SupportsGet = true)]
        //[Required]
        //[Display(Name = "State")]
        //public string StateId { get; set; }
        //public SelectList SelectState { get; set; }

        //[BindProperty(SupportsGet = true)]
        //[Required]
        //[Display(Name = "Local Gov't Area")]
        //public string LGAId { get; set; }

        ///// <summary>
        /////     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        /////     directly from your code. This API may change or be removed in future releases.
        ///// </summary>
        //[BindProperty]
        //public InputModel Input { get; set; }

        ///// <summary>
        /////     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        /////     directly from your code. This API may change or be removed in future releases.
        ///// </summary>
        //public string ReturnUrl { get; set; }

        ///// <summary>
        /////     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        /////     directly from your code. This API may change or be removed in future releases.
        ///// </summary>
        //public IList<AuthenticationScheme> ExternalLogins { get; set; }

        ///// <summary>
        /////     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        /////     directly from your code. This API may change or be removed in future releases.
        ///// </summary>
        //public class InputModel
        //{
        //    /// <summary>
        //    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        //    ///     directly from your code. This API may change or be removed in future releases.
        //    /// </summary>
        //    [Required]
        //    [EmailAddress]
        //    [Display(Name = "Email")]
        //    public string Email { get; set; }

        //    /// <summary>
        //    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        //    ///     directly from your code. This API may change or be removed in future releases.
        //    /// </summary>
        //    [Required]
        //    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        //    [DataType(DataType.Password)]
        //    [Display(Name = "Password")]
        //    public string Password { get; set; }

        //    /// <summary>
        //    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        //    ///     directly from your code. This API may change or be removed in future releases.
        //    /// </summary>
        //    [DataType(DataType.Password)]
        //    [Display(Name = "Confirm password")]
        //    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //    public string ConfirmPassword { get; set; }




        //    [Required]
        //    [Display(Name = "Title")]
        //    public string Title { get; set; } = "none";

        //    [Required]
        //    [Display(Name = "First Name")]
        //    public string FirstName { get; set; }

        //    [Display(Name = "Middle Name")]
        //    public string MiddleName { get; set; } = "none";

        //    [Required]
        //    [Display(Name = "Last Name")]
        //    public string LastName { get; set; } = "none";

        //    [Display(Name = "Organisation")]
        //    public string OrganisationName { get; set; } = "none";

        //    [Required]
        //    [Display(Name = "Profile Image")]
        //    public IFormFile FileUpload { get; set; }

        //    [Required]
        //    [Display(Name = "Are you Registering as a Clientele or as a skilled Professioal?")]
        //    public SelectList RegistrationType { get; set; }
        //    public bool IsFixer { get; set; }

        //    [Required]
        //    [Display(Name = "Phone Number")]
        //    [DataType(DataType.PhoneNumber)]
        //    public string PhoneNumber { get; set; } = "none";

        //    /// <summary>
        //    /// ///contact model
        //    /// </summary>

        //    [Required]
        //    [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        //    [Display(Name = "Address")]

        //    public string Address { get; set; } = "none";

        //    [Required]
        //    [StringLength(30)]
        //    [Display(Name = "Town")]
        //    public string Town { get; set; } = "none";

        //    [StringLength(20)]
        //    [Display(Name = "Landmark")]
        //    public string Landmarks { get; set; } = "none";
        //}


        public async Task OnGetAsync(string returnUrl = null)
        {
        }

        //public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        //{
        //    returnUrl ??= Url.Content("~/");
        //    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        //    if (ModelState.IsValid)
        //    {
        //        var user = CreateUser();

        //        await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
        //        await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
        //        var result = await _userManager.CreateAsync(user, Input.Password);

        //        if (result.Succeeded)
        //        {
        //            _logger.LogInformation("User created a new account with password.");

        //            var userId = await _userManager.GetUserIdAsync(user);
        //            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        //            var callbackUrl = Url.Page(
        //                "/Account/ConfirmEmail",
        //                pageHandler: null,
        //                values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
        //                protocol: Request.Scheme);

        //            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
        //                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        //            if (_userManager.Options.SignIn.RequireConfirmedAccount)
        //            {
        //                return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
        //            }
        //            else
        //            {
        //                await _signInManager.SignInAsync(user, isPersistent: false);
        //                return LocalRedirect(returnUrl);
        //            }
        //        }
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return Page();
        //}

        //private User CreateUser()
        //{
        //    try
        //    {
        //        return Activator.CreateInstance<User>();
        //    }
        //    catch
        //    {
        //        throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
        //            $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
        //            $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        //    }
        //}

        //private IUserEmailStore<User> GetEmailStore()
        //{
        //    if (!_userManager.SupportsUserEmail)
        //    {
        //        throw new NotSupportedException("The default UI requires a user store with email support.");
        //    }
        //    return (IUserEmailStore<User>)_userStore;
        //}
    }
}
