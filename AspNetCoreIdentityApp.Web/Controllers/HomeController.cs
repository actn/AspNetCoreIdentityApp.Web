using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AspNetCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using System.Net.Mail;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Services;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
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
        public async Task<IActionResult> SignIn(SignInViewModel request, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Action("Index", "Home");

            if (!ModelState.IsValid)
            {
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult result;
            AppUser user = MailAddress.TryCreate(request.UserName, out _) ?
           await _userManager.FindByEmailAsync(request.UserName) :
           await _userManager.FindByNameAsync(request.UserName);

            result = user == null ? await _signInManager.PasswordSignInAsync(request.UserName, request.Password, request.RememberMe, true) : await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcınız 3 dakikalığına kilitlenmiştir.");
                return View();
            }

            if (result.Succeeded)
            {
                return Redirect(returnUrl);

            }
            var errorList = new List<string>();
            errorList.Add("Email veya şifre hatalıdır.");
            if (user != null)
            {
                errorList.Add($"Başarısız giriş sayısı {await _userManager.GetAccessFailedCountAsync(user)}");
            }

            ModelState.AddModelErrorList(errorList);
            return View();

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

            foreach (var identityResultError in identityResult.Errors)
            {
                ModelState.AddModelError(string.Empty, identityResultError.Description);
            }
            return View();
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
        {
            var hasUser=await _userManager.FindByEmailAsync(request.Email);

            if (hasUser==null)
            {
                ModelState.AddModelError(string.Empty, "Bu email adresine sahip kullanıcı bulunamamıştır.");
                return View();
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);

            var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = hasUser.Id, Token = passwordResetToken },HttpContext.Request.Scheme);


            //Email servisinden gönder
            await _emailService.SendPasswordResetEmail(passwordResetLink, hasUser.Email);

            TempData["SuccessMessage"] = "Şifre yenileme linki e-posta adresinize gönderilmiştir.";
            return RedirectToAction(nameof(ForgetPassword));
        }

        public IActionResult ResetPassword(string userId,string token)
        {
            TempData["userId"] = userId; 
            TempData["token"]=token;
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];

            if (userId is null || token is null)
            {
                throw new ArgumentException("Hatalı istek parametreleri lütfen linki kontrol ediniz.");

            }

            var hasUser = await _userManager.FindByIdAsync(userId.ToString());

            if (hasUser==null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(hasUser, token.ToString(), request.Password);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifreniz başarılı şekilde değiştirilmiştir.";
            }
            else
            {
                ModelState.AddModelErrorList(result.Errors.Select(p=>p.Description).ToList());
            }

            return View();
        }
    }
}