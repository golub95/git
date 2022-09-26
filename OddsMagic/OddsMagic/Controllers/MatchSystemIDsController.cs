using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OddsMagic.Models;

namespace OddsMagic.Controllers
{
    public class MatchSystemIDsController : Controller
    {
        private HRKladeEntities db = new HRKladeEntities();

        // GET: MatchSystemIDs
        //public ActionResult Index()
        //{
        //    return View(db.MatchSystemIDs.ToList());
        //}

        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var events = from s in db.MatchSystemIDs
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                events = events.Where(s => s.EventName.Contains(searchString)
                                       || s.EventSystemID.ToString() == searchString);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    events = events.OrderByDescending(s => s.EventName);
                    break;
                case "Date":
                    events = events.OrderBy(s => s.EventDateTime);
                    break;
                case "date_desc":
                    events = events.OrderByDescending(s => s.EventDateTime);
                    break;
                default:
                    events = events.OrderBy(s => s.EventSystemID);
                    break;
            }
            return View(events.ToList());
        }

        // GET: MatchSystemIDs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatchSystemIDs matchSystemIDs = db.MatchSystemIDs.Find(id);
            if (matchSystemIDs == null)
            {
                return HttpNotFound();
            }
            return View(matchSystemIDs);
        }

        // GET: MatchSystemIDs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MatchSystemIDs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,EventSystemID,EventName,EventDateTime,EventSportTypeID,KladaName,Similarity,Matched")] MatchSystemIDs matchSystemIDs)
        {
            if (ModelState.IsValid)
            {
                db.MatchSystemIDs.Add(matchSystemIDs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(matchSystemIDs);
        }

        // GET: MatchSystemIDs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatchSystemIDs matchSystemIDs = db.MatchSystemIDs.Find(id);
            if (matchSystemIDs == null)
            {
                return HttpNotFound();
            }
            return View(matchSystemIDs);
        }

        // POST: MatchSystemIDs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,EventSystemID,EventName,EventDateTime,EventSportTypeID,KladaName,Similarity,Matched")] MatchSystemIDs matchSystemIDs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(matchSystemIDs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(matchSystemIDs);
        }

        // GET: MatchSystemIDs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatchSystemIDs matchSystemIDs = db.MatchSystemIDs.Find(id);
            if (matchSystemIDs == null)
            {
                return HttpNotFound();
            }
            return View(matchSystemIDs);
        }

        // POST: MatchSystemIDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MatchSystemIDs matchSystemIDs = db.MatchSystemIDs.Find(id);
            db.MatchSystemIDs.Remove(matchSystemIDs);
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
