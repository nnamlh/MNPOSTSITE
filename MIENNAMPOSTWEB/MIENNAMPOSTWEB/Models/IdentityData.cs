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

}