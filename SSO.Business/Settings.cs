using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Business
{
    public class Settings : ModelBase<Data.Models.Settings>
    {
        public Settings() : base(new Data.Models.Settings()) { }
        public int UpdateLang(string userId, string lang)
        {
            var setting = instance.GetByUserId(userId);
            if (setting == null)
            {
                return instance.Insert(new Data.Models.Settings()
                {
                    UserId = userId,
                    Lang = lang,
                    CreateTime = DateTime.Now
                });
            }
            else
            {
                return instance.Update(
                    new Data.Models.Settings()
                    {
                        UserId = userId,
                        Lang = lang,
                        UpdateTime = DateTime.Now
                    });
            }
        }
        public Data.Models.Settings GetSetting(string userId)
        {
            return instance.GetByUserId(userId);
        }
    }
}
