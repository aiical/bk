using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManager.Common.Extensions;
using System.Web;
using CourseManager.Common.ThirdParty.FFmpeg;
using Newtonsoft.Json;
using System.IO;
using CourseManager.Common.Config.ImagesDPI;
using CourseManager.Common.Config;
using CourseManager.Common.Enums;
using CourseManager.Core.EntitiesFromCustom;
using CourseManager.Common;
using System.Drawing;

namespace CourseManager.Application.Utils.Upload
{
	/// <summary>
	/// 处理上传的文件
	/// </summary>
	public class DealWithUploadFileHelper
	{
		public static DealWithUploadFileHelper GetInstance()
		{
			return new DealWithUploadFileHelper();
		}

		#region 属性

		AppConfig.WebSettings WebSettingsManager = new AppConfig.WebSettings();

		//Lazy<FilesService> FilesServiceManager = new Lazy<FilesService>(() => { return new FilesService(); });

		#endregion
		#region 其他属性
		/// <summary>
		/// 如果后台需要做定制操作的话，前端可以传这个值 flag=XX，可空
		/// </summary>
		private string _flag = null;
		private string _dpi = string.Empty;
        private string _videoCoverDpi = string.Empty;

        #region 以base64格式上传文件相关属性
        /// <summary>
        /// 以base64格式上传文件
        /// </summary>
        private string _base64String = string.Empty;
        /// <summary>
        /// 采用base64格式上传文件时候，需要原始文件名的话，要从前端传到后台，不然会自动生成一个文件名称
        /// </summary>
        private string _fileName = string.Empty;
        #endregion

        /// <summary>
        /// 多个文件路径的分隔符，目前是“:”
        /// </summary>
        private string _splitChar = ":";
        #endregion


        #region 处理上传文件逻辑
        /// <summary>
        /// 处理上传的文件入口，保存在临时路径下
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="dpi">生成的临时缩略图尺寸，方便展示，为空不生成，格式：480X160</param>
        /// <param name="videoCoverDpi">如果是视频，需要返回视频里面的某种图片，要设置dpi</param>
        /// <returns></returns>
        public UploadifyResult DealWithUploadFile(string base64String, string flag, string dpi,string videoCoverDpi,string fileName)
		{
			_flag = flag;
			_dpi = dpi;
            _videoCoverDpi = videoCoverDpi;
            _base64String = base64String;
            _fileName = fileName;

            UploadifyResult uploadifyResult = new UploadifyResult();//返回类
			#region 第一步：文件上传到服务器之前执行的操作，比如验证之类的，包括自定义操作(结合flag参数)
			var beforeActionResult = UploadBeforeAction();
			if (beforeActionResult.isSuccess == false)
			{
				uploadifyResult.isSuccess = false;
				uploadifyResult.msg = beforeActionResult.errorMsg;
				goto RequestEnd;
			}
			#endregion

			//第二步：处理上传文件
			uploadifyResult = UploadFilesAction();
			//第三步：文件上传到服务器成功之后执行的操作
			if (uploadifyResult.isSuccess)
			{
				UploadAfterAction(uploadifyResult);
			}

		RequestEnd:
			return uploadifyResult;
		}
		




        /// <summary>
        /// 对文件进行处理
        /// </summary>
        /// <returns></returns>
        private UploadifyResult UploadFilesAction()
		{
			var uploadifyResult = SaveUploadFiles();
			return uploadifyResult;
		}

		#region 基本操作：验证和获取文件信息
		/// <summary>
		/// 公共验证
		/// </summary>
		/// <returns></returns>
		private ResultData Validate()
		{
			var result = new ResultData();
			if (HttpContext.Current.Request.RequestType != "POST")
			{
				result.isSuccess = false;
				result.errorMsg = "提交方式有误！";
				goto ValidateEnd;
			}
			if (HttpContext.Current.Request.Files.Count > 0)
			{
				if (HttpContext.Current.Request.Files[0].FileName.Length > 180)
				{
					result.isSuccess = false;
					result.errorMsg = "文件名称过长，建议不要超过180个字符！";
					goto ValidateEnd;
				}
			}
			else if(_base64String.IsNullOrEmpty())
			{
				result.isSuccess = false;
				result.errorMsg = "请选择文件再上传！";
			}
		ValidateEnd:
			return result;
		}


		#endregion

