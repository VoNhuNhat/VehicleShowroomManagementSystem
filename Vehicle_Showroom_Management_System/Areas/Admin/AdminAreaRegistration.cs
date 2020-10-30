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
               "UserAccount",
               "Admin/UserAccount",
               new { controller = "UserAccount", action = "Index"}
           );
            context.MapRoute(
               "Brand",
               "Admin/Brand",
               new { controller = "Brand", action = "Index"}
           );
            
            context.MapRoute(
               "Admin",
               "Admin",
               new { controller = "Admin", action = "Index"}
           );
            
            context.MapRoute(
               "Login",
               "Admin/Login",
               new { controller = "Login", action = "Index"}
           );

            context.MapRoute(
              "Noification_Send_Email",
              "Admin/NoificationSendEmail",
              new { controller = "Login", action = "NotificationForgotPassword" }
          );
            
            context.MapRoute(
              "Reset_Password",
              "Admin/ResetPassword",
              new { controller = "Admin", action = "ResetPasswordInAdminPage" }
          );

            context.MapRoute(
              "Customer",
              "Admin/Customer",
              new { controller = "Customer", action = "Index" }
          );

            context.MapRoute(
                "Profile",
                "Admin/Profile/{userId}",
                new { controller = "Admin", action = "ProfileCurrentUser", userId = UrlParameter.Optional }
            );
            
            context.MapRoute(
                "Edit_Current_User",
                "Admin/EditCurrentUser/{userId}",
                new { controller = "Admin", action = "EditCurrentUser", userId = UrlParameter.Optional }
            );

            context.MapRoute(
                "Set_New_Password",
                "Admin/SetNewPassword/{userId}/{randomPassword}",
                new { controller = "Login", action = "ResetPassword", userId = UrlParameter.Optional, randomPassword = UrlParameter.Optional }
            );
            
            context.MapRoute(
                "Edit_UserAccount",
                "Admin/UserAccount/Edit/{userId}",
                new { controller = "UserAccount", action = "Edit", userId = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}