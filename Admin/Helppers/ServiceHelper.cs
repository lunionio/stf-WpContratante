using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace Admin.Helppers
{
    public class ServiceHelper
    {
        public ServiceHelper()
        { }

        public T Get<T>(string url)
        {
            var result = string.Empty;
            var request = (HttpWebRequest)WebRequest.Create(url);
            using (var httpResponse = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = httpResponse.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }

            var jss = new JavaScriptSerializer();
            var response = jss.Deserialize<T>(result);

            return response;
        }

        public T Post<T>(string url, object envio)
        {
            var jss = new JavaScriptSerializer();
            var data = jss.Serialize(envio);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var result = string.Empty;
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
                if (string.IsNullOrEmpty(result)
                    || "null".Equals(result.ToLower()))
                {
                    throw new Exception("Ouve um erro durante o processo.");
                }
            }

            var response = jss.Deserialize<T>(result);

            return response;
        }
    }
}