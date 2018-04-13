using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DDAC_CMS.Models;

namespace DDAC_CMS.Controllers
{
    public class BookingsController : Controller
    {
        private CMS_DBEntities db = new CMS_DBEntities();

        // GET: Bookings
        public ActionResult ViewVessels()
        {
            return View(db.Vessels.ToList());
        }

        public ActionResult ViewShippingSchedule()
        {
            return View(db.ShippingSchedules.ToList());
        }

        public ActionResult Index()
        {
            var bookings = db.Bookings.Include(b => b.AspNetUser).Include(b => b.ShippingSchedule).Include(b => b.Cargo);
            return View(bookings.ToList());
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            ViewBag.AgentId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.ShippingScheduleId = new SelectList(db.ShippingSchedules, "ShippingScheduleID", "ShippingScheduleName");
            ViewBag.CargoId = new SelectList(db.Cargoes, "Id", "Name");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BookingName,ShippingScheduleId,CargoId,AgentId,CreatedDate")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AgentId = new SelectList(db.AspNetUsers, "Id", "Email", booking.AgentId);
            ViewBag.ShippingScheduleId = new SelectList(db.ShippingSchedules, "ShippingScheduleID", "ShippingScheduleName", booking.ShippingScheduleId);
            ViewBag.CargoId = new SelectList(db.Cargoes, "Id", "Name", booking.CargoId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgentId = new SelectList(db.AspNetUsers, "Id", "Email", booking.AgentId);
            ViewBag.ShippingScheduleId = new SelectList(db.ShippingSchedules, "ShippingScheduleID", "ShippingScheduleName", booking.ShippingScheduleId);
            ViewBag.CargoId = new SelectList(db.Cargoes, "Id", "Name", booking.CargoId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BookingName,ShippingScheduleId,CargoId,AgentId,CreatedDate")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AgentId = new SelectList(db.AspNetUsers, "Id", "Email", booking.AgentId);
            ViewBag.ShippingScheduleId = new SelectList(db.ShippingSchedules, "ShippingScheduleID", "ShippingScheduleName", booking.ShippingScheduleId);
            ViewBag.CargoId = new SelectList(db.Cargoes, "Id", "Name", booking.CargoId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
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
