using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class PasswordChangeViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
        [Display(Name = "Eski Şifre :")]
        [MinLength(6,ErrorMessage ="Şifreniz en az 6 karakter olabilir.")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yeni Şifre boş bırakılamaz.")]
        [Display(Name = "Yeni Şifre :")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir.")]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Farklı bir şifre girdiniz.")]
        [Required(ErrorMessage = "Yeni Şifre tekrarı boş bırakılamaz.")]
        [Display(Name = "Yeni Şifre Terkar :")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir.")]
        public string NewPasswordConfirm { get; set; }
    }
}
