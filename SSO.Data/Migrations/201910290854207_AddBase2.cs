namespace SSO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBase2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Navigations", "LoginPath");
            DropColumn("dbo.Navigations", "LogOutPath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Navigations", "LogOutPath", c => c.String(maxLength: 50));
            AddColumn("dbo.Navigations", "LoginPath", c => c.String(maxLength: 50));
        }
    }
}
