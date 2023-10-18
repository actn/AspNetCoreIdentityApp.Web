using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AspNetCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
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

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel request,string returnUrl=null)
        {
            returnUrl = returnUrl ?? Url.Action("Index", "Home");

            if (!ModelState.IsValid)
            {
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult result;
            AppUser user=null;
            if (MailAddress.TryCreate(request.UserName,out _))
            {
                user = await _userManager.FindByEmailAsync(request.UserName);
               
            }

            result= user == null ? await _signInManager.PasswordSignInAsync(request.UserName, request.Password, request.RememberMe, false) : await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email veya şifre hatalıdır.");
                return View();
            }

            return Redirect(returnUrl);
        }
        public IActionResult SignUp()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            var identityResult = await _userManager.CreateAsync(new()
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.Phone
            }, request.Password);

            if (identityResult.Succeeded)
            {
                TempData["SuccessMessage"] = "Kayıt işleminiz başarı ile gerçekleştirildi.";
                return RedirectToAction(nameof(HomeController.SignUp));
            }

            foreach (var identityResultError  in identityResult.Errors)
            {
                ModelState.AddModelError(string.Empty,identityResultError.Description);                
            }
            return View();
        }
    }
}