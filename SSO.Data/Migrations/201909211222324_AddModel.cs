namespace SSO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserBasics", "CompanyName", c => c.String());
            AddColumn("dbo.UserBasics", "DepartmentName", c => c.String());
            AddColumn("dbo.UserBasics", "RoleName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserBasics", "RoleName");
            DropColumn("dbo.UserBasics", "DepartmentName");
            DropColumn("dbo.UserBasics", "CompanyName");
        }
    }
}
