
using System;
using System.Configuration;
using System.Xml.Linq;
using CourseManager.Common.Extensions;
using CourseManager.Common;
namespace CourseManager.Common.Config
{
    /// <summary>
    /// 配置文件信息获取类：Web.config，
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// 网站配置信息获取类
        /// 调用方式：<see cref="AppConfig.WebSettings"/>
        /// </summary>
        public class WebSettings
        {
            #region metenConfig文件内容获取和缓存

            //缓存key
            private const string cacheKey = "metenConfig.conf";
            public XElement root
            {
                get
                {
                    string setting = CacheStrategy.Get(cacheKey, () =>
                    {
                        var xmlPath = WebHelper.GetMapPath(AppSettings.MetenConfigPath);
                        if (System.IO.File.Exists(xmlPath))
                        {
                            XElement x = XElement.Load(xmlPath);
                            return x.ToString();
                        }
                        throw new Exception("网站主要配置文件不存在");
                    }, 480);
                    return XElement.Parse(setting);
                }
                set
                {
                    XElement a = value;
                    CacheStrategy.Insert(cacheKey, a, 480);
                }
            }
            public void ReLoad()
            {
                var xml_path = WebHelper.GetMapPath(AppSettings.MetenConfigPath);
                root = XElement.Load(xml_path);
            }

            /// <summary>
            /// 根据Key获取对应配置的值
            /// </summary>
            /// <param name="key"></param>
            /// <param name="defaultValue">默认值</param>
            /// <returns></returns>
            private string GetWebSettingValueByKey(string key, string defaultValue = "")
            {
                if (root.Element(key) != null)
                {
                    return root.Element(key).Value;
                }
                else
                {
                    root.Add(new XElement(key, defaultValue));
                    return defaultValue;
                }
            }

            #endregion

            #region 基础配置

            /// <summary>
            /// 图片上传默认主路径
            /// </summary>
            public string UploadImagesDefaultPath
            {
                get
                {
                    return GetWebSettingValueByKey("UploadImagesDefaultPath", "/Upload/Images/");
                }
            }
            /// <summary>
            /// 文件上传默认主路径，比如： /Upload/Files/
            /// </summary>
            public string UploadFilesDefaultPath
            {
                get
                {
                    return GetWebSettingValueByKey("UploadFilesDefaultPath", "/Upload/Files/");
                }
            }
            /// <summary>
            /// 上传文件被删除回收主路径，比如： /Upload/Recycle/
            /// </summary>
            public string UploadRecycleDefaultPath
            {
                get
                {
                    return GetWebSettingValueByKey("UploadRecycleDefaultPath", "/Upload/Recycle/");
                }
            }
            /// <summary>
            /// 上传文件临时保存路径，比如： /Upload/Interim/
            /// </summary>
            public string UploadInterimDefaultPath
            {
                get
                {
                    return GetWebSettingValueByKey("UploadInterimDefaultPath", "/Upload/Interim/");
                }
            }

            /// <summary>
            /// 软实力Pdf长期存放路径，主要存放低质量的pdf：/Upload/Files/Pdf/SoftPower/
            /// </summary>
            public string DownLoadSoftPowerPdfDefaultPath
            {
                get
                {
                    return GetWebSettingValueByKey("DownLoadSoftPowerPdfDefaultPath", "/Upload/Files/Pdf/SoftPower/");
                }
            }

            /// <summary>
            /// 资料输出主域，比如：http://www.meten.com
            /// 用于拼凑完整url进行下载操作
            /// </summary>
            public string OutPutUploadResourceUrl
            {
                get
                {
                    return GetWebSettingValueByKey("OutPutUploadResourceUrl", "http://localhost");
                }
            }
            /// <summary>
            /// 本产品名称
            /// </summary>
            public string LocalProductDefaultName
            {
                get
                {
                    return GetWebSettingValueByKey("LocalProductDefaultName", "美联英语");
                }
            }
            /// <summary>
            /// Web登录有效时长(分钟)
            /// </summary>
            public int WebLoginTokenMaxExpire
            {
                get
                {
                    return Convert.ToInt32(GetWebSettingValueByKey("WebLoginTokenMaxExpire", "600"));
                }
            }


            #endregion
            #region 表格文件及业务相关配置

            /// <summary>
            /// 通用导出Excel表格模版地址
            /// </summary>
            public string OutputCommonExcelPath
            {
                get
                {
                    return GetWebSettingValueByKey("OutputCommonExcelPath", "/Configs/Excels/metenOutput.xls");
                }
            }
            /// <summary>
            /// 上传图片尺寸相关设置
            /// </summary>
            public string ImagesDpiConfigXmlPath
            {
                get
                {
                    return GetWebSettingValueByKey("ImagesDpiConfigXmlPath", "/Configs/ImagesDPI.xml");
                }
            }
            #endregion

            #region 成功案例导入配置

            public string StudentCourseArrangeImportConfig
            {
                get
                {
                    return GetWebSettingValueByKey("StudentCourseArrangeImportConfig", "/Configs/ImportConfigs/StudentCourseArrangeImportConfig.xml");
                }
            }
            #endregion

        }
        /// <summary>
        /// 用于返回Web.config中的 appSettings 节点内容
        /// 调用方式：<see cref="AppConfig.AppSettings"/>
        /// </summary>
        public class AppSettings
        {
            private static string GetAppSettingValueByKey(string key, string defaultValue = "")
            {
                var o = ConfigurationManager.AppSettings[key];
                if (o.IsNullOrWhiteSpace())
                    o = defaultValue;
                return o;
            }


            /// <summary>
            /// 网站主要配置文件路径
            /// </summary>
            public static string MetenConfigPath
            {
                get
                {
                    return GetAppSettingValueByKey("MetenConfigPath", @"\Configs\metenConfig.conf");
                }
            }


        }

    }
}
