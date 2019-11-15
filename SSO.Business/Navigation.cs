using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace SSO.Business
{
    public class Navigation : ModelBase
    {
        public int Insert(string title, string baseUrl)
        {
            userCenterContext.Navigations.Add(new Data.Models.Navigation()
            {
                Title = title,
                BaseUrl = baseUrl,
                UpdateTime = DateTime.Now,
                CreateTime = DateTime.Now
            });
            return userCenterContext.SaveChanges();
        }
        public int Update(int id, string title, string baseUrl)
        {
            Data.Models.Navigation navigation = GetById(id);
            navigation.Title = title;
            navigation.BaseUrl = baseUrl;
            navigation.UpdateTime = DateTime.Now;
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
        public IEnumerable<Data.Models.Navigation> GetAll()
        {
            var query = from navigation in userCenterContext.Navigations select navigation;
            return query.OrderBy(o => o.Title).ToList();
        }
        public Data.Models.Navigation GetBaseUrl(string url)
        {
            var query = from navigation in userCenterContext.Navigations where url.Contains(navigation.BaseUrl) select navigation;
            return query.FirstOrDefault();
        }
    }
}
