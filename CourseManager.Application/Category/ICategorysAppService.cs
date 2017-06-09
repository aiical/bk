using CourseManager.Category.Dtos;
using System.Collections.Generic;

namespace CourseManager.Category
{
    public interface ICategorysAppService
    {
        CategorysDto GetCategorysBy(CategorysInput categorysInput);

        List<CategorysDto> GetCategorysPageListBy(CategorysInput categorysInput);
    }
}
