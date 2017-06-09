namespace CourseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Entity_TeacherCourseArrange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeacherCourseArranges", "ClassType", c => c.String(nullable: false, maxLength: 36));
            AddColumn("dbo.TeacherCourseArranges", "CourseType", c => c.String(nullable: false, maxLength: 36));
            DropColumn("dbo.TeacherCourseArranges", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TeacherCourseArranges", "Type", c => c.String(nullable: false, maxLength: 36));
            DropColumn("dbo.TeacherCourseArranges", "CourseType");
            DropColumn("dbo.TeacherCourseArranges", "ClassType");
        }
    }
}
