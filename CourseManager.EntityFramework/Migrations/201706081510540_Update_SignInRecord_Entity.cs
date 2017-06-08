namespace CourseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_SignInRecord_Entity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SignInRecords", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SignInRecords", "Address");
        }
    }
}
