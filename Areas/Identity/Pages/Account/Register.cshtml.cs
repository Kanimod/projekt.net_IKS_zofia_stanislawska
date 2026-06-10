#nullable disable
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Fischt.Models;
using Fischt.Repositories;
using Fischt.DTOs;

namespace projekt.net_IKS_zofia_stanislawska.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IProfileRepository _profileRepo;
        private readonly FischtDbContext _context;

        public RegisterModel(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IProfileRepository profileRepo,
            FischtDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _profileRepo = profileRepo;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public SelectList SpeciesList { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Email is required")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Hasło is required")]
            [StringLength(100, ErrorMessage = "Paswword needs to have minimum of {2} signs.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = "Passwords ain't the same.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Name is required")]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Age is required")]
            [Range(18, 99, ErrorMessage = "You need to be 18.")]
            [Display(Name = "Wiek")]
            public int Age { get; set; }

            [Display(Name = "About you")]
            public string Bio { get; set; }

            [Required(ErrorMessage = "Biological sex is required")]
            [Display(Name = "Sex")]
            public string Sex { get; set; }

            [Display(Name = "Gender")]
            public string Gender { get; set; }

            [Display(Name = "Pronouns")]
            public string Pronouns { get; set; }

            [Display(Name = "Specie")]
            public int? SpecieId { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            SpeciesList = new SelectList(_context.Species.ToList(), "Id", "Name");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            SpeciesList = new SelectList(_context.Species.ToList(), "Id", "Name");

            if (ModelState.IsValid)
            {
                var user = CreateUser();
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");

                    await _profileRepo.CreateAsync(user.Id, new ProfileDto
                    {
                        Name = Input.Name,
                        Age = Input.Age,
                        Bio = Input.Bio,
                        Sex = Input.Sex,
                        Gender = Input.Gender ?? Input.Sex,
                        Pronouns = Input.Pronouns,
                        SpecieId = Input.SpecieId,
                        Preferences = ""
                    });

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
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

            return Page();
        }

        private User CreateUser()
        {
            try { return Activator.CreateInstance<User>(); }
            catch
            {
                throw new InvalidOperationException($"Couldn't create '{nameof(User)}'.");
            }
        }

                private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
                throw new NotSupportedException("Wymagany .");
            return (IUserEmailStore<User>)_userStore;
        }


    }
    
}