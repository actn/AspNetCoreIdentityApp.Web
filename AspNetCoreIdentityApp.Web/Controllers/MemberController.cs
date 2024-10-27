using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {

        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFileProvider _fileProvider;
        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            var userViewModel = new UserViewModel
            {
                Email = currentUser.Email,
                UserName = currentUser.UserName,
                PhoneNumber = currentUser.PhoneNumber,
                PictureUrl = currentUser.Picture

            };


            return View(userViewModel);
        }

        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            var isOldPAsswordCorrect = await _userManager.CheckPasswordAsync(currentUser, request.OldPassword);

            if (!isOldPAsswordCorrect)
            {
                ModelState.AddModelError(string.Empty, "Eski şifre hatalı.");
                return View();
            }

            var result = await _userManager.ChangePasswordAsync(currentUser, request.OldPassword, request.NewPassword);


            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors.Select(p => p.Description).ToList());
                return View();
            }
            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, request.NewPassword, true, false);

            TempData["SuccessMessage"] = "Şifre değiştirme işlemi başarılı ile gerçekleştirdi.";
            return View();
        }

        public async Task<IActionResult> EditUser()
        {
            ViewBag.genderList = new SelectList(Enum.GetNames(typeof(Gender)));
            var currentUser = await _userManager.FindByNameAsync(User.Identity?.Name);

            var userEditviewModel = new UserEditViewModel()
            {
                UserName=currentUser.UserName,
                Email=currentUser.Email,
                Phone=currentUser.PhoneNumber,
                BirthDate=currentUser.BirthDate,
                City=currentUser.City,
                Gender = (Gender)(currentUser.Gender??0)
            };
            return View(userEditviewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(UserEditViewModel request,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            currentUser.UserName = request.UserName;
            currentUser.Email = request.Email;
            currentUser.PhoneNumber = request.Phone;
            currentUser.BirthDate = request.BirthDate;
            currentUser.City = request.City;
            currentUser.Gender = (byte)request.Gender;
            if (request.Picture!=null && request.Picture.Length>0)
            {
                var wwwrootFolder=_fileProvider.GetDirectoryContents("wwwroot");

                var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.Picture.FileName)}";

                var picturePath = Path.Combine(wwwrootFolder.First(x => x.Name == "userpictures").PhysicalPath, randomFileName);


                using var stream = new FileStream(picturePath,FileMode.Create);

                await request.Picture.CopyToAsync(stream);

                stream.Close();
                currentUser.Picture = randomFileName;

            }

            var updateUserResult= await _userManager.UpdateAsync(currentUser);
            if (!updateUserResult.Succeeded)
            {
                ModelState.AddModelErrorList(updateUserResult.Errors);
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(currentUser, true);
            TempData["SuccessMessage"] = "Üye bilgileri başarı ile değiştirildi.";
            ViewBag.genderList = new SelectList(Enum.GetNames(typeof(Gender)));
          
            return View(request);

        }
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

    }
}
