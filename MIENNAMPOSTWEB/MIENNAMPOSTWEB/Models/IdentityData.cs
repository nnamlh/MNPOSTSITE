using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIENNAMPOSTWEB.Models
{
    public class IdentityData
    {
    }
    public class ItemCommon
    {
        public string code { get; set; }

        public string name { get; set; }
    }

    public class AddCustomerSend
    {
        public string email { get; set; }


        public string phone { get; set; }

        public string fullName { get; set; }

        public string clientUser { get; set; }
    }

    public class AddCustomerResult : ResultInfo
    {
        public string data { get; set; }
    }


    public class ResultInfo
    {
        public int error { get; set; }

        public string msg { get; set; }

    }

    public class ResultWithDataInfo
    {
        public int error { get; set; }

        public string msg { get; set; }

        public Object data { get; set; }

    }
    public class CommonData
    {
        public string code { get; set; }

        public string name { get; set; }
    }

    public class AddressCommom : CommonData
    {
        public bool? vsvx { get; set; }
    }

    public class MailerIdentity
    {

        /*
          'MailerID': '', 'SenderID': '', 'SenderName': '', 'SenderAddress': '', 'SenderWardID': '', 'SenderDistrictID': '',
                'SenderProvinceD:\work\Project\007MNPost\MNPOST\MNPOST\Models\IdentityMailer.csID': '', 'SenderPhone': '', 'RecieverName': ''
                , 'RecieverAddress': '', 'RecieverWardID': '', 'RecieverDistrictID': '', 'RecieverProvinceID': '',
                'RecieverPhone': '', 'Weight': 0.01, 'Quantity': 1, 'PaymentMethodID': 'NGTT', 'MailerTypeID': 'SN',
                'PriceService': 0, 'MerchandiseID': 'H', 'Services': [], 'MailerDescription': '', 'Notes': '', 'COD': 0, 'LengthSize': 0, 'WidthSize': 0, 'HeightSize': 0, 'PriceMain': 0, 'CODPrice': 0,
                'PriceDefault': 0
         */

        public string MailerID { get; set; }
        public string SenderID { get; set; }
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
        public string SenderWardID { get; set; }
        public string SenderDistrictID { get; set; }
        public string SenderProvinceID { get; set; }
        public string SenderPhone { get; set; }
        public string RecieverName { get; set; }
        public string RecieverAddress { get; set; }
        public string RecieverWardID { get; set; }
        public string RecieverDistrictID { get; set; }
        public string RecieverProvinceID { get; set; }
        public string RecieverPhone { get; set; }
        public double? Weight { get; set; }
        public int Quantity { get; set; }
        public string PaymentMethodID { get; set; }
        public string MailerTypeID { get; set; }
        public decimal? PriceService { get; set; }
        public string MerchandiseID { get; set; }
        public string MailerDescription { get; set; }
        public string Notes { get; set; }
        public decimal? COD { get; set; }
        public double? LengthSize { get; set; }
        public double? WidthSize { get; set; }
        public double? HeightSize { get; set; }
        public decimal? Amount { get; set; }
        public decimal? PriceCoD { get; set; }
        public decimal? PriceDefault { get; set; }

        public decimal? MerchandiseValue { get; set; }

        public int? CurrentStatusID { get; set; }

    }

}