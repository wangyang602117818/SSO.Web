namespace SSO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Log4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Logs", "From", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Logs", "From", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
