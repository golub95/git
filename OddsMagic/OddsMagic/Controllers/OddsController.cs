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
    public class OddsController : Controller
    {
        private HRKladeEntities db = new HRKladeEntities();

        // GET: Odds
        public ActionResult Index()
        {
            return View(db.CalcOddsTable.ToList().OrderBy(t=> t.CalcOdd));
        }

        // GET: Odds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalcOddsTable calcOddsTable = db.CalcOddsTable.Find(id);
            if (calcOddsTable == null)
            {
                return HttpNotFound();
            }
            return View(calcOddsTable);
        }

        // GET: Odds/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Odds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventID,Home,Away,EventTime,Odd1,Klada1,OddX,KladaX,Odd2,Klada2,Odd1X,Klada1X,OddX2,KladaX2,Odd12,Klada12")] CalcOddsTable calcOddsTable)
        {
            if (ModelState.IsValid)
            {
                db.CalcOddsTable.Add(calcOddsTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(calcOddsTable);
        }

        // GET: Odds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalcOddsTable calcOddsTable = db.CalcOddsTable.Find(id);
            if (calcOddsTable == null)
            {
                return HttpNotFound();
            }
            return View(calcOddsTable);
        }

        // POST: Odds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventID,Home,Away,EventTime,Odd1,Klada1,OddX,KladaX,Odd2,Klada2,Odd1X,Klada1X,OddX2,KladaX2,Odd12,Klada12")] CalcOddsTable calcOddsTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(calcOddsTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(calcOddsTable);
        }

        // GET: Odds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalcOddsTable calcOddsTable = db.CalcOddsTable.Find(id);
            if (calcOddsTable == null)
            {
                return HttpNotFound();
            }
            return View(calcOddsTable);
        }

        // POST: Odds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CalcOddsTable calcOddsTable = db.CalcOddsTable.Find(id);
            db.CalcOddsTable.Remove(calcOddsTable);
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
