using System.Web;
using System.Web.Security;

namespace Mastercard.UI.Business.Helpers
{
    /// <summary>
    /// Clase que maneja las variables de Sesión.
    /// </summary>
    public class SessionHelper
    {
        public static string MarketPlaceToken
        {
            get => (HttpContext.Current.Session["MarketPlaceToken"] == null) ? null : (string)(HttpContext.Current.Session["MarketPlaceToken"]);
            set => HttpContext.Current.Session["MarketPlaceToken"] = value;
        }

        public static string IdMask
        {
            get => (HttpContext.Current.Session["IdMask"] == null) ? null : (string)(HttpContext.Current.Session["IdMask"]);
            set => HttpContext.Current.Session["IdMask"] = value;
        }

        public static string Page
        {
            get => (HttpContext.Current.Session["Page"] == null) ? null : (string)(HttpContext.Current.Session["Page"]);
            set => HttpContext.Current.Session["Page"] = value;
        }

        public static string Token
        {
            get => (HttpContext.Current.Session["Token"] == null) ? null : (string)(HttpContext.Current.Session["Token"]);
            set => HttpContext.Current.Session["Token"] = value;
        }

        public static string UserName
        {
            get => (HttpContext.Current.Session["UserName"] == null) ? null : (string)(HttpContext.Current.Session["UserName"]);
            set => HttpContext.Current.Session["UserName"] = value;
        }

        public static string Name
        {
            get => (HttpContext.Current.Session["Name"] == null) ? null : (string)(HttpContext.Current.Session["Name"]);
            set => HttpContext.Current.Session["Name"] = value;
        }

        public static string Identification
        {
            get => (HttpContext.Current.Session["Identification"] == null) ? null : (string)(HttpContext.Current.Session["Identification"]);
            set => HttpContext.Current.Session["Identification"] = value;
        }

        public static int SegmentId
        {
            get => (HttpContext.Current.Session["SegmentId"] == null) ? 0 : (int)(HttpContext.Current.Session["SegmentId"]);
            set => HttpContext.Current.Session["SegmentId"] = value;
        }

        public static string Guid
        {
            get => (HttpContext.Current.Session["Guid"] == null) ? null : (string)(HttpContext.Current.Session["Guid"]);
            set => HttpContext.Current.Session["Guid"] = value;
        }

        public static string Group
        {
            get => (HttpContext.Current.Session["Group"] == null) ? null : (string)(HttpContext.Current.Session["Group"]);
            set => HttpContext.Current.Session["Group"] = value;
        }

        public static int CardLoginId
        {
            get => (HttpContext.Current.Session["CardLoginId"] == null) ? 0 : (int)(HttpContext.Current.Session["CardLoginId"]);
            set => HttpContext.Current.Session["CardLoginId"] = value;
        }

        //public static bool IsBlackUser
        //{
        //    get => (HttpContext.Current.Session["IsBlackUser"] == null) || (bool)(HttpContext.Current.Session["IsBlackUser"]);
        //    set => HttpContext.Current.Session["IsBlackUser"] = value;
        //}
        public static bool CanRedeem
        {
            get => (HttpContext.Current.Session["CanRedeem"] == null) || (bool)(HttpContext.Current.Session["CanRedeem"]);
            set => HttpContext.Current.Session["CanRedeem"] = value;
        }

        /// <summary>
        /// Si cumplió límite de redenciones
        /// </summary>
        public static bool RedemptionLimitReached
        {
            get => (HttpContext.Current.Session["RedemptionLimitReached"] == null) || (bool)(HttpContext.Current.Session["RedemptionLimitReached"]);
            set => HttpContext.Current.Session["RedemptionLimitReached"] = value;
        }

        public static bool FirstLogin
        {
            get => (HttpContext.Current.Session["FirstLogin"] == null) || (bool)(HttpContext.Current.Session["FirstLogin"]);
            set => HttpContext.Current.Session["FirstLogin"] = value;
        }

        public static int BillingGroupGoal
        {
            get => (HttpContext.Current.Session["BillingGroupGoal"] == null) ? 0 : (int)(HttpContext.Current.Session["BillingGroupGoal"]);
            set => HttpContext.Current.Session["BillingGroupGoal"] = value;
        }

        public static bool LoginComplete
        {
            get
            {
                return HttpContext.Current.Session["LoginComplete"] != null ? (bool)HttpContext.Current.Session["LoginComplete"] : false;
            }
            set { HttpContext.Current.Session["LoginComplete"] = value; }
        }

        public static bool ChellengeCheck
        {
            get
            {
                return HttpContext.Current.Session["ChellengeCheck"] != null ? (bool)HttpContext.Current.Session["ChellengeCheck"] : false;
            }
            set { HttpContext.Current.Session["ChellengeCheck"] = value; }
        }

        public static bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(Token);
        }

        public static void LogOut()
        {
            IdMask = null;
            Token = null;
            Name = null;
            Guid = null;
            SegmentId = 0;
            Group = null;
            CardLoginId = 0;
            LoginComplete = false;
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.RemoveAll();
            FormsAuthentication.SignOut();
        }

        public static void LogOutGuid()
        {
            Guid = null;
        }
    }
}