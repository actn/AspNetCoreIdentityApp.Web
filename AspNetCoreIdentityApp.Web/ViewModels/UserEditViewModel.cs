using AspNetCoreIdentityApp.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "Kullanıcı Adı boş bırakılamaz.")]
        [Display(Name = "Kullanıcı Adı :")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "E-Mail formatı hatalıdır.")]
        [Required(ErrorMessage = "EMail boş bırakılamaz.")]
        [Display(Name = "EMail :")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon boş bırakılamaz.")]
        [Display(Name = "Telefon :")]
        public string Phone { get; set; }



        [Display(Name = "Şehir :")]
        public string City { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Doğum Tarihi :")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Cinsiyet :")]
        public Gender? Gender { get; set; }


        [Display(Name = "Profil Resmi :")]
        public IFormFile Picture { get; set; }
    }
}
