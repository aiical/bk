namespace CourseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Entity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SignInRecords",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TeacherId = c.String(nullable: false, maxLength: 36),
                        StudentId = c.String(nullable: false, maxLength: 36),
                        Type = c.String(nullable: false, maxLength: 36),
                        BeginTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatorUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        TenantId = c.Int(nullable: false),
                        Reason = c.String(maxLength: 1024),
                        Remark = c.String(maxLength: 1024),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TeacherCourseArranges",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TeacherId = c.String(nullable: false, maxLength: 36),
                        Type = c.String(nullable: false, maxLength: 36),
                        ArrangeTime = c.DateTime(nullable: false),
                        BeginTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        StudentId = c.String(nullable: false, maxLength: 36),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatorUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        TenantId = c.Int(nullable: false),
                        Remark = c.String(maxLength: 1024),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TeacherCourseArranges");
            DropTable("dbo.SignInRecords");
        }
    }
}
