using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NYCJobsWeb.Common
{
    public class Settings
    {
        public const string Aspxauth = ".ASPXAUTH";
        public static string EncSecretkey
        {
            get { return ConfigurationManager.AppSettings["EncSecretkey"]; }

        }
        public static string FTPName
        {
            get { return ConfigurationManager.AppSettings["FTP"]; }

        }
    }
}