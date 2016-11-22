using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gas_Station.Models;

namespace Gas_Station.Controllers
{
    [Authorize]
    public class OffersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Offers
        public ActionResult Index(string sortOrder, string id, string selectOff)
        {
            IEnumerable<Offer> offers = db.Offers.Include(x => x.GasStation).ToList();
            string searchString = id;
            var Offlist = new List<string>();
            var OffQry = from x in db.Offers orderby x.Type select x.Type;
            Offlist.AddRange(OffQry.Distinct());
            ViewBag.selectOff = new SelectList(Offlist);

            ViewBag.PriceSort = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.TypeSort = sortOrder == "Type" ? "Type_desc" : "Type";

            if (!String.IsNullOrEmpty(searchString))
            {
                offers = offers.Where(x => x.Type.Contains(searchString) || x.GasStation.Name.Contains(searchString) || x.Description.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(selectOff))
            {
                offers = offers.Where(x => x.Type == selectOff);
            }

            switch (sortOrder)
            {
                case "price_desc":
                    offers = offers.OrderByDescending(x => x.Price);
                    break;
                case "date_desc":
                    offers = offers.OrderByDescending(x => x.DateExpired);
                    break;
                case "Date":
                    offers = offers.OrderBy(x => x.DateExpired);
                    break;
                default:
                    offers = offers.OrderBy(x => x.Price);
                    break;
            }


            return View(offers);
        }

        // GET: Offers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // GET: Offers/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            //var offer=new Offer() { GasStation=id}
            return View();
        }

        // POST: Offers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "Id,Type,Description,Price,DateCreated,DateExpired")]*/ Offer offer)
        {
            if (ModelState.IsValid)
            {
                GasStation gasstation = db.GasStations.Find(offer.GasStationId);
                offer.GasStationId = gasstation.Id;
                offer.GasStation = gasstation;
                offer.DateCreated = DateTime.Now;
                offer.DateExpired = DateTime.Now.AddDays(30);
                db.Offers.Add(offer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(offer);
        }

        // GET: Offers/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // POST: Offers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Type,Description,Price,DateCreated,DateExpired")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(offer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(offer);
        }

        // GET: Offers/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // POST: Offers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Offer offer = db.Offers.Find(id);
            db.Offers.Remove(offer);
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
