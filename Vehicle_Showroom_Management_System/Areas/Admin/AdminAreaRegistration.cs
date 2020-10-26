using System.Web.Mvc;

namespace Vehicle_Showroom_Management_System.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Reset_Password",
                "Admin/{controller}/{action}/{userId}/{randomPassword}",
                new { controller = "Login", action = "ResetPassword", userId = UrlParameter.Optional, randomPassword = UrlParameter.Optional }
            );
        }
    }
}