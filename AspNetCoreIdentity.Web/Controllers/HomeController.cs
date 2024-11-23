 using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AspNetCoreIdentity.Web.Extensions;
using AspNetCoreIdentity.Web.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager; //kullanıcnın login olması ile ilgili işlemler
        public HomeController( UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
