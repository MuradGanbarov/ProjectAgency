using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectAgency.Areas.ProjectAgencyAdmin.Models.Utilities.Enums;
using ProjectAgency.Areas.ProjectAgencyAdmin.ViewModels.Account;
using ProjectAgency.Models;
using System.Security.Cryptography.X509Certificates;

namespace ProjectAgency.Areas.ProjectAgencyAdmin.Controllers
{
    [Area("ProjectAgencyAdmin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = new()
            {
                Name = vm.Name,
                Surname = vm.Surname,
                Email = vm.Email,
                UserName = vm.UserName,
                Gender = vm.Gender.ToString()
            };
            IdentityResult result = await _userManager.CreateAsync(user,vm.Password);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty,"Username,Email or Password incorrect");
                }
                return View();
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }
        
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm,string?returnUrl)
        {
            if(!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByNameAsync(vm.UserNameOrEmail);
            if(user is null)
            {
                user = await _userManager.FindByEmailAsync(vm.UserNameOrEmail);
                if(user is null)
                {
                    ModelState.AddModelError(string.Empty,"Password,email or username incorrect");
                    return View();
                }
            }

            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.isRemembered, true);

            if(result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Login is not enable,please try later");
                return View();
            }

            if(!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Password,email or username incorrect");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        //public async Task<IActionResult> CreateRoles()
        //{
        //    foreach(UserRoles role in Enum.GetValues(typeof(UserRoles)))
        //    {
        //        if (!(await _roleManager.RoleExistsAsync(role.ToString())))
        //            await _roleManager.CreateAsync(new IdentityRole
        //            {
        //                Name = role.ToString(),
        //            });
                
        //    }
        //    return RedirectToAction("Index", "Home");

        //}

    }
}
