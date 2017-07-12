namespace CourseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Entity_TeacherCourseArrange_AddField_StudentName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeacherCourseArranges", "StudentName", c => c.String(nullable: false, maxLength: 64));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TeacherCourseArranges", "StudentName");
        }
    }
}
