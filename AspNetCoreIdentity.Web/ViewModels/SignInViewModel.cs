using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Web.ViewModels;

public class SignInViewModel
{
    public SignInViewModel()
    {
        
    }
    public SignInViewModel(string EMail, string password, bool rememberMe = false)
    {
        EMail = EMail;
        Password = password;
        RemamberMe = rememberMe;
    }

    [EmailAddress(ErrorMessage = "Email formatı yanlıştır")]
    [Required(ErrorMessage = "Email alanı boş bırakılamaz")]
    [Display(Name = "Email :")]
    public string EMail { get; set; }
    
    [Required(ErrorMessage = "Şifre alanı boş bırakılamaz")]
    [Display(Name = "Şifre: ")]
    public string Password { get; set; }
    
    [Display(Name = "Beni hatırla")]
    public bool RemamberMe { get; set; }
}