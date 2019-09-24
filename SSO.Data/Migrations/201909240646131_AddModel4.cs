namespace SSO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModel4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserBasics", "PassWord", c => c.String(maxLength: 64));
            AlterColumn("dbo.UserBasics", "CompanyCode", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserBasics", "CompanyCode", c => c.String(maxLength: 64));
            AlterColumn("dbo.UserBasics", "PassWord", c => c.String(maxLength: 50));
        }
    }
}
