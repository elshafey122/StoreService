using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Entities.Identity;
using Talabat.APIs.Dtos;

namespace AdminPanel.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Login));
            }

            var user=await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Invalid Email");
                return RedirectToAction(nameof(Login));
            }
            var result=await _signInManager.CheckPasswordSignInAsync(user,login.Password,false);

            if(!result.Succeeded || !await _userManager.IsInRoleAsync(user,"Admin"))
            {
                ModelState.AddModelError(string.Empty, "You are not authorized");
                return RedirectToAction(nameof(Login));
            }
            return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> LogOut() // logout user
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

    }
}
