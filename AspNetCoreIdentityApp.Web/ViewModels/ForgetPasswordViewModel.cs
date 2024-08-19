using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels;

public class ForgetPasswordViewModel
{
    [EmailAddress(ErrorMessage = "E-Mail formatı hatalıdır.")]
    [Required(ErrorMessage = "EMail boş bırakılamaz.")]
    [Display(Name="EMail :")]
    public string Email { get; set; }
}