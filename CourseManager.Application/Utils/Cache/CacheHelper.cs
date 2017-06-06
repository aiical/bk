using Abp.Dependency;
using Abp.Runtime.Caching;
using CourseManager.Core.Cache;
using CourseManager.Core.EntitiesFromCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Utils.Cache
{
    /// <summary>
    /// 缓存统一调用类
    /// </summary>
    public class CacheHelper: ISingletonDependency
    {
        private readonly ICacheManager _cacheManager;
        private readonly ICache _cache;

        public CacheHelper(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
            //CourseManager的缓存统一放到固定的节点下（CacheKeys.CacheNodeName）
            _cache = _cacheManager.GetCache(CacheKeys.CacheNodeName);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public RowListData<T> GetAll<T>(string key, Func<List<T>> factory) where T : class, new()
        {
            var result = new RowListData<T>();
            result.rows = _cache.Get(key, () => {
                if (factory != null)
                {
                    return factory();
                }
                else
                {
                    return new List<T>();
                }
            });
            result.total = result.rows.Count;
            return result;

        }

        public List<T> GetAllList<T>(string key, Func<List<T>> factory) where T : class, new()
        {
            return _cache.Get(key, () => {
                if (factory != null)
                {
                    return factory();
                }
                else
                {
                    return new List<T>();
                }
            });

        }

        public void DeleteCacheData(string key) {
            _cache.Remove(key);
        }

    }

}
