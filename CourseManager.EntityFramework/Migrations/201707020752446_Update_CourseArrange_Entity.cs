namespace CourseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_CourseArrange_Entity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StudentCourseArranges", "StudentId", c => c.String(nullable: false));
            AlterColumn("dbo.TeacherCourseArranges", "StudentId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TeacherCourseArranges", "StudentId", c => c.String(nullable: false, maxLength: 36));
            AlterColumn("dbo.StudentCourseArranges", "StudentId", c => c.String(nullable: false, maxLength: 36));
        }
    }
}
