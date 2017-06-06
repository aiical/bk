using System.Linq;
using CourseManager.EntityFramework;
using CourseManager.MultiTenancy;

namespace CourseManager.Migrations.SeedData
{
    public class DefaultTenantCreator
    {
        private readonly CourseManagerDbContext _context;

        public DefaultTenantCreator(CourseManagerDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateUserAndRoles();
        }

        private void CreateUserAndRoles()
        {
            //Default tenant

            var defaultTenant = _context.Tenants.FirstOrDefault(t => t.TenancyName == Tenant.DefaultTenantName);
            if (defaultTenant == null)
            {
                _context.Tenants.Add(new Tenant {TenancyName = Tenant.DefaultTenantName, Name = Tenant.DefaultTenantName});
                _context.SaveChanges();
            }
        }
    }
}
