using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MIENNAMPOSTWEB.Models;

namespace MIENNAMPOSTWEB.Controllers
{
    public class DichVuController : Controller
    {
        MIENNAMPOSTEntities db = new MIENNAMPOSTEntities();
        // GET: DichVu
        public ActionResult Xem(string id="")
        {

            var data= db.Articles.Where(p => p.GroupArticle.Id == "DICHVU").ToList();
            ViewBag.DichVues = data;
            Article dv = null;
            if (String.IsNullOrEmpty(id))
            {
                dv = data.FirstOrDefault();
           
            } else
            {
                dv = data.Where(p => p.Code == id).FirstOrDefault();
            }
            
            if(dv == null)
            {
                return Redirect("/error");
            } else
            {
                return View(dv);
            }

        }
    }
}