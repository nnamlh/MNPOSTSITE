using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MIENNAMPOSTWEB.Models;

namespace MIENNAMPOSTWEB.Controllers
{
    public class BaiVietController : Controller
    {
        MIENNAMPOSTEntities db = new MIENNAMPOSTEntities();
        // GET: BaiViet
        public ActionResult GioiThieu()
        {
            var data = db.Articles.Where(p => p.GroupArticle.Id == "GIOITHIEU").FirstOrDefault();


            if (data == null)
            {
                return Redirect("/error");
            }
            else
            {
                return View(data);
            }
        }
    }
}