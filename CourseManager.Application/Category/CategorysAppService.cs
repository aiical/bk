using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using CourseManager.Common.Extensions;
using CourseManager.Utils.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using CourseManager.Core.Cache;
using CourseManager.Category.Dtos;

namespace CourseManager.Category
{
    public class CategorysAppService : CourseManagerAppServiceBase, ICategorysAppService
    {
        private readonly IRepository<Categorys, string> _categorysRepository = null;
        protected CacheHelper _cacheHelper;
        public CategorysAppService(IRepository<Categorys, string> repository, CacheHelper cacheHelper)
        {
            _categorysRepository = repository;
            _cacheHelper = cacheHelper;
        }

        public IEnumerable<CategorysDto> GetAllCategorys()
        {
            var data = _cacheHelper.GetAllList(CacheKeys.GetAllCategory, () =>
            {
                return _categorysRepository.GetAllList().Where(o => o.IsDeleted == false).OrderByDescending(c => c.CategoryType).ThenBy(c => c.SortNo).MapTo<List<CategorysDto>>();
            });
            return data;
        }

        #region 根据查询条件获取数据
        private IEnumerable<CategorysDto> GetCategorysByCondition(CategorysInput categorysInput)
        {
            return GetAllCategorys()
                .WhereIf(categorysInput.CategoryType.IsNotNullAndNotEmpty(), o => o.CategoryType == categorysInput.CategoryType)
                .WhereIf(categorysInput.DictionaryValue.IsNotNullAndNotEmpty(), o => o.DictionaryValue == categorysInput.DictionaryValue)
                .WhereIf(categorysInput.Level.HasValue, o => o.Level == categorysInput.Level)
                .WhereIf(categorysInput.ParentId.IsNotNullAndNotEmpty(), o => o.ParentId == categorysInput.ParentId)
                .WhereIf(categorysInput.SysDefined.HasValue, o => o.SysDefined == categorysInput.SysDefined)
                 .WhereIf(categorysInput.CategoryName.IsNotNullAndNotEmpty(), o => o.CategoryName == categorysInput.CategoryName);
        }

        public CategorysDto GetCategorysBy(CategorysInput categorysInput)
        {
            var t = GetCategorysByCondition(categorysInput).FirstOrDefault();
            return t;
        }

        public List<CategorysDto> GetCategorysPageListBy(CategorysInput categorysInput)
        {
            var result = GetCategorysByCondition(categorysInput).OrderBy(o => o.SortNo).ToList();
            return result;
        }

        #endregion
    }
}
