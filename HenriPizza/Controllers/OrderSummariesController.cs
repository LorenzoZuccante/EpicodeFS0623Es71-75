using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HenriPizza.Models;

namespace HenriPizza.Controllers
{
    public class OrderSummariesController : Controller
    {
        private DBContext db = new DBContext();

        // GET: OrderSummaries
        public ActionResult Index()
        {
            var orderSummaries = db.OrderSummaries.Include(o => o.User);
            return View(orderSummaries.ToList());
        }

        // GET: OrderSummaries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            
            OrderSummary orderSummary = db.OrderSummaries
                                          .Include(o => o.OrderItems)
                                          .Include(o => o.OrderItems.Select(i => i.Product))
                                          .SingleOrDefault(o => o.OrderSummaryId == id);

            if (orderSummary == null)
            {
                return HttpNotFound();
            }

            
            ViewBag.TotalPrice = orderSummary.OrderItems?.Sum(item => item.ItemPrice) ?? 0;
            return View(orderSummary);
        }

        // GET: OrderSummaries/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email");
            return View();
        }

        // POST: OrderSummaries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderSummaryId,UserId,OrderDate,OrderAddress,Note,TotalPrice,State")] OrderSummary orderSummary)
        {
            if (ModelState.IsValid)
            {
                db.OrderSummaries.Add(orderSummary);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", orderSummary.UserId);
            return View(orderSummary);
        }

        // GET: OrderSummaries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderSummary orderSummary = db.OrderSummaries.Find(id);
            if (orderSummary == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", orderSummary.UserId);
            return View(orderSummary);
        }

        // POST: OrderSummaries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderSummaryId,UserId,OrderDate,OrderAddress,Note,TotalPrice,State")] OrderSummary orderSummary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderSummary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", orderSummary.UserId);
            return View(orderSummary);
        }

        // GET: OrderSummaries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderSummary orderSummary = db.OrderSummaries.Find(id);
            if (orderSummary == null)
            {
                return HttpNotFound();
            }
            return View(orderSummary);
        }

        // POST: OrderSummaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderSummary orderSummary = db.OrderSummaries.Find(id);
            db.OrderSummaries.Remove(orderSummary);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmOrder(int id, string OrderAddress, string Note)
        {
            if (id < 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderSummary orderSummary = db.OrderSummaries.Where(o => o.OrderSummaryId == id).FirstOrDefault();
            if (orderSummary == null)
            {
                return HttpNotFound();
            }
            decimal PrezzoTotale = db.OrderItems
                         .Where(o => o.OrderSummaryId == id)
                         .Sum(item => item.ItemPrice);
            if (ModelState.IsValid)
            {
                orderSummary.OrderDate = DateTime.Now.ToString();
                orderSummary.State = "Evaso";
                orderSummary.OrderAddress = OrderAddress;
                orderSummary.Note = Note;
                orderSummary.TotalPrice = PrezzoTotale;

                db.Entry(orderSummary).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMessage"] = true;
                return RedirectToAction("Index", "Home");
            }

            
            return View("Details", orderSummary);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(int itemId)
        {
            OrderItem item = db.OrderItems.Find(itemId);
            if (item != null)
            {
                int orderSummaryId = item.OrderSummaryId;
                db.OrderItems.Remove(item);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = orderSummaryId });
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

    }
}
