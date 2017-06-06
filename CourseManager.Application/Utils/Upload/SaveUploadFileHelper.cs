using Abp.AutoMapper;
using Abp.Dependency;
using CourseManager.Common;
using CourseManager.Common.Config;
using CourseManager.Common.Config.ImagesDPI;
using CourseManager.Common.Enums;
using CourseManager.Common.Extensions;
using CourseManager.Core.EntitiesFromCustom;
using CourseManager.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CourseManager.Application.Utils.Upload
{
    /// <summary>
    /// 上传文件帮助类
    /// </summary>
    public class SaveUploadFileHelper : ITransientDependency
    {

        Lazy<ImagesDpiConfigHelper> ImagesDpiConfigManager = new Lazy<ImagesDpiConfigHelper>(() => { return new ImagesDpiConfigHelper(); });

        private IFileAppService _fileAppService;

        public SaveUploadFileHelper(IFileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        /// <summary>
        /// 保存多文件上传
        /// </summary>
        /// <param name="currentUserId">当前用户对象</param>
        /// <param name="modelId">关联的其他表Id</param>
        /// <param name="fileType">文件类型枚举</param>
        /// <param name="fileUpload">上传文件路径</param>
        /// <param name="fileIds">文件Id（如果是修改就可能有Id），可空</param>
        /// <param name="videoInfoList">视频的详情</param>
        /// <returns></returns>
        public List<Files> SaveMultUploadFile(long currentUserId, object modelId, FileTypesEnum fileType, List<string> fileUpload, List<int> fileIds, List<string> videoInfoList = null)
        {
            return SaveMultUploadFile(currentUserId, modelId.ToString(), fileType, fileUpload, fileIds, videoInfoList);
        }
        /// <summary>
        /// 保存多文件上传
        /// </summary>
        /// <param name="currentUserId">当前用户对象</param>
        /// <param name="modelId">关联的其他表Id</param>
        /// <param name="fileType">文件类型枚举</param>
        /// <param name="fileUpload">上传文件路径</param>
        /// <param name="fileIds">文件Id（如果是修改就可能有Id），可空</param>
        /// <param name="videoInfoList">视频的详情</param>
        /// <returns></returns>
        public List<Files> SaveMultUploadFile(long currentUserId, string modelId, FileTypesEnum fileType, List<string> fileUpload, List<int> fileIds, List<string> videoInfoList = null)
        {
            var oldFiles = _fileAppService.GetFilesListByCategoryIdAndType(modelId.ToString(), fileType.ToString());
            bool canCommit = false;
            List<Files> deleteFileList = new List<Files>();//删除的图片
            for (int i = oldFiles.Count - 1; i >= 0; i--)
            {
                if (fileIds == null || fileIds.Contains(oldFiles[i].Id) == false)
                {
                    _fileAppService.Delete(oldFiles[i]);
                    deleteFileList.Add(oldFiles[i]);//记录删除的图片
                    oldFiles.RemoveAt(i);
                    canCommit = true;
                }
            }
            if (fileUpload.IsNotNull() && fileUpload.Any())
            {
                var files = oldFiles.Select(o => o.Url).ToArray();
                for (int i = 0; i < fileUpload.Count; i++)
                {
                    var o = fileUpload[i];
                    if (files.Contains(o.Trim()) == false)
                    {
                        var f = BindUploadFiles(o, modelId.ToString(), fileType, currentUserId, false);
                        if (f.IsNotNull())
                        {
                            fileUpload[i] = f.Url;
                            if (videoInfoList.IsNotEmpty())
                            {
                                f.Description = videoInfoList[i];
                            }
                            oldFiles.Add(f);
                        }
                    }
                }
                canCommit = true;
            }
            if (canCommit)
            {
                if (oldFiles.Any())
                {
                    int i = 1;
                    foreach (var o in oldFiles)
                    {
                        o.SortNo = i++;
                    }
                    _fileAppService.Update(oldFiles);
                }
                foreach (var item in deleteFileList)//移动删除的文件
                {
                    MoveToRecycleAsync(item.Url);
                }
            }
            return oldFiles;
        }


        #region 保存文件到数据库逻辑
        /// <summary>
        /// 关联文件到对应的数据，并移动到指定的路径，并保存数据到数据库返回File对象
        /// </summary>
        /// <param name="interimPath"></param>
        /// <param name="categoryId"></param>
        /// <param name="categoryType"></param>
        /// <param name="currentUserId"></param>
        /// <param name="isReplace">是否替换掉之前所有相同类型（categoryType）的文件</param>
        /// <param name="customSaveDirPath"></param>
        /// <returns></returns>
        public Files BindUploadFiles(string interimPath, string categoryId, FileTypesEnum categoryType, long currentUserId, bool isReplace = true, string customSaveDirPath = "")
        {
            string interimFullPath = WebHelper.GetMapPath(interimPath);
            System.IO.FileInfo fi = new System.IO.FileInfo(interimFullPath);
            if (fi.Exists)
            {
                var fileHandler = DealWithUploadFileHelper.GetInstance();
                try
                {
                    string fileSourceName = string.Empty;//文件名
                    string fileName = string.Empty;//文件路径
                    //当包含]时为新传文件
                    //var f = fi.Name.IndexOf("]") > -1;
                    if (fi.Name.IndexOf(']') < 1)
                    {
                        return null;
                    }
                    int index = fi.Name.LastIndexOf(']');
                    fileName = fi.Name.Substring(index + 1);
                    fileSourceName = fi.Name.Substring(0, index).TrimStart('[');
                    string oldPath = string.Empty;
                    bool isCreate = false;
                    Files model = null;
                    if (isReplace && categoryId.IsNotNullAndNotEmpty())
                    {
                        model = _fileAppService.GetFilesByCategoryIdAndType(categoryId, categoryType.ToString());
                        if (model != null)
                        {
                            if (model.Url == interimPath) return null;
                            model.SortNo = 0;
                            oldPath = model.OldUrl;
                            model.OldUrl = model.Url;
                        }
                    }
                    if (model == null)
                    {
                        isCreate = true;
                        model = new Files();
                        //model.Id = IdentityCreator.NewGuid;
                        model.CreationTime = DateTime.Now;
                        model.RelateId = categoryId;
                        model.CategoryType = categoryType.ToString();
                        model.OldUrl = "";
                        model.SortNo = 0;
                        model.CreatorUserId = currentUserId;
                        model.TenantId = 1;

                    }
                    model.FileName = fileSourceName.Substring(0, fileSourceName.Length > 100 ? 100 : fileSourceName.Length);
                    model.FileSize = (int)fi.Length;

                    string cPath = string.Empty;
                    if (customSaveDirPath.IsNullOrEmpty())
                    {
                        string baseDirPath = DealWithUploadFileHelper.GetInstance().GetFilesSaveDirPath(categoryType.ToString());//保存文件夹路径，路径是从根目录开始
                        cPath = baseDirPath;
                    }
                    else
                    {
                        cPath = customSaveDirPath;
                    }
                    model.Url = System.IO.Path.Combine(cPath, fileName);
                    cPath = WebHelper.GetMapPath(cPath);//保存文件夹完整路径
                    if (!System.IO.Directory.Exists(cPath))
                    {
                        System.IO.Directory.CreateDirectory(cPath);
                    }
                    if (isCreate)
                    {
                        model = _fileAppService.Insert(model);
                    }
                    else
                    {
                        model = _fileAppService.Update(model);
                    }
                    cPath = WebHelper.GetMapPath(model.Url);
                    if (!System.IO.File.Exists(cPath))
                    {
                        fi.MoveTo(cPath);
                    }
                    #region 视频的封面处理

                    string videoCoverImg = string.Empty;
                    if (System.IO.Path.GetExtension(fi.FullName).ToUpper().Contains("MP4"))
                    {
                        string dir = System.IO.Path.GetDirectoryName(interimFullPath);
                        string videoFileName = System.IO.Path.GetFileNameWithoutExtension(cPath) + ".jpg";
                        string coverSourceImgPath = string.Concat(dir, "\\", videoFileName);
                        if (System.IO.File.Exists(coverSourceImgPath))
                        {
                            string saveCoverPath = string.Concat(System.IO.Path.GetDirectoryName(cPath), "\\", videoFileName);
                            new System.IO.FileInfo(coverSourceImgPath).MoveTo(saveCoverPath);
                            MakeThumbnail(saveCoverPath, categoryType);//封面裁切成指定尺寸
                        }

                    }

                    #endregion
                    #region 处理上传图片，需要缩略图等多种尺寸

                    #region 生成缩略图

                    MakeThumbnail(cPath, categoryType);

                    #endregion
                    #endregion
                    //更新删除旧文件
                    if (oldPath.IsNotNullAndNotEmpty())
                    {
                        MoveToRecycleAsync(oldPath);
                    }
                    return model;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {

                }
            }
            return null;
        }

        #endregion


        /// <summary>
        /// 根据配置文件生成缩略图
        /// </summary>
        /// <param name="cPath">绝对路径</param>
        /// <param name="categoryType"></param>
        public void MakeThumbnail(string cPath, FileTypesEnum categoryType)
        {
            #region 生成缩略图
            string[] imgFormat = new[] { "JPEG", "JPG", "PNG", "BMP" };//如果是图片才会进行裁切操作
                                                                       //缩略图保存格式，是根据配置文件ImagesDPI.xml里面的dpi来命名
            if (imgFormat.Any(o => System.IO.Path.GetExtension(cPath).ToUpper().Contains(o)))
            {
                string strCategoryType = categoryType.ToString();
                var imageDpi = ImagesDpiConfigManager.Value.GetImagesDpiConfigByCategoryType(strCategoryType);
                if (imageDpi != null)
                {
                    ImagesDpiConfigManager.Value.MakeThumbnail(cPath, imageDpi);//生成的缩略图的路径格式是：原图路径 + “-”+ dpi + 后缀
                }
            }
            #endregion
        }

        #region 删除文件

        /// <summary>
        /// 单纯把文件移动到回收站
        /// </summary>
        /// <param name="vpath"></param>
        public void MoveToRecycleAsync(string vpath)
        {
            Task.Run(() =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(vpath))
                    {
                        return;
                    }
                    string sourceFullPath = WebHelper.GetMapPath(vpath);
                    System.IO.FileInfo fi = new System.IO.FileInfo(sourceFullPath);
                    if (fi.Exists)
                    {
                        if (!vpath.StartsWith("/"))
                        {
                            vpath = "/" + vpath;
                        }
                        string fullPath = WebHelper.GetMapPath(new AppConfig.WebSettings().UploadRecycleDefaultPath + DateTime.Now.ToString("yyyyMM") + vpath);
                        string fullDirPath = System.IO.Path.GetDirectoryName(fullPath);
                        if (!System.IO.Directory.Exists(fullDirPath))
                        {
                            System.IO.Directory.CreateDirectory(fullDirPath);
                        }
                        fi.MoveTo(fullPath);
                        //移动其他尺寸图片
                        string sourceDir = System.IO.Path.GetDirectoryName(sourceFullPath);//图片所在文件夹路径
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(sourceFullPath);//获取图片文件名
                        string pattern = string.Concat(fileName, "-*.*");//匹配文件
                        System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(sourceDir);
                        var files = dirInfo.GetFiles(pattern);//当前文件夹所有文件
                        if (null != files)
                        {
                            string moveToDir = System.IO.Path.GetDirectoryName(fullPath);//移动到的文件夹
                            foreach (var item in files)//匹配到就回收
                            {
                                item.MoveTo(System.IO.Path.Combine(moveToDir, item.Name));
                            }
                        }
                        if (System.IO.Path.GetExtension(sourceFullPath).ToUpper().Contains("MP4"))//如果是mp4文件得把封面删除掉
                        {
                            string imgName = string.Concat(System.IO.Path.GetFileNameWithoutExtension(sourceFullPath), ".jpg");
                            string imgPath = string.Concat(System.IO.Path.GetDirectoryName(sourceFullPath), "\\", imgName);
                            if (System.IO.File.Exists(imgPath))
                            {
                                string moveToDir = System.IO.Path.GetDirectoryName(fullPath);//移动到的文件夹
                                new System.IO.FileInfo(imgPath).MoveTo(System.IO.Path.Combine(moveToDir, imgName));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //LogManager.Error("移动文件到回收文件夹出错，文件路径：" + vpath + "。出错信息：" + ex.Message);
                }
            });
        }
        //private void MoveToRecycle(string vpath)
        //{
        //    FileInfo fi = new FileInfo(WebHelper.GetMapPath(vpath));
        //    if (fi.Exists)
        //    {
        //        if (!vpath.StartsWith("/"))
        //        {
        //            vpath = "/" + vpath;
        //        }
        //        string fullPath = WebHelper.GetMapPath(AppConfig.WebSettings.UploadRecycleDefaultPath + DateTime.Now.ToString("yyyy") + vpath);
        //        string fullDirPath = Path.GetDirectoryName(fullPath);
        //        if (!Directory.Exists(fullDirPath))
        //        {
        //            Directory.CreateDirectory(fullDirPath);
        //        }
        //        fi.MoveTo(fullPath);
        //    }
        //}

        #endregion

    }
}
