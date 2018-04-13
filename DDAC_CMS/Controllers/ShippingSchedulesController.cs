using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DDAC_CMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;

namespace DDAC_CMS.Controllers
{
    public class ShippingSchedulesController : Controller
    {
        private CMS_DBEntities db = new CMS_DBEntities();

        // GET: ShippingSchedules
        public ActionResult Index()
        {
            var shippingSchedules = db.ShippingSchedules.Include(s => s.Port).Include(s => s.Port1).Include(s => s.Vessel).Include(s => s.AspNetUser);
            return View(shippingSchedules.ToList());
        }

        // GET: ShippingSchedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingSchedule shippingSchedule = db.ShippingSchedules.Find(id);
            if (shippingSchedule == null)
            {
                return HttpNotFound();
            }
            return View(shippingSchedule);
        }

        // GET: ShippingSchedules/Create
        public ActionResult Create()
        {
            ViewBag.ArrivalPortID = new SelectList(db.Ports, "PortID", "PortName");
            ViewBag.DeparturePortID = new SelectList(db.Ports, "PortID", "PortName");
            ViewBag.VesselID = new SelectList(db.Vessels, "VesselID", "VesselName");
            
            ViewBag.AdminID = new SelectList(db.AspNetUsers, "Id", "FullName");
            return View();
        }

        // POST: ShippingSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShippingScheduleID,ShippingScheduleName,VesselID,DeparturePortID,ArrivalPortID,DepartureDateTime,ArrivalDateTime,SpaceAvailable,AdminID")] ShippingSchedule shippingSchedule)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            if (ModelState.IsValid)
            {
                db.ShippingSchedules.Add(shippingSchedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArrivalPortID = new SelectList(db.Ports, "PortID", "PortName", shippingSchedule.ArrivalPortID);
            ViewBag.DeparturePortID = new SelectList(db.Ports, "PortID", "PortName", shippingSchedule.DeparturePortID);
            ViewBag.VesselID = new SelectList(db.Vessels, "VesselID", "VesselName", shippingSchedule.VesselID);
            
            ViewBag.AdminID = new SelectList(db.AspNetUsers, "Id", "FullName", shippingSchedule.AdminID);
            return View(shippingSchedule);
        }

        // GET: ShippingSchedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingSchedule shippingSchedule = db.ShippingSchedules.Find(id);
            if (shippingSchedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArrivalPortID = new SelectList(db.Ports, "PortID", "PortName", shippingSchedule.ArrivalPortID);
            ViewBag.DeparturePortID = new SelectList(db.Ports, "PortID", "PortName", shippingSchedule.DeparturePortID);
            ViewBag.VesselID = new SelectList(db.Vessels, "VesselID", "VesselName", shippingSchedule.VesselID);
            ViewBag.AdminID = new SelectList(db.AspNetUsers, "Id", "FullName", shippingSchedule.AdminID);
            return View(shippingSchedule);
        }

        // POST: ShippingSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShippingScheduleID,ShippingScheduleName,VesselID,DeparturePortID,ArrivalPortID,DepartureDateTime,ArrivalDateTime,SpaceAvailable,AdminID")] ShippingSchedule shippingSchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shippingSchedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArrivalPortID = new SelectList(db.Ports, "PortID", "PortName", shippingSchedule.ArrivalPortID);
            ViewBag.DeparturePortID = new SelectList(db.Ports, "PortID", "PortName", shippingSchedule.DeparturePortID);
            ViewBag.VesselID = new SelectList(db.Vessels, "VesselID", "VesselName", shippingSchedule.VesselID);
            ViewBag.AdminID = new SelectList(db.AspNetUsers, "Id", "FullName", shippingSchedule.AdminID);
            return View(shippingSchedule);
        }

        // GET: ShippingSchedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingSchedule shippingSchedule = db.ShippingSchedules.Find(id);
            if (shippingSchedule == null)
            {
                return HttpNotFound();
            }
            return View(shippingSchedule);
        }

        // POST: ShippingSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShippingSchedule shippingSchedule = db.ShippingSchedules.Find(id);
            db.ShippingSchedules.Remove(shippingSchedule);
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
