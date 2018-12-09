using MIENNAMPOSTWEB.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIENNAMPOSTWEB.Controllers
{
    public class TraCuuController : Controller
    {
        // GET: TraCuu
        public ActionResult Xem(string id = "")
        {

            ViewBag.TrackId = id;
            return View();
        }

        [HttpGet]
        public ActionResult GetInfo(string mailerId)
        {
            var data = RequestHandle.SendGet(APISource.ROOTURL + "api/Track/Find?mailerId=" + mailerId, false);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}