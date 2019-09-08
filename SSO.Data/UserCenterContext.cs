using SSO.Data.Models;
using System.Data.Entity;

namespace SSO.Data
{
    public class UserCenterContext : DbContext
    {
        public UserCenterContext() : base("name=pgsql") { }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<Department> Departments { get; set; }

    }
}
