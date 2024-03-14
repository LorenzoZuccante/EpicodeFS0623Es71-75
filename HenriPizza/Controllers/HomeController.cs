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
            LinkCarrello();
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
            using (DBContext db = new DBContext())
            {
                var user = db.Users.Where(u => u.Email == Email && u.Password == Password).FirstOrDefault();

                if (user == null)
                {
                    ViewBag.Error = "Invalid email or password";
                    return RedirectToAction("Login");
                }
                    FormsAuthentication.SetAuthCookie(user.UserId.ToString(), false); 

                    return RedirectToAction("Index");
                
            }
        }
        [Authorize, HttpPost]
        public ActionResult Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }

            return RedirectToAction("Index");
        }
        
        public void LinkCarrello()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
                using(DBContext db = new DBContext()) 
                {
                    OrderSummary Carrello = db.OrderSummaries.Where(c => c.UserId == userId && c.State == "Non Evaso").FirstOrDefault();
                    if (Carrello == null)
                    {
                        OrderSummary newOrder = new OrderSummary
                        {
                            UserId = userId,
                            State = "Non Evaso"
                        };
                        db.OrderSummaries.Add(newOrder);
                        db.SaveChanges();

                        ViewBag.OrderSummaryId = newOrder.OrderSummaryId;
                    }
                    else
                    {
                        ViewBag.OrderSummaryId = Carrello.OrderSummaryId;
                    }
                }
            }
        }
    }
}
