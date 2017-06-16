namespace CourseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Entity1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TeacherCourseArranges", "Remark", c => c.String(nullable: false, maxLength: 1024));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TeacherCourseArranges", "Remark", c => c.String(maxLength: 1024));
        }
    }
}