		/// <summary>
		/// 文件上传到服务器之前执行的操作
		/// </summary>
		/// <returns></returns>
		private ResultData UploadBeforeAction()
		{
			var result = new ResultData();
			result = Validate();//验证上传文件结果
			if (result.isSuccess == false)
			{
				goto UploadBeforeActionEnd;
			}


		UploadBeforeActionEnd:
			return result;
		}
		/// <summary>
		/// 文件上传到服务器之后执行的操作
		/// </summary>
		/// <returns></returns>
		private void UploadAfterAction(UploadifyResult uploadifyResult)
		{
			//图片dpi
			if (_dpi.IsNotNullAndNotEmpty())
			{
				List<string> paths = uploadifyResult.uploadPath.Split(_splitChar).ToList();
                foreach (var path in paths)
                {
                    string ext = Path.GetExtension(path).ToUpper();
                    if (ext.EndsWith("JPG") || ext.EndsWith("JPEG") || ext.EndsWith("PNG"))
                    {
                        int[] whInt = GetDpi(_dpi);
                        //生成的缩略图的路径格式是：原图路径 + “-”+ dpi + 后缀
                        string desPath = path.Insert(path.LastIndexOf('.'), "-" + _dpi);
                        ThumbnailHelper.GenerateImage2(WebHelper.GetMapPath("~" + path), WebHelper.GetMapPath("~" + desPath), whInt[0], whInt[1]);
                    }
                }
			}
            //视频dpi
            if (_videoCoverDpi.IsNotNullAndNotWhiteSpace())
            {
                string path = uploadifyResult.uploadPath.Split(_splitChar).First();//正常情况下，视频一次只能传一个
                string ext = Path.GetExtension(path).ToUpper();
                if (ext.EndsWith("MP4")|| ext.EndsWith("MOV"))
                {
                    GenerateVideoCoverImg(path, uploadifyResult);
                }
            }
		}
        /// <summary>
        /// 生成视频封面
        /// </summary>
        /// <param name="path">视频的绝对路径：/Upload/File/zzzzz.mp4</param>
        /// <param name="uploadifyResult"></param>
        public void GenerateVideoCoverImg(string path,UploadifyResult uploadifyResult)
        {
            #region 生成图片
            string videoFullPath = WebHelper.GetMapPath(path);
            FFmpegHelper ff = new FFmpegHelper(videoFullPath);
            VideoInfo videoInfo = ff.GetVideoInfo();
            string saveImgPath = string.Empty;
            if (videoInfo.Width < 1)
            {
                videoInfo.Width = 500;
                videoInfo.Height = 330;
            }
            if (videoInfo.Width > 0)
            {
                uploadifyResult.videoInfo = JsonConvert.SerializeObject(videoInfo);
                int[] whInt = GetDpi(_videoCoverDpi);
                string dir = Path.GetDirectoryName(videoFullPath);
                string videoFileName = Path.GetFileNameWithoutExtension(videoFullPath);
                int index = videoFileName.LastIndexOf(']');
                videoFileName = videoFileName.Substring(index + 1);//去掉括号
                int duration = 3;
                if (uploadifyResult.fileSize < 10485760)
                {
                    duration = 2;
                }
                saveImgPath = string.Concat(dir, "\\", videoFileName, ".jpg");
                ff.GetVideoFirstImage(saveImgPath, videoInfo.Width, videoInfo.Height, duration);
                //生成的缩略图的路径格式是：原图路径 + “-”+ dpi + 后缀
                if (System.IO.File.Exists(saveImgPath))
                {
                    string desPath = string.Concat(dir, "\\", videoFileName, "-" + _videoCoverDpi, ".jpg");// videoFullPath.Insert(videoFullPath.LastIndexOf('.'), "-" + _videoCoverDpi);
                    ThumbnailHelper.GenerateImage2(saveImgPath, desPath, whInt[0], whInt[1]);
                }
            }
            #endregion
        }

        /// <summary>
        /// 获取dpi的宽和高
        /// </summary>
        /// <param name="dpi"></param>
        /// <returns></returns>
        private int[] GetDpi(string dpi)
        {
            string[] wh = dpi.ToUpper().Split('X');
            int[] whInt = new int[2];
            if (wh.Length > 1)
            {
                whInt = wh.Select(o => int.Parse(o)).ToArray();
            }
            return whInt;
        }

