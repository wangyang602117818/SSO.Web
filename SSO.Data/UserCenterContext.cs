
using SSO.Model;
using System.Data.Entity;

namespace SSO.Data
{
    public class UserCenterContext : DbContext
    {
        public UserCenterContext() : base("UserCenter") { }
        public DbSet<Role> Roles { get; set; }
    }
}
