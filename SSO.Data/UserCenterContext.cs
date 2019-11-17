using SSO.Data.Models;
using SSO.Util;
using System.Data.Entity;

namespace SSO.Data
{
    public class UserCenterContext : DbContext
    {
        public static string databaseKey = AppSettings.databaseKey;
        public UserCenterContext() : base("name=" + databaseKey)
        {
            if (!Database.Exists())
            {
                Database.SetInitializer(new UserCenterInitData()); 
            }
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<UserBasic> UserBasics { get; set; }
        public DbSet<UserDepartmentMapping> UserDepartmentMappings { get; set; }
        public DbSet<UserRoleMapping> UserRoleMappings { get; set; }
        public DbSet<Navigation> Navigations { get; set; }

    }

}
