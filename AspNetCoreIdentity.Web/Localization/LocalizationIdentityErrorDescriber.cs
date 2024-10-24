using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace AspNetCoreIdentity.Web.Localization;

public class LocalizationIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError DuplicateUserName(string userName)
    {
        return new()
        {
            Code = "DuplicateUserName",
            Description = $"{userName} daha önce başka bir kullanıcı tarafından alınmıştır"
        };
    }

    public override IdentityError DuplicateEmail(string email)
    {
        return new()
        {
            Code = "DuplicateEmail",
            Description = $"Bu {email} daha önce başka bir kullanıcı tarafından alınmıştır"
        };
    }

    public override IdentityError PasswordTooShort(int length)
    {
        return new()
        {
            Code = "PasswordTooShort",
            Description = $"Şifre en az 6 karakterli olmalıdır."
        };
    }
}