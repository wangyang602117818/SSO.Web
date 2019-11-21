namespace SSO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Log : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        From = c.String(nullable: false, maxLength: 30),
                        Type = c.Int(nullable: false),
                        Content = c.String(nullable: false, maxLength: 50),
                        Remark = c.String(maxLength: 512),
                        UserId = c.String(),
                        UserName = c.String(),
                        UserHost = c.String(),
                        UserAgent = c.String(),
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
            DropTable("dbo.Logs");
            DropTable("dbo.Departments");
            DropTable("dbo.Companies");
        }
    }
}
