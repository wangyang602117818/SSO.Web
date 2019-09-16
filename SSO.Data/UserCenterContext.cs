using SSO.Data.Models;
using System.Data.Entity;

namespace SSO.Data
{
    public class UserCenterContext : DbContext
    {
        public static string databaseKey = System.Configuration.ConfigurationManager.AppSettings["databaseKey"];
        public UserCenterContext() : base("name=" + databaseKey) { }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<Department> Departments { get; set; }

    }
}
