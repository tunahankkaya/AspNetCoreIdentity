using AspNetCoreIdentity.Web.CustomValidations;
using AspNetCoreIdentity.Web.Models;

namespace AspNetCoreIdentity.Web.Extensions;

public static class StartupExtensions
{
    public static void AddIdentityWithExtension(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>(options =>
        {
            //kullanıcı ile ilgili ayarlar 
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuvwxyz1234567890_.";

            //şifre ile ilgili ayarlar
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = false;
        })
            .AddPasswordValidator<PasswordValidator>().AddEntityFrameworkStores<AppDbContext>();
    }
}