namespace CourseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Entity_SignInRecord1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SignInRecords", "ClassType", c => c.String());
            AddColumn("dbo.SignInRecords", "CourseType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SignInRecords", "CourseType");
            DropColumn("dbo.SignInRecords", "ClassType");
        }
    }
}
