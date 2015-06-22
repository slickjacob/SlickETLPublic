using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace SlickETL.Web.Utility
{
    public class ReCaptcha
    {
        class recaptchaModel
        {
            public string secret { get; set; }
            public string response { get; set; }
            public string remoteip { get; set; }
        }

        class recaptchaResult
        {
            public bool success { get; set; }
        }

        public static bool VerifyUserResponse(string response, string userip)
        {
            var googleURL = "https://www.google.com/recaptcha/api/siteverify";
            var uri = string.Format("{0}?secret={1}&response={2}&remoteip={3}",googleURL,"6LdXrggTAAAAANdNIJA-kfZQqlyM-XeSBKG2L8DV",response,userip);
            var wr = WebRequest.Create(uri);
            wr.Method = "POST";
            wr.ContentLength = 0;
            WebResponse resp = wr.GetResponse();
            if(((HttpWebResponse)resp).StatusCode != HttpStatusCode.OK){
                return false;
            }
            var rawGoogleResponse = new StreamReader(resp.GetResponseStream()).ReadToEnd();
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<recaptchaResult>(rawGoogleResponse);
            return result.success;


        }
    }
}