using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Web.Controllers;

[Authorize] //yetkilendir (bu controller üyelere özel)
public class MemberController : Controller
{
    
    private readonly SignInManager<AppUser> _signInManager;

    public MemberController(SignInManager<AppUser> signInManager)
    {
        _signInManager = signInManager;
    }
    // GET
    public async Task LogOut()
    {
        await _signInManager.SignOutAsync();
        
       
    }

    public IActionResult Index()
    {
        return View();
    }
}