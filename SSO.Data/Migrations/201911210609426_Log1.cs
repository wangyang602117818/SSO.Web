namespace SSO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Log1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "RecordId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "RecordId");
        }
    }
}
