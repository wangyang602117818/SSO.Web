namespace SSO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModel3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserBasics", "CompanyCode", c => c.String(maxLength: 64));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserBasics", "CompanyCode", c => c.String(maxLength: 30));
        }
    }
}
