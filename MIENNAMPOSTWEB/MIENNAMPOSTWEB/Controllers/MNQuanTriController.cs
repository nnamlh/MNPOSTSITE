using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MIENNAMPOSTWEB.Models;
using System.Text.RegularExpressions;
using System.Text;
using PagedList;

namespace MIENNAMPOSTWEB.Controllers
{
    [Authorize(Roles = "admin")]
    public class MNQuanTriController : Controller
    {

        MIENNAMPOSTEntities db = new MIENNAMPOSTEntities();

        // GET: MNQuanTri
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TaoBaiViet()
        {
            var groups = db.GroupArticles.ToList();

            return View(groups);
        }


        [HttpGet]
        public ActionResult GetUrl(string title)
        {
            return Json(new { Url = extUrl(title) }, JsonRequestBehavior.AllowGet);
        }


        private string extUrl(string title)
        {
            if (String.IsNullOrEmpty(title))
                title = "chu ky so viettel";

            var url = convertToUnSign3(title);
            url = url.Replace(" ", "-");
            var check = db.Articles.Where(p => p.Code == url).FirstOrDefault();
            if (check != null)
                url += "-" + DateTime.Now.Date.ToString("ddMMyyyyhhmm");

            return url;
        }

        private string convertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public ActionResult ShowBaiViet(int? page, string danhmuc = "", string search = "")
        {
            ViewBag.DanhMuc = db.GroupArticles.ToList();

            ViewBag.Search = search;
            ViewBag.DM = danhmuc;

            int pageSize = 20;
            int pageNumber = (page ?? 1);

            var baiviet = db.Articles.Where(p => p.GroupId.Contains(danhmuc) && p.Title.Contains(search)).OrderByDescending(p => p.CreateDate).ToPagedList(pageNumber, pageSize);

            return View(baiviet);
        }

        public ActionResult Modify(string id)
        {
            var check = db.Articles.Find(id);
            if (check == null)
                return View("Không thể xem");

            ViewBag.DanhMuc = db.GroupArticles.ToList();

            return View(check);
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TaoBaiViet(Article article, string contentEditor, HttpPostedFileBase image)
        {

            var checkUrl = db.Articles.Where(p => p.Code == article.Code).FirstOrDefault();
            if (checkUrl != null)
                article.Code = extUrl(article.Title);


            article.Id = Guid.NewGuid().ToString();
            article.Thumbnail = urlUpLoad(image);
            article.CreateDate = DateTime.Now;
            article.Content = contentEditor;

            db.Articles.Add(article);

            db.SaveChanges();

            return RedirectToAction("taobaiviet", "mnquantri");
        }

        public string urlUpLoad(HttpPostedFileBase files)
        {
            string urlThumbnail = "";
            string dfolder = DateTime.Now.Date.ToString("ddMMyyyy");
            // size thumbnail: 500x344
            if (files != null)
            {

                string ImageName = files.FileName;

                string fsave = "~/images/" + dfolder;

                bool exists = System.IO.Directory.Exists(Server.MapPath(fsave));

                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(fsave));

                // string path = System.IO.Path.Combine(Server.MapPath(fsave));

                ImageUpload imageUpload = new ImageUpload
                {
                    Width = 500,
                    isSacle = false,
                    UploadPath = fsave
                };

                ImageResult imageResult = imageUpload.RenameUploadFile(files);

                if (imageResult.Success)
                {
                    urlThumbnail = "/images/" + dfolder + "/" + imageResult.ImageName;
                }
            }
            return urlThumbnail;
        }
    

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Modify(string Id, string Title, string DanhMucId, string Url, string ShortContent, string contentEditor, HttpPostedFileBase image)
        {
            var check = db.Articles.Find(Id);
            if (check == null)
                return View("Không thể xem");

            check.Title = Title;

            if (check.Code != Url)
            {
                var checkUrl = db.Articles.Where(p => p.Code == Url).FirstOrDefault();
                if (checkUrl != null)
                    Url = extUrl(Title);

                check.Code = Url;
            }

            if (image != null)
                check.Thumbnail = urlUpLoad(image);

            check.Describe = ShortContent;
            check.Content = contentEditor;
            check.CreateDate = DateTime.Now;

            check.GroupId = DanhMucId;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();


            return RedirectToAction("modify", "mnquantri", new { id = Id });
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            var check = db.Articles.Find(id);
            if (check == null)
                return View("Không thể xem");

            db.Articles.Remove(check);

            db.SaveChanges();

            return RedirectToAction("showbaiviet", "mnquantri");
        }
    }
}