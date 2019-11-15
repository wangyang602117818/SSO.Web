namespace SSO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Navigations", "IconUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Navigations", "IconUrl", c => c.String(maxLength: 512));
        }
    }
}
