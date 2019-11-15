using System.Configuration;

namespace SSO.Util
{
    public class AppSettings
    {
        public static string secretKey = ConfigurationManager.AppSettings["secretKey"];
        public static string issuer = ConfigurationManager.AppSettings["issuer"];
        public static string cookieTime = ConfigurationManager.AppSettings["cookieTime"];
        public static string cookieName = ConfigurationManager.AppSettings["cookieName"];
        public static string databaseKey = ConfigurationManager.AppSettings["databaseKey"];
        public static string admin = ConfigurationManager.AppSettings["admin"];
    }
}
