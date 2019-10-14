using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace SSO.Business
{
    public class Navigation : ModelBase
    {
        public int Insert(string title, string url, string iconUrl)
        {
            userCenterContext.Navigations.Add(new Data.Models.Navigation()
            {
                Title = title,
                Url = url,
                IconUrl = iconUrl
            });
            return userCenterContext.SaveChanges();
        }
        public int Update(int id, string title, string url, string iconUrl)
        {
            Data.Models.Navigation navigation = GetById(id);
            navigation.Title = title;
            navigation.Url = url;
            navigation.IconUrl = iconUrl;
            return userCenterContext.SaveChanges();
        }
        public int Delete(IEnumerable<int> ids)
        {
            foreach (int id in ids)
            {
                Data.Models.Navigation navigation = new Data.Models.Navigation() { Id = id };
                DbEntityEntry<Data.Models.Navigation> entry = userCenterContext.Entry<Data.Models.Navigation>(navigation);
                entry.State = EntityState.Deleted;
            }
            return userCenterContext.SaveChanges();
        }
        public Data.Models.Navigation GetById(int id)
        {
            return userCenterContext.Navigations.Where(r => r.Id == id).FirstOrDefault();
        }
        public IEnumerable<Data.Models.Navigation> GetList(ref int count, string keyword = "", int pageIndex = 1, int pageSize = 15)
        {
            var query = from navigation in userCenterContext.Navigations select navigation;
            if (!string.IsNullOrEmpty(keyword)) query = query.Where(w => w.Title.Contains(keyword));
            count = query.Count();
            return query.OrderByDescending(o => o.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
