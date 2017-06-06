using CourseManager.EntityFramework;
using EntityFramework.DynamicFilters;

namespace CourseManager.Migrations.SeedData
{
    public class InitialHostDbBuilder
    {
        private readonly CourseManagerDbContext _context;

        public InitialHostDbBuilder(CourseManagerDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new DefaultEditionsCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
        }
    }
}
