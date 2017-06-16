namespace CourseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Entity_TeacherCourseArrange3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeacherCourseArranges", "CourseStatus", c => c.String(nullable: false, maxLength: 64));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TeacherCourseArranges", "CourseStatus");
        }
    }
}
