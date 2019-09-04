namespace SSO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrder : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Departments", "ParentCode", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Departments", "ParentCode", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
