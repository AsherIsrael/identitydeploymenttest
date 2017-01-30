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
    // public static class TestIt
    // {
    //     public static void DoesThisWork(this TestUser user)
    //     {
    //         Console.WriteLine("stuff");
    //     }
    // }

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
        [Route("update")]
        public async Task<IActionResult> Update(string returnUrl = "")
        {
            TestUser thingy = await GetCurrentUserAsync();
            await _userManager.UpdateAsync(thingy);
            return RedirectToAction("Show");
        }


        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            _signInManager.SignOutAsync();
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
                TestUser User = new TestUser {UserName = user.FirstName, Email = user.Email, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
                TestUser User2 = new TestUser {UserName = "Dylan", Email = user.Email, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
                Task<IdentityResult> Async1 = _userManager.CreateAsync(User, user.Password);
                Task<IdentityResult> Async2 = _userManager.CreateAsync(User2, user.Password);
                IdentityResult result = await Async1;
                IdentityResult result2 = await Async2;
                if(result.Succeeded && result2.Succeeded)
                {
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
                TestUser User = await _userManager.FindByEmailAsync(login.Email);
                var result = await _signInManager.PasswordSignInAsync(User.UserName, login.Password, isPersistent: false, lockoutOnFailure: false);
                if(result.Succeeded)
                {
                    TestUser loggedInUser = await GetCurrentUserAsync();
                    // string thing = loggedInUser.Id;
                    // Console.WriteLine(thing);
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
            string redirectUrl = Url.Action("ExternalLoginCallback", "Users");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback(string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View("Login");
            }
            ExternalLoginInfo Info = await _signInManager.GetExternalLoginInfoAsync();
            if( Info == null )
            {
                return RedirectToAction("Login");
            }
            var result = await _signInManager.ExternalLoginSignInAsync(Info.LoginProvider, Info.ProviderKey, isPersistent: false);
            if( result.Succeeded )
            {
                await _signInManager.UpdateExternalAuthenticationTokensAsync(Info);
                return RedirectToAction("Show");
            }
            ViewBag.LoginProvider = Info.LoginProvider;
            return View("ThirdPartyConfirmation");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("ExternalConfirmation")]
        public async Task<IActionResult> ExternalConfirmation(ExternalConfirmationViewModel model)
        {
            if( ModelState.IsValid )
            {
                ExternalLoginInfo Info = await _signInManager.GetExternalLoginInfoAsync();
                if ( Info == null )
                {
                    return RedirectToAction("Login");
                }
                var User = new TestUser{ UserName = model.UserName};
                IdentityResult Result = await _userManager.CreateAsync(User);
                if ( Result.Succeeded )
                {
                    Result = await _userManager.AddLoginAsync(User, Info);
                    if( Result.Succeeded )
                    {
                        await _signInManager.SignInAsync(User, isPersistent: false);
                        await _signInManager.UpdateExternalAuthenticationTokensAsync(Info);

                        return RedirectToAction("Show");
                    }
                }
            }
            return View("ThirdPartyConfirmation");
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
            TestUser User = await GetCurrentUserAsync();
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
        public async Task<IActionResult> ShowThirdParty(string returnUrl = "")
        {
            TestUser User = await GetCurrentUserAsync();
            Console.WriteLine(User.UserName);
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
            TestUser user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, code);
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