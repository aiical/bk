using Abp.Web.Mvc.Views;

namespace CourseManager.Web.Views
{
    public abstract class CourseManagerWebViewPageBase : CourseManagerWebViewPageBase<dynamic>
    {

    }

    public abstract class CourseManagerWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected CourseManagerWebViewPageBase()
        {
            LocalizationSourceName = CourseManagerConsts.LocalizationSourceName;
        }
    }
}