namespace CourseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Entity_Categorys : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categorys",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CategoryName = c.String(maxLength: 64),
                        DictionaryValue = c.String(maxLength: 64),
                        ParentId = c.String(maxLength: 36),
                        Level = c.Int(),
                        CategoryType = c.String(maxLength: 64),
                        SysDefined = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        SortNo = c.Int(),
                        CreatorUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        TenantId = c.Int(nullable: false),
                        Description = c.String(maxLength: 512),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Categorys");
        }
    }
}
