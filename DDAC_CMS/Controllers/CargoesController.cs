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
    public class CargoesController : Controller
    {
        private CMS_DBEntities db = new CMS_DBEntities();

        // GET: Cargoes
        public ActionResult Index()
        {
            var cargoes = db.Cargoes.Include(c => c.AspNetUser).Include(c => c.Customer).Include(c => c.Port).Include(c => c.Port1);
            return View(cargoes.ToList());
        }

        // GET: Cargoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cargo cargo = db.Cargoes.Find(id);
            if (cargo == null)
            {
                return HttpNotFound();
            }
            return View(cargo);
        }

        // GET: Cargoes/Create
        public ActionResult Create()
        {
            ViewBag.AgentId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name");
            ViewBag.LocationId = new SelectList(db.Ports, "PortID", "PortName");
            ViewBag.DestinationId = new SelectList(db.Ports, "PortID", "PortName");
            return View();
        }

        // POST: Cargoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,LocationId,DestinationId,Quantity,TotalWeight,CustomerId,AgentId,RegisteredDate")] Cargo cargo)
        {
            if (ModelState.IsValid)
            {
                db.Cargoes.Add(cargo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AgentId = new SelectList(db.AspNetUsers, "Id", "Email", cargo.AgentId);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", cargo.CustomerId);
            ViewBag.LocationId = new SelectList(db.Ports, "PortID", "PortName", cargo.LocationId);
            ViewBag.DestinationId = new SelectList(db.Ports, "PortID", "PortName", cargo.DestinationId);
            return View(cargo);
        }

        // GET: Cargoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cargo cargo = db.Cargoes.Find(id);
            if (cargo == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgentId = new SelectList(db.AspNetUsers, "Id", "Email", cargo.AgentId);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", cargo.CustomerId);
            ViewBag.LocationId = new SelectList(db.Ports, "PortID", "PortName", cargo.LocationId);
            ViewBag.DestinationId = new SelectList(db.Ports, "PortID", "PortName", cargo.DestinationId);
            return View(cargo);
        }

        // POST: Cargoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,LocationId,DestinationId,Quantity,TotalWeight,CustomerId,AgentId,RegisteredDate")] Cargo cargo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cargo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AgentId = new SelectList(db.AspNetUsers, "Id", "Email", cargo.AgentId);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", cargo.CustomerId);
            ViewBag.LocationId = new SelectList(db.Ports, "PortID", "PortName", cargo.LocationId);
            ViewBag.DestinationId = new SelectList(db.Ports, "PortID", "PortName", cargo.DestinationId);
            return View(cargo);
        }

        // GET: Cargoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cargo cargo = db.Cargoes.Find(id);
            if (cargo == null)
            {
                return HttpNotFound();
            }
            return View(cargo);
        }

        // POST: Cargoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cargo cargo = db.Cargoes.Find(id);
            db.Cargoes.Remove(cargo);
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
