using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SomarthokBangla.Areas.Admin.Model;
using SomarthokBangla.Data;

namespace SomarthokBangla.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<IdentityUser> _userManager;
        ApplicationDbContext _db;

         public RoleController(RoleManager<IdentityRole> roleManager, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        [HttpGet]
        public IActionResult Manage()
        {
            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = roles;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult>CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(string name)
        {

            IdentityRole role = new IdentityRole();
            role.Name = name;
            var isExist =  await _roleManager.RoleExistsAsync(name);
            if(isExist)
            {
                ViewBag.mgs = "This role exist";
                ViewBag.name = name;
                return View();
            }
            var result =await _roleManager.CreateAsync(role);
            if(result.Succeeded)
            {
                TempData["save"] = "Role Saved Successfully";
                return RedirectToAction(nameof(Manage));
            }

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if(role==null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(string id,string name)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            role.Name = name;

            //var isExist = await _roleManager.RoleExistsAsync(name);
            //if (isExist && role==null)
            //{
            //    ViewBag.mgs = "This role exist";
            //    ViewBag.name = name;
            //    return View();
            //}
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                TempData["save"] = "Role Updated Successfully";
                return RedirectToAction(nameof(Manage));
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RoleDetails(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RoleDelete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;
            return View();
        }

        [HttpPost]
        [ActionName("RoleDelete")]
        public async Task<IActionResult> RoleDeleteConfirm(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["save"] = "Role Deleted Successfully";
                return RedirectToAction(nameof(Manage));
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AssignUserRole()
        {
            ViewData["UserId"] = new SelectList(_db.ApplicationUser.ToList(), "Id", "UserName");
            ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssignUserRole(RoleUserViewModel roleUserViewModel)
        {
            var user = _db.ApplicationUser.FirstOrDefault(c=>c.Id==roleUserViewModel.UserId);
            var isRoleExist = await _userManager.IsInRoleAsync(user, roleUserViewModel.RoleId);
            if(!isRoleExist)
            {
                var role = await _userManager.AddToRoleAsync(user, roleUserViewModel.RoleId);

                if (role.Succeeded)
                {
                    TempData["save"] = "Role Assigned Successfully";
                    return RedirectToAction(nameof(Manage));
                }
            }
            else
            {
                ViewBag.msg = "User Already assigned to role";
                ViewData["UserId"] = new SelectList(_db.ApplicationUser.ToList(), "Id", "UserName");
                ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
                return View();
            }


            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ManageUser()
        {
            var user = _db.ApplicationUser.ToList();
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUser(string name)
        {

            IdentityRole role = new IdentityRole();
            role.Name = name;
            var isExist = await _roleManager.RoleExistsAsync(name);
            if (isExist)
            {
                ViewBag.mgs = "This role exist";
                ViewBag.name = name;
                return View();
            }
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                TempData["save"] = "Role Saved Successfully";
                return RedirectToAction(nameof(Manage));
            }

            return View();
        }

    }
}