namespace SSO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Navigations", "BaseUrl", c => c.String(maxLength: 512));
            AddColumn("dbo.Navigations", "LogOutPath", c => c.String(maxLength: 50));
            DropColumn("dbo.Navigations", "Url");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Navigations", "Url", c => c.String(maxLength: 512));
            DropColumn("dbo.Navigations", "LogOutPath");
            DropColumn("dbo.Navigations", "BaseUrl");
        }
    }
}
