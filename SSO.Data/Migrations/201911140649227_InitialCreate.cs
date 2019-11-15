namespace SSO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 30),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 512),
                        Order = c.Int(nullable: false),
                        UpdateTime = c.DateTime(),
                        CreateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 30),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 512),
                        Order = c.Int(nullable: false),
                        Layer = c.Int(nullable: false),
                        ParentCode = c.String(maxLength: 30),
                        CompanyCode = c.String(nullable: false, maxLength: 30),
                        UpdateTime = c.DateTime(),
                        CreateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true);
            
            CreateTable(
                "dbo.Navigations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        BaseUrl = c.String(maxLength: 512),
                        IconUrl = c.String(maxLength: 512),
                        UpdateTime = c.DateTime(),
                        CreateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 512),
                        UpdateTime = c.DateTime(),
                        CreateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.UserBasics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 50),
                        UserName = c.String(nullable: false, maxLength: 50),
                        PassWord = c.String(maxLength: 64),
                        CompanyCode = c.String(maxLength: 30),
                        Mobile = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        IdCard = c.String(maxLength: 20),
                        Sex = c.String(maxLength: 1),
                        IsModified = c.Boolean(nullable: false),
                        Delete = c.Boolean(nullable: false),
                        DeleteTime = c.DateTime(),
                        CompanyName = c.String(),
                        DepartmentName = c.String(),
                        RoleName = c.String(),
                        UpdateTime = c.DateTime(),
                        CreateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserId, unique: true);
            
            CreateTable(
                "dbo.UserDepartmentMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 50),
                        DepartmentCode = c.String(maxLength: 30),
                        UpdateTime = c.DateTime(),
                        CreateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRoleMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 50),
                        Role = c.String(maxLength: 30),
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
