﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gas_Station.Models;
using PagedList;
using UnidecodeSharpFork;

namespace Gas_Station.Controllers
{
    public class GasStationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: GasStations
        public async Task<ActionResult> Index(string sortOrder, string id, string selectMun,string selectBrand)
        {
            
            string searchString = id;
            var gasStasions = from x in db.GasStations select x;

            var Munlist = new List<string>();
            var MunQry = from x in db.GasStations orderby x.Municipality select x.Municipality;
            Munlist.AddRange(MunQry.Distinct());

            var BrandList = new List<string>();
            var BrandQry = from x in db.GasStations orderby x.Brand select x.Brand;
            BrandList.AddRange(BrandQry.Distinct());

            ViewBag.selectMun = new SelectList(Munlist);
            ViewBag.selectBrand = new SelectList(BrandList);

            ViewBag.PriceSort = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.BrandSort = sortOrder == "Brand" ? "Brand_desc" : "Brand";
            ViewBag.MunSort = sortOrder == "Mun" ? "Mun_desc" : "Mun";

            if (!String.IsNullOrEmpty(searchString))
            {
                
                gasStasions = gasStasions.Where(x => x.Name.ToUpper().Contains(searchString.ToUpper()) || x.Address.ToUpper().Contains(searchString.ToUpper()) || x.Municipality.ToUpper().Contains(searchString.ToUpper()) || x.Dep.ToUpper().Contains(searchString.ToUpper()) || x.Brand.ToUpper().Contains(searchString.ToUpper()) || x.County.ToUpper().Contains(searchString.ToUpper()));

                    //.Contains(searchString) || x.Municipality.Contains(searchString) || x.County.Contains(searchString) || x.Brand.Contains(searchString) || x.Dep.Contains(searchString)));
            }

            if (!String.IsNullOrEmpty(selectMun))
            {
                gasStasions = gasStasions.Where(x => x.Municipality == selectMun);   
            }

            if (!String.IsNullOrEmpty(selectBrand))
            {
                gasStasions = gasStasions.Where(x => x.Brand == selectBrand);
            }

            switch (sortOrder)
            {
                case "price_desc":
                    gasStasions = gasStasions.OrderByDescending(x => x.Price);
                    break;
                case "date_desc":
                    gasStasions = gasStasions.OrderByDescending(x => x.PriceUpdate);
                    break;
                case "Date":
                    gasStasions = gasStasions.OrderBy(x => x.PriceUpdate);
                    break;
                case "Brand_desc":
                    gasStasions = gasStasions.OrderByDescending(x => x.Brand);
                    break;
                case "Brand":
                    gasStasions = gasStasions.OrderBy(x => x.Brand);
                    break;
                case "Mun":
                    gasStasions = gasStasions.OrderBy(x => x.Municipality);
                    break;
                case "Mun_desc":
                    gasStasions = gasStasions.OrderByDescending(x => x.Municipality);
                    break;
                default:
                    gasStasions = gasStasions.OrderBy(x => x.Price);
                    break;
            }

            return View(await gasStasions.ToListAsync());
        }

        public JsonResult DynamicListBrands(string mun)
        {
            //var BrandList = new List<string>();
            var BrandList = db.GasStations.Where(x => x.Municipality == mun).OrderBy(x => x.Brand).Select(x => x.Brand).Distinct().ToList();
            List<SelectListItem> BrandSelectList = BrandList.Select(x => new SelectListItem { Text = x, Value = x }).ToList();
            //var BrandQry = from x in gasstations orderby x.Brand select x.Brand;
            //BrandList.AddRange(BrandQry.Distinct());
            //System.Console.Write(BrandList);
            BrandSelectList.Insert(0, new SelectListItem { Text = "All", Value = null });
            return Json(new SelectList(BrandSelectList,"Value","Text")); 
        }

        //public JsonResult DynamicListMuns(string brand)
        //{
        //    var MunList = db.GasStations.Where(x => x.Brand == brand).OrderBy(x => x.Municipality).Select(x => x.Municipality).Distinct().ToList();
        //    List<SelectListItem> MunSelectList = MunList.Select(x => new SelectListItem { Text = x, Value = x }).ToList();
        //    MunSelectList.Insert(0, new SelectListItem { Text = "All", Value = null });
        //    return Json(new SelectList(MunSelectList, "Value", "Text"));
        //}


        public JsonResult DynamicSearch(string text)
        {
            var list = new List<string>();
            var MunQry = from x in db.GasStations orderby x.Municipality select x.Municipality;
            var BrandQry = from x in db.GasStations orderby x.Brand select x.Brand;
            list.AddRange(MunQry.Distinct());
            list.AddRange(BrandQry.Distinct());

            var filter = list.Where(x => x.StartsWith(text));

            return Json(filter, JsonRequestBehavior.AllowGet);
        }

        // GET: GasStations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GasStation gasStation = db.GasStations.Find(id);
            if (gasStation == null)
            {
                return HttpNotFound();
            }
            return View(gasStation);
        }

        // GET: GasStations/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: GasStations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,County,CountyID,Municipality,DepId,Dep,Address,Brand,Latitude,Longitude,Price,PriceUpdate,FuelType,Name,Phone")] GasStation gasStation)
        {
            if (ModelState.IsValid)
            {
                gasStation.PriceUpdate = DateTime.Now;
                db.GasStations.Add(gasStation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gasStation);
        }

        // GET: GasStations/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GasStation gasStation = db.GasStations.Find(id);
            if (gasStation == null)
            {
                return HttpNotFound();
            }
            return View(gasStation);
        }

        // POST: GasStations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,County,CountyID,Municipality,DepId,Dep,Address,Brand,Latitude,Longitude,Price,PriceUpdate,FuelType,Name,Phone")] GasStation gasStation)
        {
            if (ModelState.IsValid)
            {
                gasStation.PriceUpdate = DateTime.Now;
                db.Entry(gasStation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gasStation);
        }

        // GET: GasStations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GasStation gasStation = db.GasStations.Find(id);
            if (gasStation == null)
            {
                return HttpNotFound();
            }
            return View(gasStation);
        }

        // POST: GasStations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            GasStation gasStation = db.GasStations.Find(id);
            db.GasStations.Remove(gasStation);
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
