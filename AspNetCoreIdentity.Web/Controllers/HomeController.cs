 using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AspNetCoreIdentity.Web.Extensions;
using AspNetCoreIdentity.Web.Services.Abstract;
using AspNetCoreIdentity.Web.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager; //kullanıcnın login olması ile ilgili işlemler
        private readonly IEmailService _emailService;
        public HomeController( UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl=null )
        {
            returnUrl = returnUrl ?? Url.Action("Index", "Home");
            
            var hasUser = await _userManager.FindByEmailAsync(model.EMail);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya şifre yanlış.");
                return View(model);
            }
            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, model.Password, model.RemamberMe, true);

            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl);
            }

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() {"Çok fazla deneme yaptınız.3 dakika boyunca giriş yapamazsınız"});
                return View();
            }
            ModelState.AddModelErrorList(new List<string>() {$"Email veya şifreniz yanlış."});
            ModelState.AddModelErrorList(new List<string>{$"Başarısız Giriş Sayısı: { await _userManager.GetAccessFailedCountAsync(hasUser)}"});
            return View();
        }

        [HttpPost]
        public async  Task<IActionResult> SignUp(SignUpViewModel request)
        {
         

            if (!ModelState.IsValid)
            {
                return View();
            }

            var identityResult = await _userManager.CreateAsync(new()
            {
                UserName = request.UserName,
                PhoneNumber = request.Phone,
                Email = request.Email
            }, request.Password);

            if (identityResult.Succeeded)
            {
                TempData["SuccessMessage"] = "Üyelik kayıt işlemi başarıyla gerçekleştirilmiştir";
                Console.WriteLine("Üyelik kayıt işlemi başarıyla gerçekleştirilmiştir");

                return RedirectToAction(nameof(HomeController.SignUp));
            }

            ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());
            return View();
            
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
        {
            var hasUser = await _userManager.FindByEmailAsync(request.Email);
            if (hasUser ==null)
            {
                ModelState.AddModelError(string.Empty,"geçersiz e-mail adresi");
                return View();
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);
            
            // örnek link: http://localhost:5233?userId=21313&token=abcjhdbsajhbfdas
            var passwordResetLink = Url.Action("ResetPassword","Home", new
            {
                email = hasUser.Id,
                token = passwordResetToken,
            },HttpContext.Request.Scheme); 
            
            
            await _emailService.SendResetPasswordEmail(passwordResetLink,hasUser.Email);
            TempData["SuccessMessage"] = "Şifre yenileme linki, e-mail adresinize gönderilmiştir";
            return RedirectToAction(nameof(ForgetPassword));
        }

        public IActionResult ResetPassword(string userId,string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];

            if (userId == null || token == null)
                throw new Exception("Bir hata meydana geldi");
            
            var hasUser = await _userManager.FindByIdAsync(userId.ToString()!);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty,"Kullanıcı bulunamadı.");
                return View();
            }
            
            var result = await _userManager.ResetPasswordAsync(hasUser,token.ToString()!,request.Password);
            if (result.Succeeded)
                TempData["SuccessMessage"] = "Şifreniz başarıyla yenilenmiştir";
            
            else
                ModelState.AddModelErrorList(result.Errors.Select(x => x.Description).ToList());
            
            
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
