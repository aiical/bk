using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseManager.File;
namespace CourseManager.IRepositories
{
    public interface IFileRepository : IRepository<Files>
    {
        void Submit();
    }
}
