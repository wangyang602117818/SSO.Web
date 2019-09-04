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
                        ParentCode = c.String(nullable: false, maxLength: 30),
                        CompanyCode = c.String(nullable: false, maxLength: 30),
                        UpdateTime = c.DateTime(),
                        CreateTime = c.DateTime(),
                        Company_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.Company_Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Company_Id);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Departments", "Company_Id", "dbo.Companies");
            DropIndex("dbo.Roles", new[] { "Name" });
            DropIndex("dbo.Departments", new[] { "Company_Id" });
            DropIndex("dbo.Departments", new[] { "Code" });
            DropIndex("dbo.Companies", new[] { "Code" });
            DropTable("dbo.Roles");
            DropTable("dbo.Departments");
            DropTable("dbo.Companies");
        }
    }
}