		/// <summary>
		/// 保存上传文件到服务器
		/// </summary>
		/// <param name="hfc"></param>
		/// <param name="extFormat">指定文件后缀列表</param>
		/// <param name="maxLength">最大长度(字节)</param>
		/// <returns>返回保存到数据库中的路径</returns>
		private UploadifyResult SaveUploadFiles(List<string> extFormat = null, long maxLength = -1)
        {
            UploadifyResult uploadifyResult = new UploadifyResult();
            List<string> pathList = new List<string>();//返回保存到数据库中的路径
            string baseDirPath = GetInterimDefaultPath();//保存文件夹路径，路径是从根目录开始
            string dirFullPath = WebHelper.GetMapPath(baseDirPath);//保存文件夹完整路径
            if (!Directory.Exists(dirFullPath))
            {
                Directory.CreateDirectory(dirFullPath);
            }
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc != null && hfc.Count > 0)
            {
                string ext = string.Empty;
                string fileName = string.Empty;
                foreach (string fn in hfc)
                {
                    var file = hfc[fn];
                    string sourceFileName = file.FileName;
                    if (file.FileName.IndexOf('.') < 0)
                    {
                        if (string.IsNullOrWhiteSpace(_fileName) == false)
                        {
                            sourceFileName = _fileName;
                        }
                        else {
                            if (sourceFileName.IndexOf("video")==0)
                            {
                                sourceFileName = file.FileName + ".mp4";//视频的话，默认就是mp4
                            }
                            else
                            {  
                                sourceFileName = file.FileName + ".jpg"; //图片的话，默认就是.jpg
                            }
                        }
                    }
                    ext = Path.GetExtension(sourceFileName);
                    //格式不正确或超过大小的数据不做处理
                    if ((extFormat == null || extFormat.Contains(ext)) && (maxLength == -1 || ((maxLength > 0 && file.ContentLength <= maxLength))))
                    {
                        string nameWithoutExt = Path.GetFileNameWithoutExtension(sourceFileName);
                        fileName = string.Format("{0}{1}{2}", "[" + nameWithoutExt + "]", IdentityCreator.NextIdentity, ext);
                        file.SaveAs(string.Format("{0}{1}", dirFullPath, fileName));
                        pathList.Add(string.Format("{0}{1}", baseDirPath, fileName));

                        //设置其他参数，默认是最后一个文件大小
                        uploadifyResult.fileName = sourceFileName;
                        uploadifyResult.fileSize = file.ContentLength;//字节

                    }
                }
            }
            else {
                if (_base64String.IsNotNullAndNotEmpty())
                {
                    byte[] bt = Convert.FromBase64String(_base64String);
                    using (System.IO.MemoryStream stream = new System.IO.MemoryStream(bt))
                    {
                        Bitmap bitmap = new Bitmap(stream);
                        if (_fileName.IsNullOrEmpty())
                        {
                            throw new Exception("采用base64上传图片，必须传文件名");
                        }
                        string ext = Path.GetExtension(_fileName);
                        string nameWithoutExt = Path.GetFileNameWithoutExtension(_fileName);
                        string fileName = string.Format("{0}{1}{2}", "[" + nameWithoutExt + "]", IdentityCreator.NextIdentity, ext);
                        bitmap.Save(string.Format("{0}{1}", dirFullPath, fileName));
                        pathList.Add(string.Format("{0}{1}", baseDirPath, fileName));

                        //设置其他属性
                        uploadifyResult.fileName = _fileName;
                        uploadifyResult.fileSize = (int)stream.Length;//字节
                    }

                }
            }

            uploadifyResult.uploadPath = string.Join(_splitChar, pathList);

            return uploadifyResult;
        }

        #endregion



        #region 公共方法
        
        /// <summary>
        /// 获取临时文件夹保存的相对路径，比如 \Upload\Interim\20160101\
        /// </summary>
        /// <returns></returns>
        public string GetInterimDefaultPath()
		{
			return string.Format(@"{0}/{1}/", WebSettingsManager.UploadInterimDefaultPath.TrimEnd('/'), DateTime.Now.ToString("yyyyMMdd"));
		}

		/// <summary>
		/// 获取上传文件保存的文件夹相对路径，比如 \Upload\Files\Course\20160101\
		/// </summary>
		/// <param name="categoryType"></param>
		/// <returns></returns>
		public string GetFilesSaveDirPath(string categoryType)
		{
			return string.Format(@"{0}/{1}/{2}/", WebSettingsManager.UploadFilesDefaultPath.TrimEnd('/'), categoryType, DateTime.Now.ToString("yyyyMMdd"));
		}

		public string GetImagesSaveDirPath(string categoryType)
		{
			return string.Format(@"{0}/{1}/{2}/", WebSettingsManager.UploadImagesDefaultPath.TrimEnd('/'), categoryType, DateTime.Now.ToString("yyyyMMdd"));
		}

        #endregion



    }
}
