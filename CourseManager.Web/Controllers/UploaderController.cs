using Abp.Web.Mvc.Authorization;
using CourseManager.Application.Utils.Upload;
using CourseManager.Common;
using CourseManager.Core.EntitiesFromCustom;
using CourseManager.File;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace CourseManager.Web.Controllers
{
    public class UploaderController : CourseManagerControllerBase
    {
        /// <summary>
        /// 上传文件
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
            try
            {
                UploadifyResult uploadifyResult = new DealWithUploadFileHelper().DealWithUploadFile(base64String, flag, dpi, videoCoverDpi, fileName); //返回类
                uploadifyResult.inputName = inputName;
                uploadifyResult.inputId = inputId;
                uploadifyResult.multi = multi;
                return Json(uploadifyResult);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return Json(new UploadifyResult() { isSuccess = false, msg = ex.Message });
            }
        }
        public ActionResult CreateImgByFolderPathAndDpi(string folderPath, string dpi)
        {
            int count = 0;
            var path = Server.MapPath(Request.ApplicationPath + folderPath);
            DirectoryInfo folder = new DirectoryInfo(path);
            string fileFullName;
            dpi = dpi.ToUpper();
            int[] dpiWAndH = dpi.Split('X').Select(o => int.Parse(o)).Where(w => w > 0).ToArray();
            foreach (FileInfo file in folder.GetFiles())
            {
                fileFullName = file.FullName;
                if (fileFullName.IndexOf(dpi, StringComparison.Ordinal) == -1)
                {
                    if (dpiWAndH != null && dpiWAndH.Length == 2)
                    {
                        if (dpi.StartsWith("-") == false)
                        {
                            dpi = string.Concat("-", dpi);
                        }
                        if (System.IO.File.Exists(fileFullName))
                        {
                            string descPath = fileFullName.Insert(fileFullName.LastIndexOf('.'), dpi);
                            if (System.IO.File.Exists(descPath) == false)
                            {
                                count++;
                                ThumbnailHelper.GenerateImage2(fileFullName, descPath, dpiWAndH[0], dpiWAndH[1]);
                            }
                        }
                    }
                }
            }
            return Content("生成缩略图总计：" + count);
        }


        public ActionResult DealWithVideoByTypeAndPath(string dir, string filename, string cateType, string relateId)
        {

            return Content("");
        }
    }
}