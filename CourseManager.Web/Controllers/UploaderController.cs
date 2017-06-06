using Abp.Web.Mvc.Authorization;
using CourseManager.Application.Utils.Upload;
using CourseManager.Common;
using CourseManager.Core.EntitiesFromCustom;
using CourseManager.File;
using System.Linq;
using System.Web.Mvc;

namespace CourseManager.Web.Controllers
{
    public class UploaderController : CourseManagerControllerBase
    {
        private IFileAppService _fileAppService = null;
        public UploaderController(IFileAppService fileAppService)
        {
            this._fileAppService = fileAppService;
        }
        /// <summary>
        /// 上传图片，软实力项目的详情页面的图片
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="exts"></param>
        /// <param name="inputName"></param>
        /// <param name="inputId">上传文件file控件Id</param>
        /// <param name="dpi">需要临时返回的图片尺寸</param>
        /// <param name="videoCoverDpi">视频dpi，用来截取视频中第一帧图片，然后裁切成指定dpi大小（如果传视频的话）</param>
        /// <param name="base64String">采用base64方式传图片</param>
        /// <param name="fileName">如果是采用base64方式上传图片，就要传原始文件名</param>
        /// <param name="multi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadFile(string flag, string exts, string inputName, string inputId, string dpi, string videoCoverDpi, string base64String, string fileName, bool multi = false)
        {
            UploadifyResult uploadifyResult = new DealWithUploadFileHelper().DealWithUploadFile(base64String, flag, dpi, videoCoverDpi, fileName); //返回类
            uploadifyResult.inputName = inputName;
            uploadifyResult.inputId = inputId;
            uploadifyResult.multi = multi;
            return AbpJson(data: uploadifyResult, wrapResult: false, camelCase: false);
        }

        [AbpMvcAuthorize]
        /// <summary>
        /// 手动根据尺寸生成图片
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateImgByTypeAndDpi(string cateType, string dpi)
        {
            int count = 0;
            if (!string.IsNullOrWhiteSpace(cateType) && !string.IsNullOrWhiteSpace(dpi))
            {
                dpi = dpi.ToUpper();
                int[] dpiWAndH = dpi.Split('X').Select(o => int.Parse(o)).Where(w => w > 0).ToArray();
                if (dpiWAndH != null && dpiWAndH.Length == 2)
                {
                    if (dpi.StartsWith("-") == false)
                    {
                        dpi = string.Concat("-", dpi);
                    }
                    var files = _fileAppService.GetFilesList(f => f.CategoryType == cateType).ToList();
                    if (files.Any())
                    {
                        foreach (var file in files)
                        {
                            string sourcePath = WebHelper.GetMapPath(file.Url);
                            if (System.IO.File.Exists(sourcePath))
                            {
                                string descPath = sourcePath.Insert(sourcePath.LastIndexOf('.'), dpi);
                                if (System.IO.File.Exists(descPath) == false)
                                {
                                    count++;
                                    ThumbnailHelper.GenerateImage2(sourcePath, descPath, dpiWAndH[0], dpiWAndH[1]);
                                }
                            }
                        }
                    }
                }
            }
            return Content(string.Format("{0}--变更了尺寸{1}的图片{2}张", cateType, dpi, count));
        }
    }
}