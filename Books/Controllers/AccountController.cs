using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Books.Models;
using Books.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 
using Microsoft.Extensions.Configuration;
using Nexmo.Api;
using Microsoft.AspNetCore.Http;

namespace Books.Controllers
{    

    [Authorize]
    public class AccountController : Controller
    {
        public IConfiguration _appConfig { get; }        
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private ICategoryRepository _categoryRepository;

        // ** Changed from IdentityUser to AppUser
        public AccountController(UserManager<AppUser> userMgr, SignInManager<AppUser> signInMgr,
            ICategoryRepository categoryRepo, IConfiguration config) {
            userManager = userMgr;
            signInManager = signInMgr;
            this._categoryRepository = categoryRepo;
            this._appConfig = config;
        }

        [AllowAnonymous]
        public ViewResult Login(string returnUrl) 
        {
            ViewBag.Title = "Login";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;
            _model.loginModel = new LoginModel { ReturnUrl = returnUrl };

            //return View(new LoginModel {
            //    ReturnUrl = returnUrl
            //});

            return View(_model);
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(SearchListViewModel _model)
        {
            string _errorMessage = string.Empty;
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;

            if (ModelState.IsValid) 
            {
                // ** Changed from IdentityUser to AppUser
                AppUser user = await userManager.FindByNameAsync(_model.loginModel.UserName);
                if (user != null) 
                {
                    HttpContext.Session.SetString("auth", "completed");
                    if (user.UserName.ToLower() == "admin")
                    {
                        if ((await signInManager.PasswordSignInAsync(user,
                            _model.loginModel.Password, false, false)).Succeeded)
                        {
                            // the user is admin here and must be redirect to 2fa
                            //return Redirect(_model.loginModel?.ReturnUrl ?? "/");
                            HttpContext.Session.SetString("auth", "partial");
                            return RedirectToAction("VerifyBySms", new { username = user.UserName, returnUrl = _model.loginModel?.ReturnUrl ?? " / " });                            
                        }
                        else
                        {
                            _errorMessage = "Invalid UserName or Password";
                        }
                    }
                    else
                    {
                        if (user.IsVerified == false)
                        {
                            _errorMessage = "User is not yet verified. Please open your mailbox for further verification. Thank you.";
                        }
                        else
                        {
                            if ((await signInManager.PasswordSignInAsync(user,
                                _model.loginModel.Password, false, false)).Succeeded)
                            {
                                return Redirect(_model.loginModel?.ReturnUrl ?? "/");
                            }
                            else
                            {
                                _errorMessage = "Invalid UserName or Password";
                            }
                        }
                    }
                }
                else
                {
                    _errorMessage = "Invalid UserName or Password";
                }
            }
            ModelState.AddModelError("", _errorMessage);
            return View(_model);
        }

        public async Task<RedirectResult> Logout(SearchListViewModel _model)
        {
            await this.signInManager.SignOutAsync();
            return Redirect(_model.loginModel?.ReturnUrl ?? "/");
        }

        [HttpPost]
        public async Task<IActionResult> VerifyBySms(string username, string code, string returnUrl)
        {
            if (username?.Length < 1)
                return RedirectToAction("Index", "Book");

            AppUser user = await this.userManager.FindByNameAsync(username);

            var sessionCode = HttpContext.Session.GetString("code");
            if (sessionCode == null) {
                return RedirectToAction("VerifyBySms");
            }
            else if (sessionCode.Trim() != code)
                return View("Error");
            else
            {
                HttpContext.Session.SetString("auth", "completed");
                return Redirect(returnUrl ?? "/");
            }           
        }

        [HttpGet]
        public async Task<IActionResult> VerifyBySms(string username, string returnUrl)
        {            
            if (username == null || username.Length < 3)
                return RedirectToAction("Index", "Book");

            AppUser user = await this.userManager.FindByNameAsync(username);

            if (user == null)
            {
                return RedirectToAction("Index", "Book");
            }
            else
            {
                Random random = new Random();
                string Code = random.Next(1000000, 9999999).ToString();
                HttpContext.Session.SetString("code", Code);

                string apiKey = this._appConfig["Data:Sms:ApiKey"];
                string apiSecret = this._appConfig["Data:Sms:ApiSecret"];
                string sender = this._appConfig["Data:Sms:Sender"];

                var client = new Client(creds: new Nexmo.Api.Request.Credentials
                {
                    ApiKey = apiKey,
                    ApiSecret = apiSecret
                });
                Console.WriteLine(user.ContactNumber);
                var results = client.SMS.Send(request: new SMS.SMSRequest
                {
                    from = sender,
                    to = "1" + user.ContactNumber,
                    text = $"Your PaperBack verification code for the user {user.UserName} is {Code}"
                });

                return View("VerifyBySms", user);
            }
        }
    }
}
