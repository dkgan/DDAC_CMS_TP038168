using DDAC_CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DDAC_CMS.Controllers
{
    [Authorize(Roles ="Agents")]
    public class AgentsController : Controller
    {
        private CMS_DBEntities db = new CMS_DBEntities();

        // GET: Agents
        public ActionResult Index()
        {
            return View();
        }
         
        public ActionResult ViewBookingStatus()
        {

            return View(db.Bookings.ToList());
        }
    }
}