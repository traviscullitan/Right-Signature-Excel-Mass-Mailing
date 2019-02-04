using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace RightSignature
{
    class RightSignatureRequest
    {
        private string api_token = "";
        
        public RightSignatureRequest()
        {
            NameValueCollection settings = ConfigurationManager.AppSettings;
            string api_key = settings["api_key"];
            //Convert api_key to BASE64 encoding
            api_token = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(api_key));

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public string SendTempate(string id, string json_tempate_values)
        {
            try
            {
                string URL = "https://api.rightsignature.com/public/v1/reusable_templates/" + id + "/send_document";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Headers.Add("authorization", "Basic " + api_token);
                request.Method = "POST";
                request.ContentType = "application/json";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json_tempate_values);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                using (System.IO.Stream st = request.GetResponse().GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(st))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            catch(Exception my_e)
            {
                throw new Exception(my_e.ToString());
            }
        }
        public dynamic GetTemplateMergeFields(string myguid)
        {
            string URL = "https://api.rightsignature.com/public/v1/reusable_templates/" + myguid;
            try
            {
                dynamic jsonReponseDesialized = RightSignatureRequestGet(URL);
                dynamic template = jsonReponseDesialized["reusable_template"];
                return template;
            }
            catch (Exception my_e)
            {
                string my_error = "Unable to get list of merge fields for template" +myguid + " from RightSignature." + my_e.ToString();
                throw new InvalidOperationException(my_error);
            }
        }
        public dynamic GetReusableTemplates()
        {
            try
            {
                string URL = "https://api.rightsignature.com/public/v1/reusable_templates";
                dynamic jsonReponseDesialized = RightSignatureRequestGet(URL);
                dynamic templates = jsonReponseDesialized["reusable_templates"];
                return templates;
            }
            catch (Exception my_e)
            {
                string my_error = "Unable to get list of templates from RightSignature. Exiting Program. Error: " + my_e.ToString();
                throw new InvalidOperationException(my_error);
            }

        }
        private dynamic RightSignatureRequestGet(string URL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Headers.Add("authorization", "Basic " + api_token);
            request.Method = "GET";

            string jsonResponse;

            using (System.IO.Stream st = request.GetResponse().GetResponseStream())
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(st))
                {
                    jsonResponse = sr.ReadToEnd();
                }
            }

            dynamic jsonReponseDesialized = JsonConvert.DeserializeObject(jsonResponse);
            return jsonReponseDesialized;
        }
    }
}
