using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Runtime.Serialization;

namespace MetalMynds.Utilities
{
    public class WGETHelper
    {

        public static Boolean TryGet(String Url, out MemoryStream Stream,out String Error)
        {
            return TryGet(Url, String.Empty, String.Empty, out Stream,out Error);
        }

        public static Boolean TryGet(String Url,String UserName,String Password, out MemoryStream Stream,out String Error)
        {

            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                if (UserName != String.Empty && Password != String.Empty)
                {

                    request.Credentials = AuthenticationHelper.GetBasicAuthorisation(UserName, Password,Url,null);

                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();

                MemoryStream file = new MemoryStream();
                Int32 bufferMax = 1024;
                Int32 readCount = 0;

                byte[] inputBuffer = new byte[bufferMax];

                while ((readCount = responseStream.Read(inputBuffer, 0, bufferMax)) != 0)
                {
                    file.Write(inputBuffer, 0, readCount);
                }

                file.Seek(0, 0);

                Stream = file;
                Error = String.Empty;
                return true;

            }
            catch (Exception ex)
            {
                Stream = null;
                Error = String.Format("WGETHelper:\nGet [{0}] Failed!\nError: {1}",Url, ex.Message);
                return false;
            }

        }

    }
}
