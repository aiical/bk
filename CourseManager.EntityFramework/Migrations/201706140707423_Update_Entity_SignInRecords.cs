namespace CourseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Entity_SignInRecords : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SignInRecords", "StudentId", c => c.String(nullable: false, maxLength: 1024));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SignInRecords", "StudentId", c => c.String(nullable: false, maxLength: 36));
        }
    }
}
