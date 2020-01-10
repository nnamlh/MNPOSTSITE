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
        MNPOSTEntities dbdata = new MNPOSTEntities();

        public ActionResult Index()
        {   // tinh thanh
            ViewBag.Provinces = GetProvinceDatas("", "province");
            // danh sach phu phi
            ViewBag.MailerTypes = dbdata.BS_ServiceTypes.Select(p => new CommonData()
            {
                code = p.ServiceID,
                name = p.ServiceName
            }).ToList();

            return View();
        }

        [HttpGet]
        public ActionResult GetDichVues()
        {
            /*
            var data = db.Articles.Where(p => p.GroupArticle.Id == "DICHVU").Select(p=> new
            {
                code = p.Code,
                content = p.Describe,
                title = p.Title
            }).ToList();
            */


            return Json(new List<Article>(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetBaiViets()
        {
            var data = db.Articles.OrderByDescending(p => p.CreateDate).Where(p => p.GroupArticle.Id == "TINTUC").Select(p => new
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



        [HttpGet]
        public ActionResult GetProvinces(string parentId, string type)
        {
            return Json(GetProvinceDatas(parentId, type), JsonRequestBehavior.AllowGet);
        }
        public List<AddressCommom> GetProvinceDatas(string parentId, string type)
        {
            if (type == "district")
            {
                return dbdata.BS_Districts.Where(p => p.ProvinceID == parentId).Select(p => new AddressCommom()
                {
                    code = p.DistrictID,
                    name = p.DistrictName,
                    vsvx = p.VSVS
                }).ToList();
            }
            else if (type == "ward")
            {
                return dbdata.BS_Wards.Where(p => p.DistrictID == parentId).Select(p => new AddressCommom()
                {
                    code = p.WardID,
                    name = p.WardName
                }).ToList();
            }
            else if (type == "province")
            {
                return dbdata.BS_Provinces.Select(p => new AddressCommom()
                {
                    code = p.ProvinceID,
                    name = p.ProvinceName
                }).ToList();
            }
            else
            {
                return new List<AddressCommom>();
            }

        }


        [HttpPost]
        public JsonResult CalBillPrice(float weight = 0, string provinceId = "", string serviceTypeId = "",float cod = 0, string districtId = "")
        {
            decimal? price = 0;
            if (cod > 0)
            {
                var findDitrict = dbdata.BS_Districts.Where(p => p.DistrictID == districtId).FirstOrDefault();
                int? vsvx = findDitrict == null ? 1 : (findDitrict.VSVS == true ? 1 : 0);

                price = dbdata.CalPriceCOD(weight, "", provinceId, "CD", "BCQ3", DateTime.Now.ToString("yyyy-MM-dd"), vsvx, serviceTypeId == "ST" ? "CODTK" : "CODN").FirstOrDefault();
            }
            else
            {
                price = dbdata.CalPrice(weight, "", provinceId, serviceTypeId, "BCQ3", DateTime.Now.ToString("yyyy-MM-dd")).FirstOrDefault();
            }

            return Json(new { price = price, codPrice = 0 }, JsonRequestBehavior.AllowGet);
        }
    }
}