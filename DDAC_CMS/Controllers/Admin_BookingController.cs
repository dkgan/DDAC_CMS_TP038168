using DDAC_CMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DDAC_CMS.Controllers
{
    public class Admin_BookingController : Controller
    {
        private CMS_DBEntities db = new CMS_DBEntities();

        // GET: Admin_Booking
        public ActionResult ViewBooking()
        {
            return View(db.Bookings.ToList());
        }

        public ActionResult ViewCargo()
        {
            return View(db.Cargoes.ToList());
        }

        public ActionResult UpdateBooking(int ? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateBooking([Bind(Include = "Id,BookingName,ShippingScheduleId,CargoId,AgentId,CreatedDate,Status")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewBooking");
            }
            ViewBag.AgentId = new SelectList(db.AspNetUsers, "Id", "Email", booking.AgentId);
            ViewBag.ShippingScheduleId = new SelectList(db.ShippingSchedules, "ShippingScheduleID", "ShippingScheduleName", booking.ShippingScheduleId);
            ViewBag.CargoId = new SelectList(db.Cargoes, "Id", "Name", booking.CargoId);
            return View(booking);
        }
    }
}