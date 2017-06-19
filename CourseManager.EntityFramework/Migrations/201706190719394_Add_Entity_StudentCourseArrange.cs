namespace CourseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Entity_StudentCourseArrange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudentCourseArranges",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ClassType = c.String(nullable: false, maxLength: 36),
                        CourseType = c.String(nullable: false, maxLength: 36),
                        CourseAddressType = c.String(nullable: false, maxLength: 36),
                        Address = c.String(nullable: false),
                        ArrangeTime = c.DateTime(),
                        BeginTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        StudentId = c.String(nullable: false, maxLength: 36),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatorUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        TenantId = c.Int(nullable: false),
                        Remark = c.String(nullable: false, maxLength: 1024),
                        CourseStatus = c.String(nullable: false, maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StudentCourseArranges");
        }
    }
}
