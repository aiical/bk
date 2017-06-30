using Abp.Web.Mvc.Authorization;
using CourseManager.Utils.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CourseManager.Web.Controllers
{
    public class ImportController : CourseManagerControllerBase
    {
        private ImportStudentCourseArrangeUtil _importUtil;
        public ImportController(ImportStudentCourseArrangeUtil importUtil)
        {
            this._importUtil = importUtil;
        }
        [AbpMvcAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ImportStudentCourseArrangeToDB(List<string> pathList)
        {
            int successCount = 0;
            int failCount = 0;
            int importCount = 0;
            StringBuilder sb = new StringBuilder();
            pathList.ForEach(o => {
                if (string.IsNullOrWhiteSpace(o))
                {
                    return;
                }
                var r = _importUtil.SaveStudentCourseArrangeData(o);
                if (r.Item1)
                {
                    importCount = r.Item2.Count;
                    successCount++;
                }
                else
                {
                    failCount++;
                    if (failCount == 1)
                    {
                        sb.Append("<br>出错的文件：");
                    }
                    sb.Append("　" + o.Substring(o.IndexOf('[') + 1, o.LastIndexOf(']')) + "<br>");
                }
            });
            return AbpJson(new { msg = string.Format("成功{0}个文件,导入数据{2}条；失败{1}个文件{3}", successCount, failCount, importCount, sb.ToString()) });
        }
    }
}