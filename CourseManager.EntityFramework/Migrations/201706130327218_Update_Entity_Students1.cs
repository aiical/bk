namespace CourseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Entity_Students1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "WeChat", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "WeChat");
        }
    }
}
