using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TheWall.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace TheWall.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private TestContext _context;

        private readonly UserManager<TestUser> _userManager;

        private readonly SignInManager<TestUser> _signInManager;

        // private readonly IEmailSender _emailSender;

        public UsersController(
            TestContext context,
            UserManager<TestUser> userManager,
            SignInManager<TestUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            // _emailSender = emailSender;
        }

        [HttpGet]
        [Route("index")]
        public IActionResult Index(string returnUrl = "")
        {
            List<TestUser> thisthing = _context.users
                .Include(user => user.Shoppingcarts)
                    .ThenInclude(shop => shop.Product)
                .ToList();
                
            return View(_context.users.Include(user => user.Shoppingcarts)
                .ThenInclude(shop => shop.Product).ToList());
        }

        [HttpGet]
        [Route("edit")]
        public async Task<IActionResult> Update()
        {
            Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            TestUser thingy = await GetCurrentUserAsync();
            thingy.UserName = "Lili";
            thingy.Email = "Lili@WRock.edu";
            // await _userManager.SetUserNameAsync(thingy, "Lili");
            _context.SaveChanges();
            thingy = await GetCurrentUserAsync();
            Console.WriteLine(thingy.UserName);
            // await _userManager.UpdateAsync(thingy);
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            ViewData["returnUrl"] = "";
            return View();
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(Register user)
        {

            if(ModelState.IsValid)
            {
                // var User = new TestUser {UserName = user.FirstName, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email };
                TestUser User = new TestUser {UserName = user.FirstName, Email = user.Email, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
                IdentityResult result = await _userManager.CreateAsync(User, user.Password);
                if(result.Succeeded)
                {

                    // var code = await _userManager.GenerateEmailConfirmationTokenAsync(User);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Users", new { userId = User.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    // await _emailSender.SendEmailAsync(user.Email, "Confirm your account",
                    //    "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                    await _signInManager.SignInAsync(User, isPersistent: false);
                    return RedirectToAction("Show");
                }
                AddErrors(result);
            }
            return View(user);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if(ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(login.Email);
                // var something = await _signInManager.
                var result = await _signInManager.PasswordSignInAsync(User.UserName, login.Password, isPersistent: false, lockoutOnFailure: false);
                if(result.Succeeded)
                {
                    var loggedInUser = await GetCurrentUserAsync();
                    var thing = loggedInUser.Id;
                    return RedirectToAction("Show");
                }

            }
            ModelState.AddModelError(string.Empty, "Invalid Email or Password");
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("ExternalLogin")]
        public IActionResult ExternalLogin(string provider)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ShowThirdParty", "Users");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [Route("login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            ViewData["Error"] = "";
            return View();
        }

        [HttpGet]
        [Route("user")]
        public async Task<IActionResult> Show(string returnUrl = "")
        {
            Console.WriteLine("YYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY");
            var User = await GetCurrentUserAsync();
            return View( _context.users
                .Include(user => user.Messages)
                    .ThenInclude(message => message.Comments)
                .SingleOrDefault(u => u.Id == User.Id)
            );
                //   .Include(user => user.Messages)
                //                 .ThenInclude(message => message.Comments)
        }

        [HttpGet]
        [Route("ThirdParty")]
        public IActionResult ShowThirdParty(string returnUrl = "")
        {
            return View();
        }

        [HttpGet]
        // [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private Task<TestUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion
    }
}