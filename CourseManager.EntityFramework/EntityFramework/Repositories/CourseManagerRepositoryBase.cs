using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace CourseManager.EntityFramework.Repositories
{
    public abstract class CourseManagerRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<CourseManagerDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected CourseManagerRepositoryBase(IDbContextProvider<CourseManagerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class CourseManagerRepositoryBase<TEntity> : CourseManagerRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected CourseManagerRepositoryBase(IDbContextProvider<CourseManagerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
