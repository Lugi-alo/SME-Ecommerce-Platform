using Dissertation.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dissertation.Pages.Authentication
{
    public class AuthenticationModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AuthenticationModel> _logger;

        public AuthenticationModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<AuthenticationModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public RegisterInputModel RegisterInput { get; set; }

        [BindProperty]
        public LoginInputModel LoginInput { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class RegisterInputModel
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
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "User Type")]
            public string UserType { get; set; }
        }

        public class LoginInputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            Console.WriteLine("OnGetAsync called");
        }

        public async Task<IActionResult> OnPostLoginAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            _logger.LogInformation($"Login attempt for email: {LoginInput.Email}");

            var result = await _signInManager.PasswordSignInAsync(LoginInput.Email, LoginInput.Password, LoginInput.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                Console.WriteLine("User logged in.");
                return LocalRedirect(returnUrl);
            }
            else
            {
                _logger.LogError("Invalid login attempt.");
                Console.WriteLine("Invalid login attempt.");
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostRegisterAsync(string returnUrl = null)
        {
            Console.WriteLine("OnPostRegisterAsync called");

            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            Console.WriteLine("Registration Attempt:");

            Console.WriteLine($"Email: {RegisterInput.Email}");
            Console.WriteLine($"Password: {RegisterInput.Password}");
            Console.WriteLine($"First Name: {RegisterInput.FirstName}");
            Console.WriteLine($"Last Name: {RegisterInput.LastName}");
            Console.WriteLine($"User Type: {RegisterInput.UserType}");

            var user = new AppUser
            {
                FirstName = RegisterInput.FirstName,
                LastName = RegisterInput.LastName,
                Email = RegisterInput.Email.Trim(),
                UserName = RegisterInput.Email.Trim(),
                UserType = RegisterInput.UserType
            };

            var result = await _userManager.CreateAsync(user, RegisterInput.Password);

            if (result.Succeeded)
            {
                string role = DetermineRoleBasedOnUserType(RegisterInput.UserType);
                await _signInManager.SignInAsync(user, isPersistent: false); 

                var normalizedRole = _userManager.NormalizeName(role);
                await _userManager.AddToRoleAsync(user, normalizedRole); 

                return LocalRedirect(returnUrl); 
            }

            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Registration Error: {error.Description}");
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }


        private AppUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AppUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(AppUser)}'. " +
                    $"Ensure that '{nameof(AppUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<AppUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }

            return _userManager as IUserEmailStore<AppUser>;
        }

        private string DetermineRoleBasedOnUserType(string userType)
        {
            string role = null;
            switch (userType.ToLower())
            {
                case "business":
                    role = "BusinessOwner";
                    break;
                case "tourist":
                    role = "RegularUser";
                    break;
                case "admin":
                    role = "Admin";
                    break;
            }
            Console.WriteLine($"User type: {userType}, Determined Role: {role}");
            return role;
        }

    }
}
