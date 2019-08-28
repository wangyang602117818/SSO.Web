namespace SSO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRoleName : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Roles", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Roles", new[] { "Name" });
        }
    }
}
