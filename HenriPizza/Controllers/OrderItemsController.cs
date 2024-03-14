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
    public class OrderItemsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: OrderItems
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var orderItems = db.OrderItems.Include(o => o.OrderSummary).Include(o => o.Product);
            return View(orderItems.ToList());
        }

        // GET: OrderItems/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = db.OrderItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            return View(orderItem);
        }

        // GET: OrderItems/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.OrderSummaryId = new SelectList(db.OrderSummaries, "OrderSummaryId", "OrderDate");
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName");
            return View();
        }

        // POST: OrderItems/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ProductId, Quantity")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                using (DBContext db = new DBContext())
                {
                    
                    var userId = Convert.ToInt32(User.Identity.Name); 
                    var orderSummary = db.OrderSummaries.FirstOrDefault(o => o.UserId == userId && o.State == "Non Evaso");

                    if (orderSummary == null)
                    {
                        
                        orderSummary = new OrderSummary { UserId = userId, State = "Non Evaso" };
                        db.OrderSummaries.Add(orderSummary);
                        db.SaveChanges();
                    }

                    
                    orderItem.OrderSummaryId = orderSummary.OrderSummaryId;

                    
                    var product = db.Products.Find(orderItem.ProductId);
                    if (product != null)
                    {
                        orderItem.ItemPrice = product.ProductPrice * orderItem.Quantity;
                    }

                    db.OrderItems.Add(orderItem);
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Prodotto aggiunto al carrello con successo!";
                }

                return RedirectToAction("VetrinaDetails", "Products", new { id = orderItem.ProductId });
            }

            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", orderItem.ProductId);
            return View("VetrinaDetails", db.Products.Find(orderItem.ProductId));
        }


        // GET: OrderItems/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = db.OrderItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderSummaryId = new SelectList(db.OrderSummaries, "OrderSummaryId", "OrderDate", orderItem.OrderSummaryId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", orderItem.ProductId);
            return View(orderItem);
        }

        // POST: OrderItems/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "OrderItemId,OrderSummaryId,ProductId,Quantity,ItemPrice")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderSummaryId = new SelectList(db.OrderSummaries, "OrderSummaryId", "OrderDate", orderItem.OrderSummaryId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", orderItem.ProductId);
            return View(orderItem);
        }

        // GET: OrderItems/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = db.OrderItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            return View(orderItem);
        }

        // POST: OrderItems/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderItem orderItem = db.OrderItems.Find(id);
            db.OrderItems.Remove(orderItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
