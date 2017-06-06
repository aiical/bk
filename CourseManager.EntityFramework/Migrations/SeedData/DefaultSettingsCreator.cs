using System.Linq;
using Abp.Configuration;
using Abp.Localization;
using Abp.Net.Mail;
using CourseManager.EntityFramework;

namespace CourseManager.Migrations.SeedData
{
    public class DefaultSettingsCreator
    {
        private readonly CourseManagerDbContext _context;

        public DefaultSettingsCreator(CourseManagerDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            //Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "835277164@qq.com");
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "来自jery的邮件提醒");

            //Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "zh-CN");//en 
        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }
    }
}