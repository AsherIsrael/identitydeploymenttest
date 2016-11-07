using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using TheWall.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace TheWall.Controllers
{
    public class MessagesController : Controller
    {
        private TestContext _context;

        private readonly UserManager<TestUser> _userManager;

        private readonly SignInManager<TestUser> _signInManager;


        public MessagesController(
            TestContext context,
            UserManager<TestUser> userManager,
            SignInManager<TestUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Route("messages")]
        public IActionResult Index()
        {
            // var Messages = _context.Messages.Include(message => message.User).ToList();
            ViewBag.Comment = new Comment();
            ViewBag.Message = new Message();
            return View(_context.messages
                .Include( message => message.TestUser)
                .Include(message => message.Comments)
                    .ThenInclude( comment => comment.TestUser )
                .ToList());
        }

        [HttpGet]
        [Route("new")]
        public IActionResult CreateMessage()
        {
            return View();
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateMessage(Message NewMessage)
        {
            Console.WriteLine("test");
            if(ModelState.IsValid)
            {
                var User = await GetCurrentUserAsync();
                NewMessage.TestUserId = User.Id;
                _context.messages.Add(NewMessage);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Message = NewMessage;
            ViewBag.Comment = new Comment();
            return View("Index", _context.messages
                .Include( message => message.TestUser)
                .Include(message => message.Comments)
                    .ThenInclude( comment => comment.TestUser )
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
