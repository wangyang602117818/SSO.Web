namespace SSO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Log3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Logs", "UserId", c => c.String(maxLength: 30));
            AlterColumn("dbo.Logs", "UserName", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Logs", "UserName", c => c.String());
            AlterColumn("dbo.Logs", "UserId", c => c.String());
        }
    }
}
