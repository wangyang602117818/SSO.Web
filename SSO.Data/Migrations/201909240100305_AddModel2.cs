namespace SSO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModel2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Companies", "UpdateTime", c => c.DateTime());
            AlterColumn("dbo.Companies", "CreateTime", c => c.DateTime());
            AlterColumn("dbo.Departments", "UpdateTime", c => c.DateTime());
            AlterColumn("dbo.Departments", "CreateTime", c => c.DateTime());
            AlterColumn("dbo.Roles", "UpdateTime", c => c.DateTime());
            AlterColumn("dbo.Roles", "CreateTime", c => c.DateTime());
            AlterColumn("dbo.UserBasics", "DeleteTime", c => c.DateTime());
            AlterColumn("dbo.UserBasics", "UpdateTime", c => c.DateTime());
            AlterColumn("dbo.UserBasics", "CreateTime", c => c.DateTime());
            AlterColumn("dbo.UserDepartmentMappings", "UpdateTime", c => c.DateTime());
            AlterColumn("dbo.UserDepartmentMappings", "CreateTime", c => c.DateTime());
            AlterColumn("dbo.UserRoleMappings", "UpdateTime", c => c.DateTime());
            AlterColumn("dbo.UserRoleMappings", "CreateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserRoleMappings", "CreateTime", c => c.DateTime());
            AlterColumn("dbo.UserRoleMappings", "UpdateTime", c => c.DateTime());
            AlterColumn("dbo.UserDepartmentMappings", "CreateTime", c => c.DateTime());
            AlterColumn("dbo.UserDepartmentMappings", "UpdateTime", c => c.DateTime());
            AlterColumn("dbo.UserBasics", "CreateTime", c => c.DateTime());
            AlterColumn("dbo.UserBasics", "UpdateTime", c => c.DateTime());
            AlterColumn("dbo.UserBasics", "DeleteTime", c => c.DateTime());
            AlterColumn("dbo.Roles", "CreateTime", c => c.DateTime());
            AlterColumn("dbo.Roles", "UpdateTime", c => c.DateTime());
            AlterColumn("dbo.Departments", "CreateTime", c => c.DateTime());
            AlterColumn("dbo.Departments", "UpdateTime", c => c.DateTime());
            AlterColumn("dbo.Companies", "CreateTime", c => c.DateTime());
            AlterColumn("dbo.Companies", "UpdateTime", c => c.DateTime());
        }
    }
}
