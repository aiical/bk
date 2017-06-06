using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Common
{
	public interface ICacheManager
	{
		/// <summary>
		/// 生成获取缓存数据的键
		/// </summary>
		/// <param name="key"></param>
		string GenerateGetDataKey(string key);

		/// <summary>
		/// 生成插入缓存数据的键
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		string GenerateInsertCacheKey(string key);

		/// <summary>
		/// 生成要移除的缓存键列表
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		List<string> GenerateRemoveKeys(string key);
	}
}
