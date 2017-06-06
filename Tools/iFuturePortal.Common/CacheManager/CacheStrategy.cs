using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
namespace CourseManager.Common
{
	/// <summary>
	///  系统缓存策略
	/// </summary>
	public static class CacheStrategy
	{
		private static Cache _cache;
		private static int _timeout = 3600;//单位秒
		private static ICacheManager _cacheManager;
		private static object _lockObj = new object();//锁对象
		static CacheStrategy()
		{
			_cache = HttpRuntime.Cache;
			_cacheManager = new CacheManger();
		}

		/// <summary>
		/// 获得指定键的缓存值
		/// </summary>
		/// <param name="key">缓存键</param>
		/// <returns>缓存值</returns>
		public static object Get(string key)
		{
			if (string.IsNullOrEmpty(key)) return null;

			return _cache.Get(_cacheManager.GenerateGetDataKey(key));
		}

		/// <summary>
		/// 获得指定键的缓存值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="setAction">不存在就设置</param>
		/// <param name="minute"></param>
		/// <returns></returns>
		public static T Get<T>(string key, Func<T> setAction, int minute) where T : class
		{
			if (string.IsNullOrEmpty(key)) return null;
			object local = Get(_cacheManager.GenerateGetDataKey(key));
			if (local == null && setAction != null)
			{
				local = setAction();
				Insert(key, local, minute);
			}
			return (T)local;
		}

		/// <summary>
		/// 将指定键的对象添加到缓存中(默认缓存一个小时（3600s）)
		/// </summary>
		/// <param name="key">缓存键</param>
		/// <param name="data">缓存值</param>
		public static void Insert(string key, object data)
		{
			if (string.IsNullOrWhiteSpace(key) || data == null)
				return;
			lock (_lockObj)
			{
				_cache.Insert(_cacheManager.GenerateInsertCacheKey(key), data, null, DateTime.Now.AddSeconds(_timeout), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
			}

		}

		/// <summary>
		/// 将指定键的对象添加到缓存中，并指定过期时间
		/// </summary>
		/// <param name="key">缓存键</param>
		/// <param name="data">缓存值</param>
		/// <param name="cacheTime">缓存过期时间</param>
		public static void Insert(string key, object data, int cacheTime)
		{
			if (string.IsNullOrWhiteSpace(key) || data == null)
				return;
			lock (_lockObj)
			{
				_cache.Insert(_cacheManager.GenerateInsertCacheKey(key), data, null, DateTime.Now.AddSeconds(cacheTime), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
			}
		}

		public static void Insert(string key, object data, DateTime absoluteExpiration)
		{
			if (string.IsNullOrWhiteSpace(key) || data == null)
				return;
			lock (_lockObj)
			{
				_cache.Insert(_cacheManager.GenerateInsertCacheKey(key), data, null, absoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.High, null);
			}
		}

		/// <summary>
		/// 从缓存中移除指定键的缓存值
		/// </summary>
		/// <param name="key">缓存键</param>
		public static void Remove(string key)
		{
			if (string.IsNullOrEmpty(key)) return;
			lock (_lockObj)
			{
				foreach (string ky in _cacheManager.GenerateRemoveKeys(key))
					_cache.Remove(ky);
			}
		}

		/// <summary>
		/// 清空所有缓存对象
		/// </summary>
		public static void Clear()
		{
			lock (_lockObj)
			{
				IDictionaryEnumerator cacheEnum = _cache.GetEnumerator();
				while (cacheEnum.MoveNext())
					_cache.Remove(cacheEnum.Key.ToString());
			}
		}
		/// <summary>
		/// 刷新过期时间
		/// </summary>
		public static void RefreshExpiredTime(string key, object value)
		{
			_cache.Remove(key);
			Insert(key, value, 3600);
		}
	}
}
