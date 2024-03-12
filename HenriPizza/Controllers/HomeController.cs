using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security; 
using HenriPizza.Models;

namespace HenriPizza.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        [HttpGet]
        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string Email, string Password)
        {
            using (var db = new DBContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Email == Email && u.Password == Password);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(Email, false); 

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "Invalid email or password";
                    return View();
                }
            }
        }
        
        public ActionResult Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }

            return RedirectToAction("Index");
        }
    }
}
