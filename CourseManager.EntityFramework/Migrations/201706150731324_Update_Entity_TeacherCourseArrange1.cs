namespace CourseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Entity_TeacherCourseArrange1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeacherCourseArranges", "CourseAddressType", c => c.String(nullable: false, maxLength: 36));
            AddColumn("dbo.TeacherCourseArranges", "Address", c => c.String(nullable: false));
            AlterColumn("dbo.TeacherCourseArranges", "TeacherId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TeacherCourseArranges", "TeacherId", c => c.String(nullable: false, maxLength: 36));
            DropColumn("dbo.TeacherCourseArranges", "Address");
            DropColumn("dbo.TeacherCourseArranges", "CourseAddressType");
        }
    }
}
