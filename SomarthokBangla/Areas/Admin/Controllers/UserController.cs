using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SomarthokBangla.Data;
using SomarthokBangla.Models;

namespace SomarthokBangla.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }


        [Authorize]
        public async Task<IActionResult> Manage()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var AppUser = _db.ApplicationUser.FirstOrDefault(c=>c.UserName==user.UserName);
            if (AppUser == null)
            {
                return NotFound();
            }


            return View(AppUser);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Manage(ApplicationUser users)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var AppUser = _db.ApplicationUser.FirstOrDefault(c => c.UserName == user.UserName);
            if (AppUser == null)
            {
                return NotFound();
            }

            AppUser.Email = users.Email;
            AppUser.NormalizedEmail = users.Email.ToUpper();
            AppUser.PhoneNumber = users.PhoneNumber;
            AppUser.LockoutEnd = users.LockoutEnd;

            var result = await _userManager.UpdateAsync(AppUser);
            if (result.Succeeded)
            {
                TempData["save"] = "User Updated Successfully";
                return RedirectToAction(nameof(Manage));
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> PersonalData()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var AppUser = _db.ApplicationUser.FirstOrDefault(c => c.UserName == user.UserName);
            if (AppUser == null)
            {
                return NotFound();
            }

            return View(AppUser);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PersonalData(ApplicationUser applicationUser)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var AppUser = _db.ApplicationUser.FirstOrDefault(c => c.Id == user.Id);
            if (AppUser == null)
            {
                return NotFound();
            }

            AppUser.FirstName = applicationUser.FirstName;
            AppUser.LastName = applicationUser.LastName;
            AppUser.Address = applicationUser.Address;

            var result = await _userManager.UpdateAsync(AppUser);
            if (result.Succeeded)
            {
                TempData["update"] = "User Updated Successfully";
                return RedirectToAction(nameof(PersonalData));
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            LoginUser model = new LoginUser();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginUser model,string ReturnUrl)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                //if (user != null && !user.EmailConfirmed)
                //{
                //    ViewData["ErroMessage"] = "Email not confirmed yet";
                //    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                //    return View(model);

                //}
                if (await _userManager.CheckPasswordAsync(user, model.Password) == false)
                {
                    ViewData["ErroMessage"] = "Invalid Credentials";
                    ModelState.AddModelError(string.Empty, "Invalid credentials");
                    return View(model);

                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(user, new Claim("UserRole", "Admin"));
                    if (!String.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }

                }
                else if (result.IsLockedOut)
                {
                    return View("AccountLocked");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                    return View(model);
                }
            }
            return View(model);
        }


        [HttpGet, AllowAnonymous]
        public IActionResult Register()
        {
            ApplicationUser model = new ApplicationUser();
            return View(model);
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var userCheck = await _userManager.FindByEmailAsync(user.Email);
                if (userCheck == null)
                {

                    var userInfo = new ApplicationUser
                    {
                        UserName = user.Email,
                        NormalizedUserName = user.Email,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        EmailConfirmed = false,
                        PhoneNumberConfirmed = false,

                        FirstName = user.FirstName,
                        Address =user.Address,

                    };

                        var result = await _userManager.CreateAsync(userInfo, user.PasswordHash);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        if (result.Errors.Count() > 0)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("message", error.Description);
                            }
                        }
                        return View(user);
                    }
                }
                else
                {
                    ModelState.AddModelError("message", "Email already exists.");
                    return View(user);
                }
            }
            return View(user);

        
}


        public async Task<IActionResult> Logout(string returnUrl)
        {
            await _signInManager.SignOutAsync();

            if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
        }

        [Authorize]
        public IActionResult ChangePassword()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
        {

                if(ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                }

                await _signInManager.RefreshSignInAsync(user);
            }

            return View();
        }

    }

}


