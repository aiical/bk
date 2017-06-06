using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CourseManager.Common.Extensions;
using CourseManager.Common;

namespace CourseManager.Common.Config.ImagesDPI
{
    /// <summary>
    /// 上传图片尺寸相关设置帮助类
    /// </summary>
    public class ImagesDpiConfigHelper
    {
        public Lazy<AppConfig.WebSettings> WebSettingsManager = new Lazy<AppConfig.WebSettings>(() => { return new AppConfig.WebSettings(); });

        public List<ImagesDpiDetail> GetImagesDpiConfig()
        {
            return CacheStrategy.Get<List<ImagesDpiDetail>>(CacheKeys.ImagesDpiConfigCacheKey, () =>
            {
            string menuXmlPath = WebHelper.GetMapPath(WebSettingsManager.Value.ImagesDpiConfigXmlPath);
            return XmlDeserialize<ImagesDpiGroup>(menuXmlPath).ImagesDpiDetails;
           }, 360);
        }
        public ImagesDpiDetail GetImagesDpiConfigByCategoryType(string categoryType)
        {
            if (categoryType.IsNullOrEmpty())
            {
                return null;
            }
            ImagesDpiDetail imagesDpiDetail = null;
            var allImageDpi = GetImagesDpiConfig();
            if (allImageDpi != null)
            {
                imagesDpiDetail = allImageDpi.FirstOrDefault(o => o.id == categoryType);
            }
            return imagesDpiDetail;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalImagePath"></param>
        /// <param name="imagesDpiDetail"></param>
        /// <param name="fileType">image/video</param>
        public Tuple<int, int> MakeThumbnail(string originalImagePath, ImagesDpiDetail imagesDpiDetail, string fileType = "image")
        {
            Tuple<int, int> wh = Tuple.Create(0, 0);
            if (imagesDpiDetail != null)
            {
                imagesDpiDetail.SectionItems.ForEach(s =>
                {
                    //生成的缩略图的路径格式是：原图路径 + “-”+ dpi + 后缀
                    string desPath = originalImagePath.Insert(originalImagePath.LastIndexOf('.'), "-" + (s.dpi ?? imagesDpiDetail.def));
                    //ThumbnailHelper.GenerateImage(originalImagePath, desPath, s.width, s.height);
                    if (!System.IO.File.Exists(desPath))
                    {
                        wh = ThumbnailHelper.GenerateImage2(originalImagePath, desPath, s.width, s.height, fileType);
                    }
                });
            }
            return wh;
        }


        /// <summary>
        /// 对菜单数据进行序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="menuXmlPath"></param>
        /// <returns></returns>
        private T XmlDeserialize<T>(string menuXmlPath)
        {
            using (FileStream stream = new FileStream(menuXmlPath, FileMode.Open, FileAccess.Read))
            {
                return (T)(new XmlSerializer(typeof(T)).Deserialize(stream));
            }
        }
    }
}
