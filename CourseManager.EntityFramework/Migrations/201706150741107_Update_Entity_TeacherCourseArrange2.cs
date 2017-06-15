namespace CourseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Entity_TeacherCourseArrange2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TeacherCourseArranges", "ArrangeTime", c => c.DateTime());
            AlterColumn("dbo.TeacherCourseArranges", "BeginTime", c => c.DateTime());
            AlterColumn("dbo.TeacherCourseArranges", "EndTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TeacherCourseArranges", "EndTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TeacherCourseArranges", "BeginTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TeacherCourseArranges", "ArrangeTime", c => c.DateTime(nullable: false));
        }
    }
}
