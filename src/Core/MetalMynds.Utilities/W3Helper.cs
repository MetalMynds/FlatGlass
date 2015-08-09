using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace MetalMynds.Utilities
{
    public class W3Helper
    {

        public enum Method
        {
            Post,
            Get
        }

        public static Boolean TryBasicAuthRequest(String UserName, String Password, String Url, String Request, String ContentType, Method Method, int TimeoutSeconds, out String Response, out String Error)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                request.Timeout = TimeoutSeconds * 1000;

                if (UserName != String.Empty && Password != String.Empty)
                {

                    request.Credentials = AuthenticationHelper.GetBasicAuthorisation(UserName, Password, Url, null);

                }

                request.ContentType = ContentType;

                switch (Method)
                {
                    case Method.Get:
                        request.Method = "GET";
                        break;

                    case Method.Post:
                        request.Method = "POST";
                        break;
                }

                StreamWriter writer = new StreamWriter(request.GetRequestStream());

                writer.Write(Request);

                writer.Flush();

                writer.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                StreamReader reader = new StreamReader(response.GetResponseStream());

                Response = reader.ReadToEnd();

                reader.Close();

                response.Close();

                Error = String.Empty;

                return true;

            }
            catch (Exception ex)
            {
                Response = String.Empty;
                Error = String.Format("W3Helper:\nTryBasicAuthRequest Failed!\nUrl: {0}\nRequest: {1}\nError: {2}", Url, Request, ex.Message);
                return false;
            }

        }

        public static Boolean TryTokenizedRequest(String SessionToken, String Url, String Request, String ContentType, Method Method, int TimeoutSeconds, out String Response, out String Error)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                request.Headers.Add("Cookie", SessionToken);

                request.Timeout = TimeoutSeconds * 1000;

                request.ContentType = ContentType;

                switch (Method)
                {
                    case Method.Get:
                        request.Method = "GET";
                        break;

                    case Method.Post:
                        request.Method = "POST";
                        break;
                }

                StreamWriter writer = new StreamWriter(request.GetRequestStream());

                writer.Write(Request);

                writer.Flush();

                writer.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                StreamReader reader = new StreamReader(response.GetResponseStream());

                Response = reader.ReadToEnd();

                reader.Close();

                response.Close();

                Error = String.Empty;

                return true;

            }
            catch (Exception ex)
            {
                Response = String.Empty;
                Error = String.Format("W3Helper:\nTryBasicAuthRequest Failed!\nUrl: {0}\nRequest: {1}\nError: {2}", Url, Request, ex.Message);
                return false;
            }

        }


    }

}
