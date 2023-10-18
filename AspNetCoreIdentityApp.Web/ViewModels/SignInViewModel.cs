using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class SignInViewModel
    {
        public SignInViewModel()
        {
            
        }
        public SignInViewModel(string userName, string password, bool rememberMe)
        {
            UserName = userName;
            Password = password;
            RememberMe = rememberMe;
        }

        [Required(ErrorMessage = "Kullanıcı Adı boş bırakılamaz.")]
        [Display(Name = "Kullanıcı Adı :")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
        [Display(Name = "Şifre :")]
        public string Password { get; set; }


        [Display(Name = "Beni Hatırla :")]
        public bool RememberMe { get; set; }
    }
}
