using Abp.Application.Navigation;
using Abp.Localization;
using CourseManager.Authorization;

namespace CourseManager.Web
{
    /// <summary>
    /// This class defines menus for the application.
    /// It uses ABP's menu system.
    /// When you add menu items here, they are automatically appear in angular application.
    /// See .cshtml and .js files under App/Main/views/layout/header to know how to render menu.
    /// </summary>
    public class CourseManagerNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(
                    new MenuItemDefinition(
                        "Home",
                        L("CourseManager"),
                        //new LocalizableString("HomePage", CourseManagerConsts.LocalizationSourceName),
                        url: "#/",
                        icon: "fa fa-home",
                        requiresAuthentication: true
                        )
                )
                     //.AddItem(
                     //    new MenuItemDefinition(
                     //        "Tenants",
                     //        L("Tenants"),
                     //        url: "#tenants",
                     //        icon: "fa fa-globe",
                     //        requiredPermissionName: PermissionNames.Pages_Tenants
                     //        )
                     //)
                     .AddItem(
                    new MenuItemDefinition(
                        "SignIn",
                        L("SignIn"),
                        url: "#signIn",
                        icon: "fa fa-globe"
                        )
                )
                .AddItem(
                    new MenuItemDefinition(
                        "AbsentCheckIn",
                        L("AbsentCheckIn"),
                        url: "#absentCheckIn",
                        icon: "fa fa-globe"
                        )
                )
                  .AddItem(
                    new MenuItemDefinition(
                        "ClassHourStatistics",
                        L("ClassHourStatistics"),
                        url: "#classHourStatistics",
                        icon: "fa fa-info"
                        )
                )
                  .AddItem(
                    new MenuItemDefinition(
                        "OfficeHourStatistics",
                        L("OfficeHourStatistics"),
                        url: "#officeHourStatistics",
                        icon: "fa fa-info"
                        )
                ).AddItem(
                    new MenuItemDefinition(
                        "PayCalculation",
                        L("PayCalculation"),
                        url: "#payCalculation",
                        icon: "fa fa-info"
                        )
                )
                .AddItem(
                    new MenuItemDefinition(
                        "Students",
                        L("Students"),
                        url: "#students",
                        icon: "fa fa-users"
                        )
                )
                .AddItem(
                    new MenuItemDefinition(
                        "Users",
                        L("Users"),
                        url: "#users",
                        icon: "fa fa-users",
                        requiredPermissionName: PermissionNames.Pages_Users
                        )
                );
                //.AddItem(
                //    new MenuItemDefinition(
                //        "About",
                //        new LocalizableString("About", CourseManagerConsts.LocalizationSourceName),
                //        url: "#/about",
                //        icon: "fa fa-info"
                //        )
                //);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, CourseManagerConsts.LocalizationSourceName);
        }
    }
}
