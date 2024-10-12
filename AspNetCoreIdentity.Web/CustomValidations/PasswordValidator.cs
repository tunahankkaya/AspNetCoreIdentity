using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Web.CustomValidations;

public class PasswordValidator : IPasswordValidator<AppUser>
{
    public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
    {
        var errors = new List<IdentityError>();

        if (password!.ToLower().Contains(user.UserName!.ToLower()))
        {
            errors.Add(new () {Code = "PasswordNoContainUserName", Description = "Şifre alanı kullanıcı adı içeremez."});
        }

        if (password!.ToLower().StartsWith("1234"))
        {
            errors.Add(new() { Code = "PasswordNoContain1234", Description = "Şifre alanı ardışık sayı." });
        }

        if (errors.Any())
        {
            foreach (var identityError in errors) return Task.FromResult(IdentityResult.Failed(identityError));
        }

        return Task.FromResult(IdentityResult.Success);

    }
}