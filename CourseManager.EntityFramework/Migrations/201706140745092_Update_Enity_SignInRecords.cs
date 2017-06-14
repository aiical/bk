namespace CourseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Enity_SignInRecords : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SignInRecords", "CourseAddressType", c => c.String(nullable: false, maxLength: 36));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SignInRecords", "CourseAddressType");
        }
    }
}
