using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MIENNAMPOSTWEB.Models;
using MIENNAMPOSTWEB.Utils;
using System.IO;
using OfficeOpenXml;
using System.Text.RegularExpressions;

namespace MIENNAMPOSTWEB.Controllers
{

    [Authorize(Roles = "user")]
    public class DonHangController : Controller
    {

        MIENNAMPOSTEntities db = new MIENNAMPOSTEntities();
        MNPOSTEntities mnpost = new MNPOSTEntities();

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

        // tu excel
        [HttpGet]
        public ActionResult ImportExcel()
        {
            return View();
        }


        #region
        [HttpPost]
        public ActionResult InsertByExcel(HttpPostedFileBase files, string senderID)
        {

            var sendInfo = mnpost.BS_Customers.Where(p => p.CustomerCode == senderID).FirstOrDefault();

            if (sendInfo == null)
                new ResultWithDataInfo()
                {
                    error = 1,
                    msg = "Thiếu thông tin"
                };

            List<MailerIdentity> mailers = new List<MailerIdentity>();
            var result = new ResultWithDataInfo()
            {
                error = 0,
                msg = "Đã tải",
                data = mailers
            };
            string path = "";
            try
            {

                if (files == null || files.ContentLength <= 0)
                    throw new Exception("Thiếu file Excel");

                string extension = System.IO.Path.GetExtension(files.FileName);

                if (extension.Equals(".xlsx") || extension.Equals(".xls"))
                {
                    string fileSave = "mailersupload" + DateTime.Now.ToString("ddMMyyyyhhmmss") + extension;
                    path = Server.MapPath("~/Temps/" + fileSave);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    files.SaveAs(path);
                    FileInfo newFile = new FileInfo(path);
                    var package = new ExcelPackage(newFile);

                    ExcelWorksheet sheet = package.Workbook.Worksheets[1];

                    int totalRows = sheet.Dimension.End.Row;
                    int totalCols = sheet.Dimension.End.Column;

                    // 
                    int mailerCodeIdx = -1;
                    int receiverIdx = -1;
                    int receiPhoneIdx = -1;
                    int receiAddressIdx = -1;
                    int receiProvinceIdx = -1;
                    int receiDistrictIdx = -1;
                    int mailerTypeIdx = -1;
                    int payTypeIdx = -1;
                    int codIdx = -1;
                    int merchandiseIdx = -1;
                    int weigthIdx = -1;
                    int quantityIdx = -1;
                    int notesIdx = -1;
                    int desIdx = -1;

                    // lay index col tren excel
                    for (int i = 0; i < totalCols; i++)
                    {
                        var colValue = Convert.ToString(sheet.Cells[1, i + 1].Value).Trim();

                        Regex regex = new Regex(@"\((.*?)\)");
                        Match match = regex.Match(colValue);

                        if (match.Success)
                        {
                            string key = match.Groups[1].Value;

                            switch (key)
                            {
                                case "1":
                                    mailerCodeIdx = i + 1;
                                    break;
                                case "2":
                                    receiverIdx = i + 1;
                                    break;
                                case "3":
                                    receiPhoneIdx = i + 1;
                                    break;
                                case "4":
                                    receiAddressIdx = i + 1;
                                    break;
                                case "5":
                                    receiProvinceIdx = i + 1;
                                    break;
                                case "6":
                                    receiDistrictIdx = i + 1;
                                    break;
                                case "8":
                                    mailerTypeIdx = i + 1;
                                    break;
                                case "9":
                                    payTypeIdx = i + 1;
                                    break;
                                case "10":
                                    codIdx = i + 1;
                                    break;
                                case "11":
                                    merchandiseIdx = i + 1;
                                    break;
                                case "12":
                                    weigthIdx = i + 1;
                                    break;
                                case "13":
                                    quantityIdx = i + 1;
                                    break;
                                case "17":
                                    notesIdx = i + 1;
                                    break;
                                case "18":
                                    desIdx = i + 1;
                                    break;
                            }

                        }
                    }

                    // check cac gia tri can
                    if (receiverIdx == -1 || receiAddressIdx == -1 || receiPhoneIdx == -1 || receiDistrictIdx == -1 || receiProvinceIdx == -1 || mailerTypeIdx == 1 ||
                        merchandiseIdx == -1 || weigthIdx == -1)
                        throw new Exception("Thiếu các cột cần thiết");

                    //

                    for (int i = 2; i <= totalRows; i++)
                    {

                        //
                        string receiverPhone = Convert.ToString(sheet.Cells[i, receiPhoneIdx].Value);
                        if (String.IsNullOrEmpty(receiverPhone))
                            throw new Exception("Dòng " + (i) + " cột " + receiPhoneIdx + " : thiếu thông tin");

                        //
                        string receiver = Convert.ToString(sheet.Cells[i, receiverIdx].Value);
                        if (String.IsNullOrEmpty(receiver))
                            throw new Exception("Dòng " + (i) + " cột " + receiverIdx + " : thiếu thông tin");
                        //
                        string receiverAddress = Convert.ToString(sheet.Cells[i, receiAddressIdx].Value);
                        if (String.IsNullOrEmpty(receiverAddress))
                            throw new Exception("Dòng " + (i) + " cột " + receiAddressIdx + " : thiếu thông tin");
                        // 
                        string receiverProvince = Convert.ToString(sheet.Cells[i, receiProvinceIdx].Value);
                        var checkProvince = mnpost.BS_Provinces.Where(p => p.ProvinceCode == receiverProvince).FirstOrDefault();
                        if (checkProvince == null)
                            throw new Exception("Dòng " + (i) + " cột " + receiProvinceIdx + " : sai thông tin");

                        //
                        string receiverDistrict = Convert.ToString(sheet.Cells[i, receiDistrictIdx].Value);
                        var receiverDistrictSplit = receiverDistrict.Split('-');
                        var checkDistrict = mnpost.BS_Districts.Find(receiverDistrictSplit[0]);
                        if (checkDistrict == null)
                            throw new Exception("Dòng " + (i) + " cột " + receiDistrictIdx + " : sai thông tin");

                        string mailerType = Convert.ToString(sheet.Cells[i, mailerTypeIdx].Value);
                        var checkMailerTypeSplit = mailerType.Split('-');
                        var checkMailerType = mnpost.BS_ServiceTypes.Find(checkMailerTypeSplit[0]);
                        if (checkMailerType == null)
                            throw new Exception("Dòng " + (i) + " cột " + mailerTypeIdx + " : sai thông tin");

                        //
                        var mailerPay = payTypeIdx == -1 ? "NGTT" : Convert.ToString(sheet.Cells[i, payTypeIdx].Value);
                        if (payTypeIdx != -1)
                        {
                            var mailerPaySplit = mailerPay.Split('-');
                            mailerPay = mailerPaySplit[0];
                            var checkMailerPay = mnpost.CDatas.Where(p => p.Code == mailerPay && p.CType == "MAILERPAY").FirstOrDefault();
                            mailerPay = checkMailerPay == null ? "NGTT" : checkMailerPay.Code;
                        }

                        // COD
                        var codValue = sheet.Cells[i, codIdx].Value;
                        decimal cod = 0;
                        if (codValue != null)
                        {
                            var isCodeNumber = codIdx == -1 ? false : Regex.IsMatch(codValue.ToString(), @"^\d+$");
                            cod = isCodeNumber ? Convert.ToDecimal(codValue) : 0;
                        }

                        // hang hoa
                        var merchandisType = Convert.ToString(sheet.Cells[i, merchandiseIdx].Value);
                        var merchandisTypeSplit = merchandisType.Split('-');
                        merchandisType = merchandisTypeSplit[0];
                        var checkMerchandisType = mnpost.CDatas.Where(p => p.Code == merchandisType && p.CType == "GOODTYPE").FirstOrDefault();
                        if (checkMerchandisType == null)
                            throw new Exception("Dòng " + (i) + " cột " + merchandiseIdx + " : sai thông tin");

                        // trong luong
                        var weightValue = sheet.Cells[i, weigthIdx].Value;
                        double weight = 0;
                        if (weightValue == null)
                            throw new Exception("Dòng " + (i) + " cột " + weigthIdx + " : sai thông tin");
                        else
                        {
                            var isWeightNumber = Regex.IsMatch(weightValue.ToString(), @"^\d+$");
                            weight = isWeightNumber ? Convert.ToDouble(sheet.Cells[i, weigthIdx].Value) : 0;
                        }

                        // so luong
                        var quantityValue = sheet.Cells[i, quantityIdx].Value;
                        var isQuantityNumber = quantityIdx == -1 ? false : Regex.IsMatch(quantityValue == null ? "0" : quantityValue.ToString(), @"^\d+$");
                        var quantity = isQuantityNumber ? Convert.ToInt32(quantityValue) : 0;
                        //
                        string notes = notesIdx == -1 ? "" : Convert.ToString(sheet.Cells[i, notesIdx].Value);

                        //
                        string describe = desIdx == -1 ? "" : Convert.ToString(sheet.Cells[i, desIdx].Value);

                        var price = mnpost.CalPrice(weight, senderID, checkProvince.ProvinceID, checkMailerType.ServiceID, "BCQ3", DateTime.Now.ToString("yyyy-MM-dd")).FirstOrDefault();
                        var codPrice = 0;


                        mailers.Add(new MailerIdentity()
                        {
                            MailerID = "",
                            COD = cod,
                            PriceCoD = codPrice,
                            HeightSize = 0,
                            LengthSize = 0,
                            MailerDescription = describe,
                            MailerTypeID = checkMailerType.ServiceID,
                            MerchandiseID = merchandisType,
                            MerchandiseValue = cod,
                            Notes = notes,
                            PaymentMethodID = mailerPay,
                            PriceDefault = price,
                            Amount = price + codPrice,
                            PriceService = 0,
                            Quantity = quantity,
                            RecieverAddress = receiverAddress,
                            RecieverDistrictID = checkDistrict.DistrictID,
                            RecieverName = receiver,
                            RecieverPhone = receiverPhone,
                            RecieverProvinceID = checkProvince.ProvinceID,
                            Weight = weight,
                            WidthSize = 0,
                            SenderID = senderID,
                            SenderAddress = sendInfo.Address,
                            SenderDistrictID = sendInfo.DistrictID,
                            SenderName = sendInfo.CustomerName,
                            SenderPhone = sendInfo.Phone,
                            SenderProvinceID = sendInfo.ProvinceID
                        });

                    }
                    // xoa file temp
                    package.Dispose();
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                }

                result.data = mailers;
            }
            catch (Exception e)
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                result.error = 1;
                result.msg = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public ActionResult AddListOrder(string data)
        {
            var res = RequestHandle.SendPost(APISource.ROOTURL + "api/mailer/AddListMailer", data, true);

            return Json(res, JsonRequestBehavior.AllowGet);
        }

    }
}