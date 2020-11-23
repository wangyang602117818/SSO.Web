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
        public int Count()
        {
            return instance.Count();
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
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <param name="t">需要过滤的参数</param>
        /// <returns></returns>
        public List<T> GetAll(object obj)
        {
            return instance.GetAll<T>(obj);
        }
        public List<T> GetPageList(ref int count, T t, object replacement = null)
        {
            return instance.GetPageList<T>(ref count, t, replacement);
        }
    }
}
