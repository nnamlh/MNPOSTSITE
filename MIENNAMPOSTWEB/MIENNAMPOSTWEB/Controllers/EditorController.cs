using MIENNAMPOSTWEB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIENNAMPOSTWEB.Controllers
{

    [Authorize(Roles = "admin")]
    public class EditorController : Controller
    {
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase upload)
        {
            if(upload != null)
            {
                string dfolder = DateTime.Now.Date.ToString("ddMMyyyy");
                string targetFolder = "/MNFiles/" + dfolder + "/";
                bool exists = System.IO.Directory.Exists(Server.MapPath(targetFolder));

                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(targetFolder));

                string name = DateTime.Now.ToString("ddMMyyyyHHmmss") + upload.FileName;
                string targetPath = Path.Combine(Server.MapPath(targetFolder), name);
                upload.SaveAs(targetPath);

                return Json(new
                {
                    uploaded = 1,
                    fileName = name,
                    url = targetFolder + name
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                uploaded = 0,
                fileName = "",
                url = "",
                message = "Lỗi"
            }, JsonRequestBehavior.AllowGet);

        }





        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            string dfolder = DateTime.Now.Date.ToString("ddMMyyyy");
            string url = "/images/" + dfolder + "/"; // url to return

            string name = "";
            if (upload != null)
            {
                string ImageName = upload.FileName;

                string fsave = "~/images/" + dfolder;

                bool exists = System.IO.Directory.Exists(Server.MapPath(fsave));

                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(fsave));

                string path = System.IO.Path.Combine(Server.MapPath(fsave), ImageName);

                ImageUpload imageUpload = new ImageUpload
                {
                    Width = 800,
                    isSacle = false,
                    UploadPath = fsave
                };
                ImageResult imageResult = imageUpload.RenameUploadFile(upload);

                if (imageResult.Success)
                {

                    url = url + ImageName;
                    name = imageResult.ImageName;
                }
                else
                {

                    url = "";
                }
            }
            else
            {

                url = "";
            }
            //   string output = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + url + "\", \"" + message + "\");</script></body></html>";
            return Json(new {
                uploaded = 1,
                fileName = name,
                url = url
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult uploadPartial()
        {
            var appData = Server.MapPath("~/images");
            var images = Directory.GetFiles(appData).Select(x => new ImageViewModel
            {
                Url = Url.Content("/images" + Path.GetFileName(x))
            });

            return View(images);
        }
    }
}