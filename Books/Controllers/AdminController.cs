using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Books.Models;
using System.Threading.Tasks;
using Books.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Books.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private UserManager<AppUser> _userManager;
        private ICategoryRepository _categoryRepository;
        private IAdvertisementRepository _advertisementRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        
        public AdminController(UserManager<AppUser> userManagerRepo, 
            ICategoryRepository categoryRepo, IAdvertisementRepository advertisementRepo, IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
            this._userManager = userManagerRepo;
            this._categoryRepository = categoryRepo;
            this._advertisementRepository = advertisementRepo;
        }

   
        public ActionResult ListUsers()
        {
            string authStatus = _contextAccessor.HttpContext.Session.GetString("auth");
            if (authStatus != "completed")                
                return RedirectToAction("Logout", "Account");            

            ViewBag.Title = "List of User Accounts";
            TempData["loadMessage"] = "User List has been loaded.";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;
            _model.appUserListViewModel = new AppUserListViewModel();
            _model.appUserListViewModel.appUsers = this._userManager.Users
                .Where(n => !n.UserName.Equals("admin") && 
                (n.ContactNumber != null && n.EmailAddress != null))
                .OrderBy(u => u.Id);

            return View(_model);
        }

        public ActionResult ListAdvertisements()
        {
            string authStatus = _contextAccessor.HttpContext.Session.GetString("auth");
            if (authStatus != "completed")
                return RedirectToAction("Logout", "Account");

            ViewBag.Title = "List of Request Advertisements";
            TempData["loadMessage"] = "Request Advertisements List has been loaded.";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;

            _model.advertisementsListViewModel = new AdvertisementsListViewModel();
            _model.advertisementsListViewModel.Advertisements = this._advertisementRepository.Advertisements
                .OrderBy(ads => ads.RequestId);

            int _adsCount = _model.advertisementsListViewModel.Advertisements.Count();
            if (_adsCount == 0)
            {
                TempData["errorMessage"] = "No record(s) found";
            }

            return View(_model);
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            string authStatus = _contextAccessor.HttpContext.Session.GetString("auth");
            if (authStatus != "completed")
                return RedirectToAction("Logout", "Account");

            AppUser _deleteUser = await this._userManager.FindByIdAsync(userId);

            if (_deleteUser != null)
            {
                IdentityResult _result = await this._userManager.DeleteAsync(_deleteUser);
                if (_result.Succeeded)
                {
                    string _url = Url.Action("DeleteSuccess");
                    string _urlQueryString = $"?userName={_deleteUser.UserName}";

                    return new RedirectResult(_url + _urlQueryString);
                }
                else
                {
                    foreach (IdentityError error in _result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View("ListUsers");
        }

        public ActionResult DeleteSuccess(string userName)
        {
            string authStatus = _contextAccessor.HttpContext.Session.GetString("auth");
            if (authStatus != "completed")
                return RedirectToAction("Logout", "Account");

            ViewBag.Title = "Deleted User";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;

            //_model.appUserListViewModel = new AppUserListViewModel();
            //_model.appUserListViewModel.appUsers = this._userManager.Users
            //    .Where(u => u.UserName == userName).FirstOrDefault();

            return View(_model);

            //return View();
        }

        public ActionResult EditUser(string userName)
        {
            string authStatus = _contextAccessor.HttpContext.Session.GetString("auth");
            if (authStatus != "completed")
                return RedirectToAction("Logout", "Account");

            ViewBag.Title = "Edit User";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;
            _model.appUser = this._userManager.Users
                .Where(u => u.UserName == userName).FirstOrDefault();

            return View(_model);

            //return View(this._userManager.Users
            //    .Where(u => u.UserName == userName)
            //    .FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(SearchListViewModel _model)
        {
            string authStatus = _contextAccessor.HttpContext.Session.GetString("auth");
            if (authStatus != "completed")
                return RedirectToAction("Logout", "Account");

            AppUser _editUser = await this._userManager.FindByNameAsync(_model.appUser.UserName);

            if (_editUser != null)
            {
                //_editUser.FirstName = _model.appUser.FirstName;
                //_editUser.MiddleName = _model.appUser.MiddleName;
                //_editUser.LastName = _model.appUser.LastName;
                _editUser.Address = _model.appUser.Address;
                _editUser.ContactNumber = _model.appUser.ContactNumber;
                _editUser.Email = _model.appUser.EmailAddress;
                _editUser.EmailAddress = _model.appUser.EmailAddress;
                //_editUser.PasswordHash = _model.appUser.PasswordHash;
            }

            IdentityResult _result = await this._userManager.UpdateAsync(_editUser);

            if (_result.Succeeded)
            {
                string _url = Url.Action("EditSuccess");
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

        public ActionResult EditSuccess(string userName)
        {
            string authStatus = _contextAccessor.HttpContext.Session.GetString("auth");
            if (authStatus != "completed")
                return RedirectToAction("Logout", "Account");

            ViewBag.Title = "Edit User";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;
            _model.appUser = this._userManager.Users
                .Where(u => u.UserName == userName).FirstOrDefault();
            return View(_model);

            //return View();
        }

        [HttpGet]
        public ActionResult EditAdvertisement(int? requestId)
        {
            string authStatus = _contextAccessor.HttpContext.Session.GetString("auth");
            if (authStatus != "completed")
                return RedirectToAction("Logout", "Account");
            ViewBag.Title = "Edit Request Advertisement";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;

            _model.advertisementsListViewModel = new AdvertisementsListViewModel();
            _model.advertisementsListViewModel.advertisement = this._advertisementRepository.Advertisements
                .Where(ads => ads.RequestId == requestId).FirstOrDefault();

            return View(_model);
        }

        [HttpPost]
        public ActionResult EditAdvertisement(SearchListViewModel _model)
        {
            string authStatus = _contextAccessor.HttpContext.Session.GetString("auth");
            if (authStatus != "completed")
                return RedirectToAction("Logout", "Account");

            ViewBag.Title = "Edit Request Advertisement";
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;

            if (ModelState.IsValid)
            {
                if (_model.advertisementsListViewModel.advertisement.RequestId > 0)
                {
                    _model.advertisementsListViewModel.advertisement.Status = "Approved";
                    this._advertisementRepository.SaveAdvertisement(_model.advertisementsListViewModel.advertisement);
                    TempData["returnMessage"] = "View Request Advertisement Details";
                    TempData["successMessage"] = $"Successfully edited request: {_model.advertisementsListViewModel.advertisement.Title}";
                }
                else
                {
                    TempData["returnMessage"] = null;
                    TempData["successMessage"] = null;
                }
            }
            else
            {
                TempData["returnMessage"] = null;
                TempData["successMessage"] = null;
            }
            
            return View("ViewRequestAdvertisements", _model);
        }

        [HttpGet]
        public ActionResult ViewRequestAdvertisements(int? requestId)
        {
            string authStatus = _contextAccessor.HttpContext.Session.GetString("auth");
            if (authStatus != "completed")
                return RedirectToAction("Logout", "Account");

            ViewBag.Title = "View Request Advertisement Details";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;

            _model.advertisementsListViewModel = new AdvertisementsListViewModel();
            _model.advertisementsListViewModel.advertisement = this._advertisementRepository.Advertisements
                .Where(ads => ads.RequestId == requestId).First();

            return View(_model);
        }

        [HttpGet]
        public ActionResult DeleteAdvertisement(int requestId)
        {
            string authStatus = _contextAccessor.HttpContext.Session.GetString("auth");
            if (authStatus != "completed")
                return RedirectToAction("Logout", "Account");

            ViewBag.Title = "Delete Request Advertisement";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;

            _model.advertisementsListViewModel = new AdvertisementsListViewModel();
            _model.advertisementsListViewModel.advertisement = this._advertisementRepository.DeleteAdvertisement(requestId);

            return View(_model);
        }

    }
}
