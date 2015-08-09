using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace MetalMynds.Utilities
{
    public class AuthenticationHelper
    {

        public static CredentialCache GetBasicAuthorisation(String UserName, String Password, String Url, CredentialCache Cache)
        {

            NetworkCredential cred = new NetworkCredential(UserName, Password, "");

            if (Cache == null)
            {
                Cache = new CredentialCache();
            }

            Cache.Add(new Uri(Url), "Basic", cred);

            return Cache;

        }

    }

}