using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CourseManager.Common
{
	public class CacheManger : ICacheManager
	{
		private Hashtable _cachekeys = new Hashtable();//缓存键列表
		/// <summary>
		/// 保存缓存键到_cacheKeys哈希表中
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public void SaveKey2CacheKeys(string key)
		{
			if (!_cachekeys.Contains(key))
			{
				_cachekeys.Add(key, DateTime.Now.ToString());
			}
		}

		public string GenerateGetDataKey(string key)
		{
			return key;
		}

		public string GenerateInsertCacheKey(string key)
		{
			SaveKey2CacheKeys(key);
			return key;
		}

		public List<string> GenerateRemoveKeys(string key)
		{
			List<string> matchedKeyList = new List<string>();
			Regex regex = new Regex(key, RegexOptions.IgnoreCase | RegexOptions.Singleline);
			foreach (string ky in _cachekeys.Keys)
			{
				if (regex.IsMatch(ky))
					matchedKeyList.Add(ky);
			}
			return matchedKeyList;
		}
	}
}
