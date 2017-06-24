using System.Net.Security;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Security.Cryptography.X509Certificates;

namespace CourseManager.Common
{

	public class WebHelper
	{
		//搜索引擎列表
		private static string[] _searchenginelist = new string[] { "baidu", "google", "360", "sogou", "bing", "msn", "sohu", "soso", "sina", "163", "yahoo", "jikeu" };
		//meta正则表达式
		private static Regex _metaregex = new Regex("<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase | RegexOptions.Multiline);
		//浏览器列表
		private static string[] _browserlist = new string[] { "ie", "chrome", "mozilla", "netscape", "firefox", "opera", "konqueror", "360" };
		#region web 编码助手
		/// <summary>
		/// HTML解码
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string HtmlDecode(string str)
		{
			return HttpUtility.HtmlDecode(str);
		}

		/// <summary>
		/// HTML编码
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string HtmlEncode(string str)
		{
			return HttpUtility.HtmlEncode(str);
		}

		/// <summary>
		/// url编码
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string UrlEncode(string str)
		{
			return HttpUtility.UrlEncode(str);
		}
		/// <summary>
		/// url解码
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string UrlDecode(string str)
		{
			return HttpUtility.UrlDecode(str);
		}
		#endregion

		#region cookie管理
		/// <summary>
		/// 获取指定名称的cookie值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string GetCookie(string name)
		{
			HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
			if (cookie != null)
				return cookie.Value;
			return string.Empty;
		}

		/// <summary>
		/// 删除指定名称的cookie
		/// </summary>
		/// <param name="name"></param>
		public static void DeleteCookie(string name)
		{
			HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
			cookie.Expires = DateTime.Now.AddYears(-1);
			HttpContext.Current.Response.AppendCookie(cookie);
		}

		/// <summary>
		/// 获取指定名称 指定键的cookie值
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="key">键</param>
		/// <returns></returns>
		public static string GetCookie(string name, string key)
		{
			HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
			if (cookie != null && cookie.HasKeys)
			{
				string value = cookie[key];
				if (value != null) return value;
			}
			return string.Empty;
		}


		/// <summary>
		/// 设定指定名称的cookie值
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="value">值</param>
		public static void SetCookie(string name, string value)
		{
			HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
			if (cookie == null)
				cookie = new HttpCookie(name);

			cookie.Value = value;
			HttpContext.Current.Response.AppendCookie(cookie);
		}

		/// <summary>
		/// 设定指定名称和过期时间的cookie
		/// </summary>
		/// <param name="name">名称</param>
		/// <param name="value">值</param>
		/// <param name="expires">过期时间(分钟)</param>
		public static void SetCookie(string name, string value, double expires)
		{
			HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
			if (cookie == null)
			{
				cookie = new HttpCookie(name);
			}
			cookie.Value = value;
			cookie.Expires.AddMinutes(expires);

			HttpContext.Current.Response.AppendCookie(cookie);
		}
		/// <summary>
		/// 设置指定名称的Cookie特定键的值
		/// </summary>
		/// <param name="name">Cookie名称</param>
		/// <param name="key">键</param>
		/// <param name="value">值</param>
		public static void SetCookie(string name, string key, string value)
		{
			HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
			if (cookie == null)
				cookie = new HttpCookie(name);

			cookie[key] = value;
			HttpContext.Current.Response.AppendCookie(cookie);
		}

		/// <summary>
		/// 设置指定名称的Cookie特定键的值
		/// </summary>
		/// <param name="name">Cookie名称</param>
		/// <param name="key">键</param>
		/// <param name="value">值</param>
		/// <param name="expires">过期时间</param>
		public static void SetCookie(string name, string key, string value, double expires)
		{
			HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
			if (cookie == null)
				cookie = new HttpCookie(name);

			cookie[key] = value;
			cookie.Expires = DateTime.Now.AddMinutes(expires);
			HttpContext.Current.Response.AppendCookie(cookie);
		}
		#endregion

		#region  统一获取客户端(浏览器端/app端)信息
		/// <summary>
		/// 是否是get请求
		/// </summary>
		/// <returns></returns>
		public static bool IsGet()
		{
			return HttpContext.Current.Request.HttpMethod == "GET";
		}

		/// <summary>
		/// 是否是post请求
		/// </summary>
		/// <returns></returns>
		public static bool IsPost()
		{
			return HttpContext.Current.Request.HttpMethod == "POST";
		}

		/// <summary>
		/// 是否是ajax请求
		/// </summary>
		/// <returns></returns>
		public static bool IsAjax()
		{
			return HttpContext.Current.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
		}


		/// <summary>
		/// 获取查询字符串中key对应的value值
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string GetQueryString(string key)
		{
			return GetQueryString(key, "");
		}

		/// <summary>
		///  获取查询字符串中key对应的value值 可返回默认值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static string GetQueryString(string key, string defaultValue)
		{
			string value = HttpContext.Current.Request.QueryString[key];
			if (!string.IsNullOrWhiteSpace(value))
				return value;
			else
				return defaultValue;
		}

		/// <summary>
		/// 获得表单中的值
		/// </summary>
		/// <param name="key">键</param>
		/// <returns></returns>
		public static string GetFormString(string key)
		{
			return GetFormString(key, "");
		}
		/// <summary>
		/// 获得表单中的值
		/// </summary>
		/// <param name="key">键</param>
		/// <param name="defaultValue">默认值</param>
		/// <returns></returns>
		public static string GetFormString(string key, string defaultValue)
		{
			string value = HttpContext.Current.Request.Form[key];
			if (!string.IsNullOrWhiteSpace(value))
				return value;
			else
				return defaultValue;
		}

		/// <summary>
		/// 获得请求中的值（表单提交/url get提交）
		/// </summary>
		/// <param name="key">键</param>
		/// <param name="defaultValue">默认值</param>
		/// <returns></returns>
		public static string GetRequestString(string key, string defaultValue)
		{
			if (HttpContext.Current.Request.Form[key] != null)
				return GetFormString(key, defaultValue);
			else
				return GetQueryString(key, defaultValue);
		}
		/// <summary>
		/// 获得请求中的值（表单提交/url get提交）
		/// </summary>
		/// <param name="key">键</param>
		/// <returns></returns>
		public static string GetRequestString(string key)
		{
			if (HttpContext.Current.Request.Form[key] != null)
				return GetFormString(key);
			else
				return GetQueryString(key);
		}

		/// <summary>
		/// 获得上次请求的url
		/// </summary>
		/// <returns></returns>
		public static string GetUrlReferrer()
		{
			Uri uri = HttpContext.Current.Request.UrlReferrer;
			if (uri == null)
				return string.Empty;

			return uri.ToString();
		}

		/// <summary>
		/// 获得请求的主机部分
		/// </summary>
		/// <returns></returns>
		public static string GetHost()
		{
			return HttpContext.Current.Request.Url.Host;
		}

		/// <summary>
		/// 获得请求的url
		/// </summary>
		/// <returns></returns>
		public static string GetUrl()
		{
			return HttpContext.Current.Request.Url.ToString();
		}

		/// <summary>
		/// 获得请求的原始url
		/// </summary>
		/// <returns></returns>
		public static string GetRawUrl()
		{
			return HttpContext.Current.Request.RawUrl;
		}

		/// <summary>
		/// 获得请求的ip
		/// </summary>
		/// <returns></returns>
		public static string GetIP()
		{
			string ip = string.Empty;
			ip = HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString() : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

			if (string.IsNullOrEmpty(ip) || !ValidateHelper.IsIP(ip))
				ip = "127.0.0.1";
			return ip;
		}

		/// <summary>
		/// 获得请求的浏览器类型
		/// </summary>
		/// <returns></returns>
		public static string GetBrowserType()
		{
			string type = HttpContext.Current.Request.Browser.Type;
			if (string.IsNullOrEmpty(type) || type == "unknown")
				return "未知";

			return type.ToLower();
		}

		/// <summary>
		/// 获得请求的浏览器名称
		/// </summary>
		/// <returns></returns>
		public static string GetBrowserName()
		{
			string name = HttpContext.Current.Request.Browser.Browser;
			if (string.IsNullOrEmpty(name) || name == "unknown")
				return "未知";

			return name.ToLower();
		}

		/// <summary>
		/// 获得请求的浏览器版本
		/// </summary>
		/// <returns></returns>
		public static string GetBrowserVersion()
		{
			string version = HttpContext.Current.Request.Browser.Version;
			if (string.IsNullOrEmpty(version) || version == "unknown")
				return "未知";

			return version;
		}

		/// <summary>
		/// 获得请求客户端的操作系统类型
		/// </summary>
		/// <returns></returns>
		public static string GetOSType()
		{
			string userAgent = HttpContext.Current.Request.UserAgent;
			if (userAgent == null)
				return "未知";

			string type = null;
			if (userAgent.Contains("NT 6.1"))
				type = "Windows 7";
			else if (userAgent.Contains("NT 5.1"))
				type = "Windows XP";
			else if (userAgent.Contains("NT 6.2"))
				type = "Windows 8";
			else if (userAgent.Contains("android"))
				type = "Android";
			else if (userAgent.Contains("iphone"))
				type = "IPhone";
			else if (userAgent.Contains("Mac"))
				type = "Mac";
			else if (userAgent.Contains("NT 6.0"))
				type = "Windows Vista";
			else if (userAgent.Contains("NT 5.2"))
				type = "Windows 2003";
			else if (userAgent.Contains("NT 5.0"))
				type = "Windows 2000";
			else if (userAgent.Contains("98"))
				type = "Windows 98";
			else if (userAgent.Contains("95"))
				type = "Windows 95";
			else if (userAgent.Contains("Me"))
				type = "Windows Me";
			else if (userAgent.Contains("NT 4"))
				type = "Windows NT4";
			else if (userAgent.Contains("Unix"))
				type = "UNIX";
			else if (userAgent.Contains("Linux"))
				type = "Linux";
			else if (userAgent.Contains("SunOS"))
				type = "SunOS";
			else
				type = "未知";

			return type;
		}

		/// <summary>
		/// 获得请求客户端的操作系统名称
		/// </summary>
		/// <returns></returns>
		public static string GetOSName()
		{
			string name = HttpContext.Current.Request.Browser.Platform;
			if (string.IsNullOrEmpty(name))
				return "未知";

			return name;
		}

		/// <summary>
		/// 判断是否是浏览器请求
		/// </summary>
		/// <returns></returns>
		public static bool IsBrowser()
		{
			string name = GetBrowserName();
			foreach (string item in _browserlist)
			{
				if (name.Contains(item))
					return true;
			}
			return false;
		}

		/// <summary>
		/// 是否是移动设备请求
		/// </summary>
		/// <returns></returns>
		public static bool IsMobile()
		{
			if (HttpContext.Current.Request.Browser.IsMobileDevice)
				return true;

			bool isTablet = false;
			if (bool.TryParse(HttpContext.Current.Request.Browser["IsTablet"], out isTablet) && isTablet)
				return true;

			return false;
		}

		/// <summary>
		/// 判断是否是搜索引擎爬虫请求
		/// </summary>
		/// <returns></returns>
		public static bool IsCrawler()
		{
			bool result = HttpContext.Current.Request.Browser.Crawler;
			if (!result)
			{
				string referrer = GetUrlReferrer();
				if (referrer.Length > 0)
				{
					foreach (string item in _searchenginelist)
					{
						if (referrer.Contains(item))
							return true;
					}
				}
			}
			return result;
		}

		#endregion

		#region HTTP 请求

		/// <summary>
		/// 获得参数列表
		/// </summary>
		/// <param name="data">数据</param>
		/// <returns></returns>
		public static NameValueCollection GetParmList(string data)
		{
			NameValueCollection parmList = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
			if (!string.IsNullOrEmpty(data))
			{
				int length = data.Length;
				for (int i = 0; i < length; i++)
				{
					int startIndex = i;
					int endIndex = -1;
					while (i < length)
					{
						char c = data[i];
						if (c == '=')
						{
							if (endIndex < 0)
								endIndex = i;
						}
						else if (c == '&')
						{
							break;
						}
						i++;
					}
					string key;
					string value;
					if (endIndex >= 0)
					{
						key = data.Substring(startIndex, endIndex - startIndex);
						value = data.Substring(endIndex + 1, (i - endIndex) - 1);
					}
					else
					{
						key = data.Substring(startIndex, i - startIndex);
						value = string.Empty;
					}
					parmList[key] = value;
					if ((i == (length - 1)) && (data[i] == '&'))
						parmList[key] = string.Empty;
				}
			}
			return parmList;
		}

		/// <summary>
		/// 获得http请求数据
		/// </summary>
		/// <param name="url">请求地址</param>
		/// <param name="postData">发送数据</param>
		/// <returns></returns>
		public static string GetRequestData(string url, string postData)
		{
			return GetRequestData(url, "post", postData);
		}

		/// <summary>
		/// 获得http请求数据
		/// </summary>
		/// <param name="url">请求地址</param>
		/// <param name="method">请求方式</param>
		/// <param name="postData">发送数据</param>
		/// <returns></returns>
		public static string GetRequestData(string url, string method, string postData)
		{
			return GetRequestData(url, method, postData, Encoding.UTF8);
		}

		/// <summary>
		/// 获得http请求数据
		/// </summary>
		/// <param name="url">请求地址</param>
		/// <param name="method">请求方式</param>
		/// <param name="postData">发送数据</param>
		/// <param name="encoding">编码</param>
		/// <returns></returns>
		public static string GetRequestData(string url, string method, string postData, Encoding encoding)
		{
			return GetRequestData(url, method, postData, encoding, 20000);
		}

		/// <summary>
		/// 获得http请求数据
		/// </summary>
		/// <param name="url">请求地址</param>
		/// <param name="method">请求方式</param>
		/// <param name="postData">发送数据</param>
		/// <param name="encoding">编码</param>
		/// <param name="timeout">超时值</param>
		/// <returns></returns>
		public static string GetRequestData(string url, string method, string postData, Encoding encoding, int timeout)
		{
			if (!(url.Contains("http://") || url.Contains("https://")))
				url = "http://" + url;

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = method.Trim().ToLower();
			request.Timeout = timeout;
			request.AllowAutoRedirect = true;
			request.ContentType = "text/html";
			request.Accept = "text/html, application/xhtml+xml, */*,zh-CN";
			request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
			request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);

			try
			{
				if (!string.IsNullOrEmpty(postData) && request.Method == "post")
				{
					byte[] buffer = encoding.GetBytes(postData);
					request.ContentLength = buffer.Length;
					request.GetRequestStream().Write(buffer, 0, buffer.Length);
				}

				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				{
					if (encoding == null)
					{
						MemoryStream stream = new MemoryStream();
						if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
							new GZipStream(response.GetResponseStream(), CompressionMode.Decompress).CopyTo(stream, 10240);
						else
							response.GetResponseStream().CopyTo(stream, 10240);

						byte[] RawResponse = stream.ToArray();
						string temp = Encoding.Default.GetString(RawResponse, 0, RawResponse.Length);
						Match meta = _metaregex.Match(temp);
						string charter = (meta.Groups.Count > 2) ? meta.Groups[2].Value : string.Empty;
						charter = charter.Replace("\"", string.Empty).Replace("'", string.Empty).Replace(";", string.Empty);
						if (charter.Length > 0)
						{
							charter = charter.ToLower().Replace("iso-8859-1", "gbk");
							encoding = Encoding.GetEncoding(charter);
						}
						else
						{
							if (response.CharacterSet.ToLower().Trim() == "iso-8859-1")
							{
								encoding = Encoding.GetEncoding("gbk");
							}
							else
							{
								if (string.IsNullOrEmpty(response.CharacterSet.Trim()))
								{
									encoding = Encoding.UTF8;
								}
								else
								{
									encoding = Encoding.GetEncoding(response.CharacterSet);
								}
							}
						}
						return encoding.GetString(RawResponse);
					}
					else
					{
						StreamReader reader = null;
						if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
						{
							using (reader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress), encoding))
							{
								return reader.ReadToEnd();
							}
						}
						else
						{
							using (reader = new StreamReader(response.GetResponseStream(), encoding))
							{
								try
								{
									return reader.ReadToEnd();
								}
								catch
								{
									return "close";
								}

							}
						}
					}
				}

			}
			catch (WebException ex)
			{
				return "error";
			}
		}

		#endregion

		#region .NET 内部
		/// <summary>
		/// 获得当前应用程序的信任级别
		/// </summary>
		/// <returns></returns>
		public static AspNetHostingPermissionLevel GetTrustLevel()
		{
			AspNetHostingPermissionLevel trustLevel = AspNetHostingPermissionLevel.None;
			//权限列表
			AspNetHostingPermissionLevel[] levelList = new AspNetHostingPermissionLevel[] {
                                                                                            AspNetHostingPermissionLevel.Unrestricted,
                                                                                            AspNetHostingPermissionLevel.High,
                                                                                            AspNetHostingPermissionLevel.Medium,
                                                                                            AspNetHostingPermissionLevel.Low,
                                                                                            AspNetHostingPermissionLevel.Minimal 
                                                                                            };

			foreach (AspNetHostingPermissionLevel level in levelList)
			{
				try
				{
					//通过执行Demand方法检测是否抛出SecurityException异常来设置当前应用程序的信任级别
					new AspNetHostingPermission(level).Demand();
					trustLevel = level;
					break;
				}
				catch (SecurityException ex)
				{
					continue;
				}
			}
			return trustLevel;
		}
		public static void RestartAppDomain()
		{
			if (GetTrustLevel() > AspNetHostingPermissionLevel.Medium)//如果当前信任级别大于Medium，则通过卸载应用程序域的方式重启
			{
				HttpRuntime.UnloadAppDomain();
				TryWriteGlobalAsax();
			}
			else//通过修改web.config方式重启应用程序
			{
				bool success = TryWriteWebConfig();
				if (!success)
				{
					throw new Exception("修改web.config文件重启应用程序");
				}

				success = TryWriteGlobalAsax();
				if (!success)
				{
					throw new Exception("修改global.asax文件重启应用程序");
				}
			}
		}

		/// <summary>
		/// 修改global.asax文件
		/// </summary>
		/// <returns></returns>
		private static bool TryWriteGlobalAsax()
		{
			try
			{
				File.SetLastWriteTimeUtc(Helper.GetMapPath("~/global.asax"), DateTime.UtcNow);
				return true;
			}
			catch
			{
				return false;
			}
		}
		/// <summary>
		/// 尝试修改WebConfig来达到重启应用程序的效果
		/// </summary>
		/// <returns></returns>
		private static bool TryWriteWebConfig()
		{
			try
			{
				File.SetLastWriteTimeUtc(Helper.GetMapPath("~/web.config"), DateTime.UtcNow);
				return true;
			}
			catch
			{
				return false;
			}
		}
		#endregion

		#region 文件处理
		public static void CreateFolder(string folderPath)
		{
			if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
		}
		public static void CreateThumbnail(string pic, string path, string filename, string[] thumbnails)
		{
			if (thumbnails.Length == 0) thumbnails = new string[] { "200x200", "85x85" };
			foreach (var s in thumbnails)
			{
				var ss = s.Split(new[] { 'x' });
				if (ss.Length != 2) continue;
				var w = Helper.StrToInt(ss[0]);
				var h = Helper.StrToInt(ss[1]);
				if (w <= 0 || h <= 0) continue;
				CreateFolder(HttpContext.Current.Server.MapPath(path + s + "/"));
				ThumbnailHelper.MakeThumbnailImage(HttpContext.Current.Server.MapPath(pic), HttpContext.Current.Server.MapPath(path + s + "/") + filename, w, h);
			}
		}
		public static string GetTrueWebPath()
		{
			string webPath = HttpContext.Current.Request.Path;
			webPath = webPath.LastIndexOf("/", StringComparison.Ordinal) !=
				    webPath.IndexOf("/", StringComparison.Ordinal)
					  ? webPath.Substring(webPath.IndexOf("/", StringComparison.Ordinal),
								    webPath.LastIndexOf("/", StringComparison.Ordinal) + 1)
					  : "/";

			return webPath;
		}

		public static string GetRootUrl(string forumPath)
		{
			int port = HttpContext.Current.Request.Url.Port;
			return string.Format("{0}://{1}{2}{3}",
						   HttpContext.Current.Request.Url.Scheme,
						   HttpContext.Current.Request.Url.Host,
						   (port == 80 || port == 0) ? "" : ":" + port,
						   forumPath);
		}
		private static readonly char[] InvalidFileNameChars = new[]
                                                                  {
                                                                      '"',
                                                                      '<',
                                                                      '>',
                                                                      '|',
                                                                      '\0',
                                                                      '\u0001',
                                                                      '\u0002',
                                                                      '\u0003',
                                                                      '\u0004',
                                                                      '\u0005',
                                                                      '\u0006',
                                                                      '\a',
                                                                      '\b',
                                                                      '\t',
                                                                      '\n',
                                                                      '\v',
                                                                      '\f',
                                                                      '\r',
                                                                      '\u000e',
                                                                      '\u000f',
                                                                      '\u0010',
                                                                      '\u0011',
                                                                      '\u0012',
                                                                      '\u0013',
                                                                      '\u0014',
                                                                      '\u0015',
                                                                      '\u0016',
                                                                      '\u0017',
                                                                      '\u0018',
                                                                      '\u0019',
                                                                      '\u001a',
                                                                      '\u001b',
                                                                      '\u001c',
                                                                      '\u001d',
                                                                      '\u001e',
                                                                      '\u001f',
                                                                      ':',
                                                                      '*',
                                                                      '?',
                                                                      '\\',
                                                                      '/'
                                                                  };
		public static string CleanInvalidFileName(string fileName)
		{
			fileName = fileName + "";
			fileName = InvalidFileNameChars.Aggregate(fileName, (current, c) => current.Replace(c + "", ""));

			if (fileName.Length > 1)
				if (fileName[0] == '.')
					fileName = "dot" + fileName.TrimStart('.');

			return fileName;
		}

		
		public static string CleanInvalidXmlChars(string text)
		{
			const string re = @"[^\x09\x0A\x0D\x20-\xD7FF\xE000-\xFFFD\x10000-x10FFFF]";
			return Regex.Replace(text, re, "");
		}

		public static void CreateHtml(string url, string outpath)
		{
			FileStream fs;
			if (File.Exists(outpath))
			{
				File.Delete(outpath);
				fs = File.Create(outpath);
			}
			else
			{
				fs = File.Create(outpath);
			}
			byte[] bt = Encoding.UTF8.GetBytes(GetSourceTextByUrl(url));
			fs.Write(bt, 0, bt.Length);
			fs.Close();
		}
		public static string GetSourceTextByUrl(string url)
		{
			try
			{
				WebRequest request = WebRequest.Create(url);
				request.Timeout = 20000;
				WebResponse response = request.GetResponse();

				Stream resStream = response.GetResponseStream();
				if (resStream != null)
				{
					var sr = new StreamReader(resStream);
					return sr.ReadToEnd();
				}
			}
			catch
			{
			}
			return string.Empty;
		}
		public static void DeleteHtml(string path)
		{
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}

        #endregion

		private static WebProxy _webproxy = null;
		#region Get

		/// <summary>
		/// 使用Get方法获取字符串结果（没有加入Cookie）
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static string HttpGet(string url, Encoding encoding = null)
		{
			WebClient wc = new WebClient();
			wc.Proxy = _webproxy;
			wc.Encoding = encoding ?? Encoding.UTF8;
			return wc.DownloadString(url);
		}

		#endregion

        public static string GetMapPath(string path)
        {
            if (path.IndexOf(Path.VolumeSeparatorChar) > -1)
            {
                return path;
            }
            if (!path.StartsWith("~"))
            {
                path = string.Concat(@"~", path);
            }
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(path);
            }
            else
            {
                return System.Web.Hosting.HostingEnvironment.MapPath(path);
            }
        }
        public static bool CheckUri(string strUri)
        {
            try
            {
                System.Net.HttpWebRequest.Create(strUri).GetResponse();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ReplaceSpecialChar(string source)
        {
            return string.IsNullOrEmpty(source)
                ? source
                : source.Replace("\r\n", "<br/>").Replace("\n", "<br/>").Replace(" ", "&nbsp;");
        }

    }
}
