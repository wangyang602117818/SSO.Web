namespace SSO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBase1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Navigations", "LoginPath", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Navigations", "LoginPath");
        }
    }
}
