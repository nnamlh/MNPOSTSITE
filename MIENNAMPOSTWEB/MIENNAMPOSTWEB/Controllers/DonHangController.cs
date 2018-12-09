using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MIENNAMPOSTWEB.Models;
using MIENNAMPOSTWEB.Utils;

namespace MIENNAMPOSTWEB.Controllers
{

    [Authorize(Roles = "user")]
    public class DonHangController : Controller
    {

        MIENNAMPOSTEntities db = new MIENNAMPOSTEntities();

        // GET: Order
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult ShowList()
        {
            var findUSer = db.AspNetUsers.Where(p => p.UserName == User.Identity.Name).FirstOrDefault();

            ViewBag.InfoID = findUSer.IDClient;

            return View();
        }

        [HttpPost]
        public ActionResult GetData(string data)
        {
            var res = RequestHandle.SendPost(APISource.ROOTURL + "api/mailer/GetMailers", data, true);

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FindMailer(string mailerId)
        {
            var res = RequestHandle.SendGet(APISource.ROOTURL + "api/mailer/FindMailer?mailerId=" + mailerId, true);

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSetting()
        {
            var res = RequestHandle.SendGet(APISource.ROOTURL + "api/basedata/GetMailerSetting", false);

            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CalBillPrice(float weight = 0, string customerId = "", string provinceId = "", string serviceTypeId = "")
        {

            var url = APISource.ROOTURL + "api/basedata/CalBillPrice?weight=" + weight + "&customerId=" + customerId + "&provinceId=" + provinceId + "&serviceTypeId=" + serviceTypeId;

            var res = RequestHandle.SendGet(url, false);

            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult AddOrder(string data)
        {
            var res = RequestHandle.SendPost(APISource.ROOTURL + "api/mailer/AddMailer", data, true);

            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ChiTiet(string id = "")
        {
            ViewBag.MailerId = id;
            return View();
        }

        [HttpPost]
        public ActionResult HuyDon(string data)
        {
            var res = RequestHandle.SendPost(APISource.ROOTURL + "api/mailer/AddMailer", data, true);

            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}