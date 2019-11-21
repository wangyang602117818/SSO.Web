namespace SSO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Log2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Logs", "RecordId", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Logs", "RecordId", c => c.Int(nullable: false));
        }
    }
}
