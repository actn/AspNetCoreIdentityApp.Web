using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class SignUpViewModel
    {
        public SignUpViewModel()
        {
                
        }
        public SignUpViewModel(string userName, string email, string phone, string password)
        {
            UserName = userName;
            Email = email;
            Phone = phone;
            Password = password;
        }

        [Required(ErrorMessage = "Kullanıcı Adı boş bırakılamaz.")]
        [Display(Name="Kullanıcı Adı :")]
        public string UserName { get; set; }
        
        [EmailAddress(ErrorMessage = "E-Mail formatı hatalıdır.")]
        [Required(ErrorMessage = "EMail boş bırakılamaz.")]
        [Display(Name="EMail :")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Telefon boş bırakılamaz.")]
        [Display(Name="Telefon :")]
        public string Phone  { get; set; }
        
        [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
        [Display(Name="Şifre :")]
        public string Password { get; set; }
        
        [Compare(nameof(Password),ErrorMessage= "Farklı bir şifre girdiniz.")]
        [Required(ErrorMessage = "Şifre tekrarı boş bırakılamaz.")]
        [Display(Name="Şifre Terkar :")]
        public string PasswordConfirm { get; set; }

    }
}
