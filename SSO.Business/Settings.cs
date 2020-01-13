using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Business
{
    public class Settings : ModelBase
    {
        public int UpdateLang(string userId, string lang)
        {
            var setting = userCenterContext.Settings.Where(r => r.UserId == userId).FirstOrDefault();
            if (setting == null)
            {
                userCenterContext.Settings.Add(new Data.Models.Settings()
                {
                    UserId = userId,
                    Lang = lang,
                    UpdateTime = DateTime.Now,
                    CreateTime = DateTime.Now
                });
            }
            else
            {
                setting.Lang = lang;
                setting.UpdateTime = DateTime.Now;
            }
            return userCenterContext.SaveChanges();
        }
        public Data.Models.Settings GetSetting(string userId)
        {
            return userCenterContext.Settings.Where(r => r.UserId == userId).FirstOrDefault();
        }
    }
}
