using System;

namespace SSO.Business
{
    public class Role : ModelBase
    {
        public int Insert(string name, string description)
        {
            userCenterContext.Roles.Add(new Data.Role()
            {
                Name = name,
                Description = description,
                UpdateTime = DateTime.Now,
                CreateTime = DateTime.Now
            });
            return userCenterContext.SaveChanges();
        }
    }
}
