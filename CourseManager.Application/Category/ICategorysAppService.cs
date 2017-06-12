using Abp.Application.Services;
using CourseManager.Category.Dtos;
using System.Collections.Generic;

namespace CourseManager.Category
{
    public interface ICategorysAppService: IApplicationService
    {
        IEnumerable<CategorysDto> GetAllCategorys();
        CategorysDto GetCategorysBy(CategorysInput categorysInput);

        List<CategorysDto> GetCategorysPageListBy(CategorysInput categorysInput);
    }
}
