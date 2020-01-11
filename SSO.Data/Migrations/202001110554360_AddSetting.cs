namespace SSO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSetting : DbMigration
    {
        public override void Up()
        {

            CreateTable(
                "dbo.Settings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.String(nullable: false, maxLength: 50),
                    Lang = c.String(nullable: false, maxLength: 8),
                    UpdateTime = c.DateTime(),
                    CreateTime = c.DateTime(),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserId, unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserBasics", new[] { "UserId" });
            DropIndex("dbo.Settings", new[] { "UserId" });
            DropIndex("dbo.Roles", new[] { "Name" });
            DropIndex("dbo.Departments", new[] { "Code" });
            DropIndex("dbo.Companies", new[] { "Code" });
            DropTable("dbo.UserRoleMappings");
            DropTable("dbo.UserDepartmentMappings");
            DropTable("dbo.UserBasics");
            DropTable("dbo.Settings");
            DropTable("dbo.Roles");
            DropTable("dbo.Navigations");
            DropTable("dbo.Logs");
            DropTable("dbo.Departments");
            DropTable("dbo.Companies");
        }
    }
}
