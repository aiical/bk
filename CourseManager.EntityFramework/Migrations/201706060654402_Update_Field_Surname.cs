namespace CourseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Field_Surname : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AbpUsers", "Name", c => c.String(maxLength: 32));
            AlterColumn("dbo.AbpUsers", "EmailAddress", c => c.String(maxLength: 256));
            DropColumn("dbo.AbpUsers", "Surname");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AbpUsers", "Surname", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.AbpUsers", "EmailAddress", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.AbpUsers", "Name", c => c.String(nullable: false, maxLength: 32));
        }
    }
}
