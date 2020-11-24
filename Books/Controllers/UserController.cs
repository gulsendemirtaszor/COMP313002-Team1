using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Books.Models;
using Books.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Books.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private UserManager<AppUser> _userManager;
        private ICategoryRepository _categoryRepository;

        public UserController(UserManager<AppUser> usrMgr, ICategoryRepository categoryRepo, 
            IAdvertisementRepository advertisementRepo)
        {
            this._userManager = usrMgr;
            this._categoryRepository = categoryRepo;
        }

        //public ViewResult Index() => View(this._userManager.Users);

        public ViewResult EditUser(string userName) 
        {
            ViewBag.Title = "Edit User";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;
            _model.appUser = this._userManager.Users
                .Where(u => u.UserName == userName).FirstOrDefault();

            return View(_model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(SearchListViewModel _model)
        {
            AppUser _editUser = await this._userManager.FindByNameAsync(_model.appUser.UserName);

            if (_editUser != null)
            {                
                _editUser.Address = _model.appUser.Address;
                _editUser.ContactNumber = _model.appUser.ContactNumber;
                _editUser.Email = _model.appUser.EmailAddress;
                _editUser.EmailAddress = _model.appUser.EmailAddress;
                _editUser.HideMap = _model.appUser.HideMap;
            }

            IdentityResult _result = await this._userManager.UpdateAsync(_editUser);

            if (_result.Succeeded)
            {
                string _url = Url.Action("Success");
                string _urlQueryString = $"?userName={_model.appUser.UserName}";

                return new RedirectResult(_url + _urlQueryString);
            }
            else
            {
                foreach (IdentityError error in _result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(_model);
        }
                
        public ViewResult Success(string userName)
        {
            ViewBag.Title = "Updated User";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;
            _model.appUser = this._userManager.Users
                .Where(u => u.UserName == userName).FirstOrDefault();
            return View(_model);
        }

    }
}
