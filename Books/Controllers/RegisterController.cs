using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Books.Models;
using Books.Models.ViewModels;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Nexmo.Api;

namespace Books.Controllers
{
    public class RegisterController : Controller
    {
        public IConfiguration _appConfig { get; }
        private UserManager<AppUser> _userManager;
        public EncryptDecryptData _encryptDecryptData;
        private ICategoryRepository _categoryRepository;

        public RegisterController(UserManager<AppUser> usrMgr, IConfiguration config,
            ICategoryRepository categoryRepo)
        {
            this._userManager = usrMgr;
            this._appConfig = config;
            this._categoryRepository = categoryRepo;
        }

        public ViewResult Index() => View();

        public ViewResult Register()
        {
            ViewBag.Title = "Registration";

            SearchListViewModel _model = new SearchListViewModel();
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;
            _model.appUser = new AppUser();

            return View(_model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(SearchListViewModel _model)
        {
            _model.bookInfoListViewModel = new BookInfoListViewModel();
            _model.bookInfoListViewModel.Categories = this._categoryRepository.Categories;

            if (ModelState.IsValid)
            {
                string _email = _model.appUser.EmailAddress;
                string _userName = _model.appUser.FirstName.Substring(0, 1) + _model.appUser.LastName.Replace(" ", string.Empty).ToLower();
                string _verificationCode = Guid.NewGuid().ToString().Replace("-", string.Empty).ToLower();

                AppUser _appUser = new AppUser
                {
                    UserName = _userName,
                    FirstName = _model.appUser.FirstName,
                    MiddleName = _model.appUser.MiddleName,
                    LastName = _model.appUser.LastName,
                    Address = _model.appUser.Address,
                    ContactNumber = _model.appUser.ContactNumber,
                    Email = _model.appUser.EmailAddress,
                    EmailAddress = _model.appUser.EmailAddress,
                    UserType = "New",
                    IsVerified = false,
                    VerificationCode = _verificationCode

                };

                string _newPasswordHash = this._userManager.PasswordHasher.HashPassword(_appUser, _model.appUser.Password);
                _appUser.PasswordHash = _newPasswordHash;

                IdentityResult _result = await this._userManager.CreateAsync(_appUser, _model.appUser.Password);

                if (_result.Succeeded)
                {
                    this.SendMail(_email, _appUser.FirstName + " " + _appUser.LastName, _appUser.UserName, _appUser.VerificationCode);

                    return RedirectToAction("ThankYou", new { @username = _appUser.UserName });
                }
                else
                {
                    foreach (IdentityError error in _result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(_model);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyBySms(string username, string code)
        {
            if (username?.Length < 1)
                return RedirectToAction("Index", "Book");

            AppUser user = await this._userManager.FindByNameAsync(username);

            if (user.IsVerified == true)            
                return View("Expired");
            
            if (user.VerificationCode.Trim() != code)
                return View("Error");

            user.IsVerified = true;
            user.VerificationCode = "Expired";
            user.PhoneNumberConfirmed = true;

            IdentityResult _result = await this._userManager.UpdateAsync(user);

            if (_result.Succeeded)
            {
                return RedirectToAction("Verify");
            }
            else
            {
                foreach (IdentityError error in _result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> VerifyBySms(string username)
        {
            if (username == null || username.Length < 3)
                return RedirectToAction("Index", "Book");

            AppUser user = await this._userManager.FindByNameAsync(username);

            if (user == null)
            {
                return RedirectToAction("Index", "Book");
            }
            else
            {
                Random random = new Random();
                user.VerificationCode = random.Next(1000000, 9999999).ToString();                
                IdentityResult _result = await this._userManager.UpdateAsync(user);
                string apiKey = this._appConfig["Data:Sms:ApiKey"];
                string apiSecret = this._appConfig["Data:Sms:ApiSecret"];
                string sender = this._appConfig["Data:Sms:Sender"];

                var client = new Client(creds: new Nexmo.Api.Request.Credentials
                {
                    ApiKey = apiKey,
                    ApiSecret = apiSecret
                });
                var results = client.SMS.Send(request: new SMS.SMSRequest
                {
                    from = sender,
                    to = "1" + user.ContactNumber,
                    text = $"Your PaperBack verification code for the user {user.UserName} is {user.VerificationCode}"
                });                

                return View("VerifyBySms", user);
            }            
        }



        public async Task<IActionResult> ThankYou(string username)
        {
            AppUser user = await this._userManager.FindByNameAsync(username);
            ViewBag.Title = "Thank you for registering";
            return View(user);
        }

        public void SendMail(string _email, string _name, string _username, string _verificationCode)
        {
            this._encryptDecryptData = new EncryptDecryptData();
            string _sName = this._appConfig["Data:Email:G_Name:Name"];
            string _sEmail = this._appConfig["Data:Email:G_Sndr:Sender"];
            string _password = this._appConfig["Data:Email:G_Pass:Password"];
            string _subject = this._appConfig["Data:Email:G_Subj:Subject"];
            string _host = this._appConfig["Data:Email:G_Host:Host"];
            int _port = int.Parse(this._appConfig["Data:Email:G_Port:Port"]);
            int _enableSsl = int.Parse(this._appConfig["Data:Email:G_ESsl:SSL"]);
            string _protocol = this._appConfig["Data:Email:G_Protocol:Protocol"];
            string _dns = this._appConfig["Data:Email:G_DNS:DNS"];
            string _path = this._appConfig["Data:Email:G_Path:Path"];

            string _rName = _name;
            string _rEmail = _email;

            MailAddress _senderEmail = new MailAddress(_sEmail, _sName);
            MailAddress _receiverEmail = new MailAddress(_rEmail, _rName);

            string _mailPassword = this._encryptDecryptData.DecryptData(_password);
            string _mailSubject = _subject + " " + _rName;

            string _url = $"{_protocol}://{_dns}{_path}?username={_username}&code={_verificationCode}";
            string _mailBody = this.MailBody(_url, _rName, _username);

            SmtpClient smtp = new SmtpClient
            {
                Host = _host,
                Port = _port,
                EnableSsl = _enableSsl == 1 ? true : false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(_senderEmail.Address, _mailPassword)
            };

            using (MailMessage mess = new MailMessage(_senderEmail, _receiverEmail)
            {
                Subject = _mailSubject,
                Body = _mailBody,
                IsBodyHtml = true
            })
            {
                smtp.Send(mess);
            }
        }

        public string MailBody(string _url, string _name, string _username)
        {
            string _mailBody = "";
            _mailBody = _mailBody + "<br/>";
            _mailBody = _mailBody + $"<div style='font-family:Calibri;font-size:12pt'>Dear { _name },<br />";
            _mailBody = _mailBody + $"Your username to access the PaperBack is <b>{_username}</b> ,<br />";
            _mailBody = _mailBody + "<br/>";
            _mailBody = _mailBody + "<div style='font-family:Calibri;font-size:12pt'>To finish your registration setup, " +
                "click URL below to verify your account.</div>";
            _mailBody = _mailBody + "<br/>";
            _mailBody = _mailBody + $"<div style='font-family:Calibri;font-size:12pt'>URL:<a href='{ _url }'>{ _url }</a></div>";
            _mailBody = _mailBody + "<br/>";
            _mailBody = _mailBody + "<div style='font-family:Calibri;font-size:12pt'>Thank you</div>";
            _mailBody = _mailBody + "<div style='font-family:Calibri;font-size:12pt'><b>The PaperBack Team</b></div>";
            return _mailBody;
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Verify(string username, string code)
        {
            ViewBag.Title = "Verification";

            if (code == null || code == string.Empty || username == null || username == string.Empty)
            {
                return View("Verify");
            }

            AppUser user = await this._userManager.FindByNameAsync(username);

            if (user != null)
            {
                if (user.IsVerified == true || user.VerificationCode.Trim() != code)
                {
                    return View("Expired");
                }


                user.IsVerified = true;
                user.VerificationCode = "Expired";
                user.EmailConfirmed = true;

                IdentityResult _result = await this._userManager.UpdateAsync(user);

                if (_result.Succeeded)
                {
                    return RedirectToAction("Verify");
                }
                else
                {
                    foreach (IdentityError error in _result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }

            return View("Verify?username=" + username + "&code=" + code);
        }
    }
}