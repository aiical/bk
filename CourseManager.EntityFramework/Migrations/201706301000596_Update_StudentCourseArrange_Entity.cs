namespace CourseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_StudentCourseArrange_Entity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentCourseArranges", "StudentName", c => c.String(maxLength: 36));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentCourseArranges", "StudentName");
        }
    }
}
