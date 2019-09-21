namespace SSO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModel1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserBasics", "DeleteTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserBasics", "DeleteTime");
        }
    }
}
