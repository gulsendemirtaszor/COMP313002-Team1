using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Books.Models;
using System.Threading.Tasks;
using Books.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Books.Controllers
{
    [Authorize]
    public class BUP_AdminController : Controller
    {
        private UserManager<IdentityUser> userManager;

        public BUP_AdminController(UserManager<IdentityUser> usrMgr)
        {
            userManager = usrMgr;
        }
        
        public ViewResult Index() => View(userManager.Users);

        public ViewResult Create() => View();
        public ViewResult Edit() => View();

        [HttpPost]
        public async Task<IActionResult> Create(User userModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = userModel.UserName,
                    Email = userModel.Email
                };

                IdentityResult result = await userManager.CreateAsync(user, userModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User userModel)
        {
            if (ModelState.IsValid)
            {

                IdentityUser user = await userManager.FindByNameAsync(userModel.UserName);
                user.Email = userModel.Email;

                string newPasswordHash = userManager.PasswordHasher.HashPassword(user,userModel.Password);
                user.PasswordHash = newPasswordHash;
                               
                IdentityResult result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View("Index", userManager.Users);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
