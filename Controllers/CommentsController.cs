using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using TheWall.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TheWall.Controllers
{
    public class CommentsController : Controller
    {
        private TestContext _context;

        private readonly UserManager<TestUser> _userManager;

        private readonly SignInManager<TestUser> _signInManager;


        public CommentsController(
            TestContext context,
            UserManager<TestUser> userManager,
            SignInManager<TestUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("createComment")]
        public async Task<IActionResult> CreateComment( Comment comment )
        {
            if(ModelState.IsValid)
            {
                var User = await GetCurrentUserAsync();
                comment.TestUserId = User.Id;
                _context.comments.Add( comment );
                _context.SaveChanges();
                return RedirectToAction("Index", "Messages");
            }
            ViewBag.Comment = comment;
            ViewBag.Message = new Message();
            return View("Index", _context.messages
                .Include( message => message.TestUser)
                .Include(message => message.Comments)
                    .ThenInclude( c => c.TestUser )
                .ToList());
        }

        #region Helpers

        private Task<TestUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion
    }
}