using CourseManager.Category;
using CourseManager.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseManager.Migrations.SeedData
{
    public class DefaultCategorysCreator
    {
        private readonly CourseManagerDbContext _context;

        private readonly List<Categorys> _categorys;

        public DefaultCategorysCreator(CourseManagerDbContext context)
        {
            _context = context;
            _categorys = new List<Categorys>()
            {
                new Categorys{ Id=Common.IdentityCreator.NewGuid, CategoryName="1V1",DictionaryValue="one2One", CategoryType="CourseType", CreationTime=DateTime.Now, CreatorUserId=0, IsActive=true, IsDeleted=false, Level=1, SortNo=1, SysDefined=1, TenantId=1,ParentId="0"  },
                    new Categorys{ Id=Common.IdentityCreator.NewGuid, CategoryName="班级课",DictionaryValue="ClassCourse", CategoryType="CourseType", CreationTime=DateTime.Now, CreatorUserId=0, IsActive=true, IsDeleted=false, Level=1, SortNo=2, SysDefined=1, TenantId=1,ParentId="0"  },
                   new Categorys{ Id=Common.IdentityCreator.NewGuid, CategoryName="迟到",DictionaryValue="Late", CategoryType="SignInRecordType", CreationTime=DateTime.Now, CreatorUserId=0, IsActive=true, IsDeleted=false, Level=1, SortNo=1, SysDefined=1, TenantId=1,ParentId="0"  },
                    new Categorys{ Id=Common.IdentityCreator.NewGuid, CategoryName="准时上课",DictionaryValue="Normal", CategoryType="SignInRecordType", CreationTime=DateTime.Now, CreatorUserId=0, IsActive=true, IsDeleted=false, Level=1, SortNo=2, SysDefined=1, TenantId=1,ParentId="0"  },
                         new Categorys{ Id=Common.IdentityCreator.NewGuid, CategoryName="未上课",DictionaryValue="NoCourse", CategoryType="SignInRecordType", CreationTime=DateTime.Now, CreatorUserId=0, IsActive=true, IsDeleted=false, Level=1, SortNo=3, SysDefined=1, TenantId=1,ParentId="0"  },

                            new Categorys{ Id=Common.IdentityCreator.NewGuid, CategoryName="老师请假",DictionaryValue="TeacherLeave", CategoryType="NoCourseReasonType", CreationTime=DateTime.Now, CreatorUserId=0, IsActive=true, IsDeleted=false, Level=1, SortNo=1, SysDefined=1, TenantId=1,ParentId="0"  },
                               new Categorys{ Id=Common.IdentityCreator.NewGuid, CategoryName="学生请假",DictionaryValue="StudentLeave", CategoryType="NoCourseReasonType", CreationTime=DateTime.Now, CreatorUserId=0, IsActive=true, IsDeleted=false, Level=1, SortNo=2, SysDefined=1, TenantId=1,ParentId="0"  },
                                new Categorys{ Id=Common.IdentityCreator.NewGuid, CategoryName="其他原因",DictionaryValue="OtherReason", CategoryType="NoCourseReasonType", CreationTime=DateTime.Now, CreatorUserId=0, IsActive=true, IsDeleted=false, Level=1, SortNo=3, SysDefined=1, TenantId=1,ParentId="0"  },
            };
        }


        public void Create()
        {
            foreach (var c in _categorys)
            {
                if (_context.Categorys.Any(a => a.CategoryName == c.CategoryName && a.CategoryType == c.CategoryType) == false)
                {
                    _context.Categorys.Add(c);
                }
            }
            _context.SaveChanges();
        }
    }
}
