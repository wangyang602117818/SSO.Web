namespace SSO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNativation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Navigations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        Url = c.String(maxLength: 512),
                        IconUrl = c.String(maxLength: 512),
                        UpdateTime = c.DateTime(),
                        CreateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserBasics", new[] { "UserId" });
            DropIndex("dbo.Roles", new[] { "Name" });
            DropIndex("dbo.Departments", new[] { "Code" });
            DropIndex("dbo.Companies", new[] { "Code" });
            DropTable("dbo.UserRoleMappings");
            DropTable("dbo.UserDepartmentMappings");
            DropTable("dbo.UserBasics");
            DropTable("dbo.Roles");
            DropTable("dbo.Navigations");
            DropTable("dbo.Departments");
            DropTable("dbo.Companies");
        }
    }
}
