using DDAC_CMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DDAC_CMS.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class AdminController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateUser()
        {
            ViewBag.Roles = context.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(RegisterViewModel model)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            string UserName = model.UserName;
            string email = model.Email;
            string userPWD = model.Password;
            string rol = model.Roles;
            string fullName = model.FullName;
            string phone = model.PhoneNumber;

            //cretae default user
            var user = new ApplicationUser();
            user.UserName = UserName;
            user.Email = email;
            user.FullName = fullName;
            user.PhoneNumber = phone;
            
            var newuser = userManager.Create(user, userPWD);

            //Add User to Role    
            if (newuser.Succeeded)
            {
                var result = userManager.AddToRole(user.Id, rol);
            }

            return View("Index");
        }

        public ActionResult ViewUser()
        {
            var usersWithRoles = (from user in context.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      Email = user.Email,
                                      FullName = user.FullName,
                                      PhoneNumber = user.PhoneNumber,
                                      RoleNames = (from userRole in user.Roles
                                                   join role in context.Roles on userRole.RoleId equals role.Id
                                                   select role.Name).ToList()
                                  }).ToList().Select(p => new UserViewModel()
                                  {
                                      UserId = p.UserId,
                                      Username = p.Username,
                                      Email = p.Email,
                                      FullName = p.FullName,
                                      PhoneNumber = p.PhoneNumber,
                                      Role = string.Join(",", p.RoleNames)
                                  });
            return View(usersWithRoles);
        }

        public ActionResult ResetUserPassword(string userId, string UserName)
        {
            ViewBag.Username = UserName.ToString();
            ViewBag.UserId = userId.Trim().ToString();
            return View();
        }

        [HttpPost]
        public ActionResult ResetUserPassword(ResetUserPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
                if (userManager.HasPassword(model.UserId))
                {
                    userManager.RemovePassword(model.UserId);
                    userManager.AddPassword(model.UserId, model.ConfirmPassword);

                }
                TempData["Message"] = "Password successfully reset to " + model.ConfirmPassword;
                TempData["MessageValue"] = "1";

                return RedirectToAction("ViewUser", "Admin", new { area = "", });
            }
            else
            {
                // If we got this far, something failed, redisplay form
                TempData["Message"] = "Invalid User Details. Please try again in some minutes ";
                TempData["MessageValue"] = "0";
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            string username = string.Empty;
            if (!String.IsNullOrEmpty(userId))
            {
                ApplicationUser applicationUser = await userManager.FindByIdAsync(userId);
                if (applicationUser != null)
                {
                    username = applicationUser.UserName;
                }
            }
            return PartialView("_DeleteUser", username);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(string userId, DeleteUserViewModel model)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!String.IsNullOrEmpty(userId))
            {
                ApplicationUser applicationUser = await userManager.FindByIdAsync(userId);
                if (applicationUser != null)
                {
                    IdentityResult result = await userManager.DeleteAsync(applicationUser);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ViewUser");
                    }
                }
            }
            return View();
        }

    }
}