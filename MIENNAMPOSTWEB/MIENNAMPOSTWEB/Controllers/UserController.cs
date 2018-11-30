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
    public class UserController : Controller
    {
        MIENNAMPOSTEntities db = new MIENNAMPOSTEntities();

        public ActionResult Show()
        {


            return View();
        }

        [HttpGet]
        public ActionResult GetInfo()
        {
            var findUser = db.AspNetUsers.Where(p => p.UserName == User.Identity.Name).FirstOrDefault();

            var res = RequestHandle.SendGet(APISource.ROOTURL + "api/customer/customerinfo?cusId=" + findUser.IDClient, true);

            return Json(res, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult SendUpdate(string data)
        {

            var res = RequestHandle.SendPost(APISource.ROOTURL + "api/customer/UpdateCustomer", data, true);

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        //
        [HttpGet]
        public ActionResult GetProvince()
        {
            var res = RequestHandle.SendGet(APISource.ROOTURL + "api/basedata/GetProvince", false);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDistrict(string provinceId)
        {
            var res = RequestHandle.SendGet(APISource.ROOTURL + "api/basedata/GetDistrict?provinceID=" + provinceId, false);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetWard(string districtId)
        {
            var res = RequestHandle.SendGet(APISource.ROOTURL + "api/basedata/GetWard?districtID=" + districtId, false);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}