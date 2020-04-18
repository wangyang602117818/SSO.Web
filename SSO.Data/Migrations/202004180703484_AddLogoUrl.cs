namespace SSO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLogoUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Navigations", "LogoUrl", c => c.String(maxLength: 512));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Navigations", "LogoUrl");
        }
    }
}
