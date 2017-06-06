namespace CourseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Entity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FolderId = c.String(maxLength: 36),
                        FileName = c.String(maxLength: 128),
                        Description = c.String(maxLength: 128),
                        CategoryType = c.String(maxLength: 32),
                        Url = c.String(maxLength: 255),
                        OldUrl = c.String(maxLength: 255),
                        Click = c.Int(nullable: false),
                        SortNo = c.Int(nullable: false),
                        IsTop = c.Boolean(),
                        SetTopTime = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        UpdateTime = c.DateTime(),
                        CreatorUserId = c.Long(),
                        FileSize = c.Int(nullable: false),
                        RelateId = c.String(maxLength: 36),
                        TenantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CnName = c.String(nullable: false, maxLength: 36),
                        EnName = c.String(maxLength: 128),
                        Mobile = c.String(maxLength: 36),
                        Sex = c.Int(nullable: false),
                        Age = c.Int(nullable: false),
                        Position = c.String(maxLength: 36),
                        LocalCountryName = c.String(maxLength: 128),
                        TenantId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Extend1 = c.String(maxLength: 512),
                        Extend2 = c.String(maxLength: 512),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Students");
            DropTable("dbo.Files");
        }
    }
}
