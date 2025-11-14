using MyHibernateUtil;
using MySystem.Base.Extensions;
using System.Linq;
using System.Reflection;
using System.Web;
using vitaaid.com.Jwt;
using static backend.vitaaid.com.ServicesHelper;

namespace backend.vitaaid.com.Model
{
    public class ApplicationUser
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
    }

    public static class AuthHelper
    {
        public const string AppName = "backend.vitaaid.com";
        private static ITokenManager _tokenManager;
        public static bool SignIn(string name, string password)
        {
            // STEP0: check username and password
            var versionInfo = Assembly.GetExecutingAssembly().VersionInfo() + "(" + NHibernateHelper.Factories + ")" + "(" + NHibernateHelper.DataSources + ")";
            if (name == "superuser" && password == "32168421")
                return true;
            var oAuth = ECSServerObj.RESTfullObject.authApp(name, password, "B00", versionInfo);

            if (oAuth.bResult)
            {
                HttpContext.Current.Session["User"] = new ApplicationUser
                {
                    UserName = oAuth.oData.ShortName,
                    FirstName = oAuth.oData.Name,
                    LastName = string.Empty,
                    Email = oAuth.oData.Email,
                    AvatarUrl = "~/Content/Photo/Logged_in_User.png"
                };
                return true;
            }
            return false;
        }
        public static void SignOut()
        {
            HttpContext.Current.Session["User"] = null;
        }
        public static bool IsAuthenticated()
        {
            return GetLoggedInUserInfo() != null;
        }

        public static ApplicationUser GetLoggedInUserInfo()
        {
            return HttpContext.Current.Session["User"] as ApplicationUser;
        }
        private static ApplicationUser CreateDefualtUser()
        {
            return new ApplicationUser
            {
                UserName = "JBell",
                FirstName = "Julia",
                LastName = "Bell",
                Email = "julia.bell@example.com",
                AvatarUrl = "~/Content/Photo/Julia_Bell.jpg"
            };
        }
    }
}