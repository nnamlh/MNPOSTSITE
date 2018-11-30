using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MIENNAMPOSTWEB.Models;

namespace MIENNAMPOSTWEB.Controllers
{
    public class HomeController : Controller
    {

        MIENNAMPOSTEntities db = new MIENNAMPOSTEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetDichVues()
        {
            var data = db.Articles.Where(p => p.GroupArticle.Id == "DICHVU").Select(p=> new
            {
                code = p.Code,
                content = p.Describe,
                title = p.Title
            }).ToList();



            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetBaiViets()
        {
            var data = db.Articles.OrderByDescending(p => p.CreateDate).Where(p => p.GroupArticle.Id == "BAIVIET").Select(p => new
            {
                code = p.Code,
                content = p.Describe,
                title = p.Title,
                image = p.Thumbnail
            }).Take(3).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Contact()
        {
            return View();
        }
    }
}