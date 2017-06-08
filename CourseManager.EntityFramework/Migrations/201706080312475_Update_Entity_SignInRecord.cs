namespace CourseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Entity_SignInRecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SignInRecords", "UnNormalType", c => c.String(maxLength: 36));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SignInRecords", "UnNormalType");
        }
    }
}
