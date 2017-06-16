namespace CourseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Entity_SignInRecords1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SignInRecords", "CourseArranges", c => c.String(nullable: false, maxLength: 1024));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SignInRecords", "CourseArranges");
        }
    }
}
