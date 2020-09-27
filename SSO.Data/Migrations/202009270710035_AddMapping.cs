namespace SSO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMapping : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PermissionRoleMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Role = c.String(maxLength: 50),
                        Permission = c.String(maxLength: 50),
                        UpdateTime = c.DateTime(),
                        CreateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PermissionUserMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 50),
                        Permission = c.String(maxLength: 50),
                        UpdateTime = c.DateTime(),
                        CreateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PermissionUserMappings");
            DropTable("dbo.PermissionRoleMappings");
        }
    }
}
