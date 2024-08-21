using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class ResetPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
        [Display(Name = "Yeni Şifre :")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Farklı bir şifre girdiniz.")]
        [Required(ErrorMessage = "Şifre tekrarı boş bırakılamaz.")]
        [Display(Name = "Yeni Şifre Terkar :")]
        public string PasswordConfirm { get; set; }
    }
}
