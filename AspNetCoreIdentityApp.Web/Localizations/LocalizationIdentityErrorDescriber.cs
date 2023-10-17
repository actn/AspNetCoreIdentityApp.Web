using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.Localizations
{
    public class LocalizationIdentityErrorDescriber:IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new() { Code = nameof(DuplicateUserName), Description = $"{userName} kullanıcı adı daha önce alınmış." };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new() { Code = nameof(DuplicateEmail), Description = $"{email} mail adresi daha önce kullanılmış." };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new() { Code = nameof(PasswordTooShort), Description = $"Şifre için {length} uzunluğu yeterli değil. En az 6 karakterden oluşmalı. " };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new() { Code = nameof(PasswordRequiresLower), Description = $"Şifre küçük harf içermeli. " };
        }
    }
}
