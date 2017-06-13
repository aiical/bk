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
            //.AddItem(
            //    new MenuItemDefinition(
            //        "Tenants",
            //        L("Tenants"),
            //        url: "#tenants",
            //        icon: "fa fa-globe",
            //        requiredPermissionName: PermissionNames.Pages_Tenants
            //        )
            //)
            context.Manager.MainMenu
                .AddItem(
                    new MenuItemDefinition(
                        "Home",//一个常量，控制菜单是否被选中
                        L("CourseManager"),////菜单显示名称，在语言文件中配置
                                           //new LocalizableString("HomePage", CourseManagerConsts.LocalizationSourceName),
                        url: "#/",
                        icon: "fa fa-home",
                        requiresAuthentication: true
                        )
                )
                    .AddItem(new MenuItemDefinition(
                            "ArrangeCourse",
                            L("ArrangeCourse"),
                            icon: "fa fa-calendar",
                            requiredPermissionName: PermissionNames.Pages_Users
                        ).AddItem(
                            new MenuItemDefinition(
                            "TeacherArrange",
                            L("TeacherArrange"),
                            url: "CourseArrange/TeacherCourseArrange",
                            icon: "fa fa-calendar",
                              requiredPermissionName: PermissionNames.Pages_Users
                        )).AddItem(
                         new MenuItemDefinition(
                            "StudentArrange",
                            L("StudentArrange"),
                            url: "CourseArrange/StudentCourseArrange",
                            icon: "fa fa-calendar",
                           requiredPermissionName: PermissionNames.Pages_Users
                        )
                )).AddItem(
                            new MenuItemDefinition(
                            "SignIn",
                            L("SignIn"),
                            url: "#signIn",
                            icon: "fa fa-check-circle",
                              requiredPermissionName: PermissionNames.Pages_Users
                        ))
                //.AddItem(new MenuItemDefinition(
                //            "Sign",
                //            L("Sign"),
                //            icon: "fa  fa-check-circle"
                //        ).AddItem(
                //            new MenuItemDefinition(
                //            "SignIn",
                //            L("SignIn"),
                //            url: "#signIn",
                //            icon: "fa fa-check-circle",
                //              requiredPermissionName: PermissionNames.Pages_Users
                //        )).AddItem(
                //         new MenuItemDefinition(
                //            "AbsentCheckIn",
                //            L("AbsentCheckIn"),
                //            url: "#absentCheckIn",
                //            icon: "fa fa-check-circle",
                //              requiredPermissionName: PermissionNames.Pages_Users
                //        )
                //))
                .AddItem(
                    new MenuItemDefinition(
                        "ClassHourStatistics",
                        L("ClassHourStatistics"),
                        icon: "fa fa-bar-chart-o"
                        ).AddItem(
                            new MenuItemDefinition(
                                "TeacherClassHourStatistics",
                                L("TeacherClassHourStatistics"),
                                 url: "ClassHourStatistics/TeacherClassHours",
                                //url: "#teacherClassHourStatistics",
                                icon: "fa fa-bar-chart-o",
                                     requiredPermissionName: PermissionNames.Pages_Users
                            )
                        ).AddItem(
                        new MenuItemDefinition(
                            "StudentClassHourStatistics",
                            L("StudentClassHourStatistics"),
                            url: "#studentClassHourStatistics",
                            icon: "fa fa-bar-chart-o",
                              requiredPermissionName: PermissionNames.Pages_Users
                        )
                    )
                  )
                  .AddItem(
                    new MenuItemDefinition(
                        "OfficeHourStatistics",
                        L("OfficeHourStatistics"),
                        url: "#officeHourStatistics",
                        icon: "fa fa-bar-chart-o",
                          requiredPermissionName: PermissionNames.Pages_Users
                        )
                ).AddItem(
                    new MenuItemDefinition(
                        "PayCalculation",
                        L("PayCalculation"),
                        url: "#payCalculation",
                        icon: "fa fa-cny",
                          requiredPermissionName: PermissionNames.Pages_Users
                        )
                )
                .AddItem(
                    new MenuItemDefinition(
                        "Students",
                        L("Students"),
                        url: "#students",
                        icon: "fa fa-users",
                          requiredPermissionName: PermissionNames.Pages_Users
                        )
                ).AddItem(
                    new MenuItemDefinition(
                        "Teachers",
                        L("Teachers"),
                        url: "#teachers",
                        icon: "fa fa-users",
                          requiredPermissionName: PermissionNames.Pages_Users
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
