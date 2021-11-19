using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Business
{
    public class ModelBase<T> where T : Data.ModelBase
    {
        protected T instance;
        public ModelBase(T instance)
        {
            this.instance = instance;
        }
        public int RecordCount()
        {
            return instance.RecordCount();
        }
        public int Delete(IEnumerable<int> ids)
        {
            int count = 0;
            try
            {
                count = instance.Delete(new { Ids = ids });
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("constraint"))
                    count = -1;
            }
            return count;
        }
        public T GetById(int id)
        {
            return instance.GetById<T>(id);
        }
        public int Update(object obj)
        {
            return instance.Update(obj);
        }
        public int Insert(object obj)
        {
            return instance.Insert(obj);
        }
        public int InsertIdentity(object obj)
        {
            return instance.InsertIdentity(obj);
        }
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <param name="obj">需要过滤的参数</param>
        /// <returns></returns>
        public IEnumerable<T> GetAll(object obj)
        {
            return instance.GetAll<T>(obj);
        }
        public IEnumerable<T> GetPageList<T>(object t, object replacement = null) where T : new()
        {
            return instance.GetPageList<T>(t, replacement);
        }
    }
}
