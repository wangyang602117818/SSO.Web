using SSO.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Business
{
    public class Log : ModelBase
    {
        public int Insert(string from, LogType type, IEnumerable<string> recordId, string content, string remark, string userId, string userName, string userHost, string userAgent)
        {
            foreach (string rId in recordId)
                userCenterContext.Logs.Add(new Data.Models.Log()
                {
                    From = from,
                    Type = type,
                    RecordId = rId,
                    Content = content,
                    Remark = remark,
                    UserId = userId,
                    UserName = userName,
                    UserHost = userHost,
                    UserAgent = userAgent,
                    CreateTime = DateTime.Now
                });
            return userCenterContext.SaveChanges();
        }
        public IEnumerable<Data.Models.Log> GetList(ref int count, string keyword = "", int pageIndex = 1, int pageSize = 15)
        {
            var query = from log in userCenterContext.Logs select log;
            if (!string.IsNullOrEmpty(keyword)) query = query.Where(w => w.Content == keyword);
            count = query.Count();
            return query.OrderByDescending(o => o.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
