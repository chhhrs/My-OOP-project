using System;
using System.Linq;
using System.Web.Mvc;
using UserAccounts.Models;

namespace UserAccounts.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult UserList()
        {
            ViewBag.Message = "The user list.";

            using (var db = new ApplicationDbContext())
            {
                var viewModel = db.Users.ToList().Select(x => new UserListViewModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Status = ParseStatus(x.LockoutEndDateUtc)
                });

                return View(viewModel);
            }
        }

        private string ParseStatus(DateTime? lockoutEnd)
        {
            return lockoutEnd > DateTime.Now ? "Locked" : "Active";
        }
    }
}