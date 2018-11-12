using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace UtilZ.ParaService.ClientProx
{
    public class Client
    {
        private static void TestPost()
        {
            try
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://localhost:5001/v1/api/Para");
                request.Method = "POST";
                //request.Accept = "application/json, text/javascript, */*"; //"text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json; charset=utf-8";

                var obj = new login { action = "login", email = "din@gmail.com", password = "qwerty" };
                string jsonString = JsonConvert.SerializeObject(obj, Formatting.Indented);
                byte[] buffer = encoding.GetBytes(jsonString);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
                {
                    string ret = reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                var res = (HttpWebResponse)ex.Response;
                StringBuilder sb = new StringBuilder();
                StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                sb.Append(sr.ReadToEnd());
                //string ssb = sb.ToString();
                throw new Exception(sb.ToString());
            }
        }

        private static void TestGet()
        {
            string url = @"https://localhost:5001/v1/api/Para/张海能";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.Credentials = new NetworkCredential(this._userName, this._password);
            //request.KeepAlive = false;
            //request.Proxy = this._proxy;
            request.Method = WebRequestMethods.Http.Get;
            using (Stream stream = request.GetResponse().GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                string str = sr.ReadToEnd();
            }
        }
    }
}
