using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Helpers;
using MIENNAMPOSTWEB.Models;

namespace MIENNAMPOSTWEB.Utils
{
    public class RequestHandle
    {
        public static string SendGet(string url, bool isAuthen, int idx = 1)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            if (isAuthen)
            {
                request.Headers["Authorization"] = GetToken(false);
            }

            var httpResponse = (HttpWebResponse)request.GetResponse();

            if (httpResponse.StatusCode.ToString() == "401")
            {
                GetToken(true);
                if (idx == 3)
                {
                    return "";
                }
                else
                {
                    return SendGet(url, isAuthen, idx++);
                }
            }
            else
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return result;
                }

            }


        }

        public static string SendPost(string url, string json, bool isAuthen, int idx = 1)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";
            if (isAuthen)
            {
                request.Headers["Authorization"] = GetToken(false);
            }

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)request.GetResponse();

            if (httpResponse.StatusCode.ToString() == "401")
            {
                GetToken(true);
                if (idx == 3)
                {
                    return "";
                }
                else
                {
                    return SendGet(url, isAuthen, idx++);
                }
            }
            else
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return result;
                }

            }

        }

        public static string GetToken(bool requestNew)
        {
            MIENNAMPOSTEntities db = new MIENNAMPOSTEntities();

            var data = db.TokenDatas.Where(p => p.UserName == "mnwebsite").FirstOrDefault();

            if (data == null)
            {
                data = new TokenData()
                {
                    UserName = "mnwebsite",
                    CreateTime = DateTime.Now,
                    Id = Guid.NewGuid().ToString(),
                    Token = ""
                };

                db.TokenDatas.Add(data);
                db.SaveChanges();

                requestNew = true;
            }


            if (requestNew)
            {
                data.Token = RequetToken();
                data.CreateTime = DateTime.Now;
                db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                var hours = DateTime.Now.Subtract(data.CreateTime.Value).TotalHours;

                if (hours >= 24)
                {
                    data.Token = RequetToken();
                    data.CreateTime = DateTime.Now;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }


            return "bearer " + data.Token;

        }

        public static string RequetToken()
        {

            var postData = "grant_type=password&username=mnwebsite&password=mnpost@123";

            var data = Encoding.ASCII.GetBytes(postData);

            string url = APISource.ROOTURL + "mntoken";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            var res = Json.Decode(responseString);

            return res.access_token;
        }
    }


    public class APISource
    {
        //public static string ROOTURL = "http://noiboapi.miennampost.vn/";
        public static string ROOTURL = "http://localhost:1519/";
    }
}