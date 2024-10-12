using AspNetCoreIdentity.Web.Areas.Admin.Models;
using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentity.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<AppUser> _userManager;

    public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }


    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> UserList()
    {
        var userList = await _userManager.Users.ToListAsync();

        var userViewModelList = userList.Select(u => new UserViewModel()
        {
            UserId = u.Id,
            UserName = u.UserName,
            Email = u.Email
        }).ToList();
        return View(userViewModelList);
    }
}