namespace CourseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Entity_SignInRecord2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SignInRecords", "Duration", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SignInRecords", "Duration");
        }
    }
}
