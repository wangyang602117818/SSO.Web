using Newtonsoft.Json;
using SSO.Model;
using SSO.Util.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SSO.Data
{
    public abstract class SqlBase
    {
        private DateTime? createTime = DateTime.Now;
        public int Id { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? CreateTime { get => createTime; set => createTime = value; }
        [JsonIgnore]
        public int PageIndex { get; set; }
        [JsonIgnore]
        public int PageSize { get; set; }
        static Dictionary<string, XDocument> configFiles = new Dictionary<string, XDocument>();
        protected XDocument sqlXml = null;
        protected SqlHelper sqlHelper = new SqlHelper();
        static SqlBase()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var configs = assembly.GetManifestResourceNames();
            foreach (var config in configs)
            {
                Stream stream = assembly.GetManifestResourceStream(config);
                XDocument xDocument = XDocument.Load(stream);
                configFiles.Add(config.Replace(assembly.GetName().Name + ".", ""), xDocument);
                if (config.EndsWith("create.sql.xml"))
                {
                    CreateTableNotExists(xDocument.Root.Value);
                }
            }
        }
        static void CreateTableNotExists(string sql)
        {
            new SqlHelper().ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 默认获取与类名称相同的配置文件
        /// </summary>
        public SqlBase()
        {
            string name = this.GetType().Name.ToLower() + ".sql.xml";
            foreach (var item in configFiles)
            {
                if (item.Key.ToLower().EndsWith(name)) sqlXml = item.Value;
            }
        }
        /// <summary>
        /// config:配置文件名称,自动在 Embedded Resource中查找
        /// </summary>
        /// <param name="config"></param>
        public SqlBase(string config)
        {
            foreach (var item in configFiles)
            {
                if (item.Key.EndsWith(config)) sqlXml = item.Value;
            }
        }
        public int Count()
        {
            return (int)ExecuteScalar("count", null);
        }
        public int Insert<T>(T t) where T : SqlBase
        {
            return ExecuteNonQuery("insert", t);
        }
        public int Update<T>(T t) where T : SqlBase
        {
            return ExecuteNonQuery("update", t);
        }
        public int Delete(IEnumerable<int> ids)
        {
            return ExecuteNonQuery("delete", new { Ids = ids });
        }
        public T GetById<T>(int id) where T : class
        {
            int count = 0;
            return QueryObject<T>("get-by-id", new { Id = id }, ref count);
        }
        public List<T> GetAll<T>(T t) where T : class
        {
            int count = 0;
            return QueryList<T>("get-all", t, ref count);
        }
        public List<T> GetPageList<T>(ref int count, T t, object replacement) where T : class
        {
            return QueryList<T>("get-page-list", t, ref count, replacement);
        }

        protected object ExecuteScalar(string nodeName, object paras, object replacement = null)
        {
            string sql = ParseStatements(nodeName, paras, replacement);
            SqlParameter[] sqlParameters = ParseSqlParameter(paras);
            return sqlHelper.ExecuteScalar(sql, sqlParameters);
        }
        protected int ExecuteNonQuery(string nodeName, object paras, object replacement = null)
        {
            string sql = ParseStatements(nodeName, paras, replacement);
            SqlParameter[] sqlParameters = ParseSqlParameter(paras);
            return sqlHelper.ExecuteNonQuery(sql, sqlParameters);
        }
        /// <summary>
        /// 执行事务,返回最后一个sql影响的行数
        /// </summary>
        /// <param name="nodeNames"></param>
        /// <param name="paras"></param>
        /// <param name="replacements"></param>
        /// <returns></returns>
        protected int ExecuteNonQueryTransaction(IEnumerable<string> nodeNames, IEnumerable<object> paras, IEnumerable<object> replacements = null)
        {
            List<string> sqls = new List<string>();
            List<SqlParameter[]> sqlParameters = new List<SqlParameter[]>();
            for (var i = 0; i < nodeNames.Count(); i++)
            {
                string sql = ParseStatements(nodeNames.ElementAt(i), paras.ElementAt(i), replacements == null ? null : replacements.ElementAt(i));
                SqlParameter[] parameters = ParseSqlParameter(paras.ElementAt(i));
                sqls.Add(sql);
                sqlParameters.Add(parameters);
            }
            return sqlHelper.ExecuteNonQueryTransaction(sqls, sqlParameters);
        }
        protected List<T> QueryList<T>(string nodeName, object paras, ref int count, object replacement = null) where T : class
        {
            var result = Execute(nodeName, paras, ref count, replacement);
            if (result == null) return new List<T>();
            return JsonSerializerHelper.Deserialize<List<T>>(result);
        }
        protected T QueryObject<T>(string nodeName, object paras, ref int count, object replacement = null) where T : class
        {
            var result = Execute(nodeName, paras, ref count, replacement);
            if (result == null) return null;
            return JsonSerializerHelper.Deserialize<List<T>>(result)[0];
        }

        /// <summary>
        /// 判断对象中某个属性是否可用(不为空 并且 不为null)
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool ElementNotEmpty(string propertyName, object obj)
        {
            if (obj == null) return false;
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                if (prop.Name == propertyName)
                {
                    var value = prop.GetValue(obj);
                    if (value == null) return false;
                    switch (prop.PropertyType.Name.ToLower())
                    {
                        case "string":
                            if ((string)value == "") return false;
                            break;
                    }
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 判断对象中某个属性是否可用(不为NULL)
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool ElementNotNull(string propertyName, object obj)
        {
            if (obj == null) return false;
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                if (prop.Name == propertyName)
                {
                    var value = prop.GetValue(obj);
                    if (value == null) return false;
                    return true;
                }
            }
            return false;
        }
        private string Execute(string nodeName, object paras, ref int count, object replacement)
        {
            string sql = ParseStatements(nodeName, paras, replacement);
            SqlParameter[] sqlParameters = ParseSqlParameter(paras);
            DataTable dt = sqlHelper.ExecuteDataTable(sql, sqlParameters);
            if (dt.Rows.Count == 0) return null;
            count = dt.Columns.Contains("total") ? (int)dt.Rows[0]["total"] : dt.Rows.Count;
            return JsonSerializerHelper.Serialize(dt);
        }
        /// <summary>
        /// 解析XElement里面的sql语句
        /// </summary>
        /// <param name="nodeName">要获取的节点名称</param>
        /// <param name="paras">要防止注入的参数列表</param>
        /// <param name="replacement">要替换的对象</param>
        /// <returns></returns>
        protected string ParseStatements(string nodeName, object paras, object replacement)
        {
            string sql = "";
            var nodes = sqlXml.Root.Element(nodeName).Nodes();
            foreach (var node in nodes)
            {
                if (node.NodeType == XmlNodeType.Text) sql += node;
                if (node.NodeType == XmlNodeType.CDATA) sql += ((XText)node).Value;
                if (node.NodeType == XmlNodeType.Element)
                {
                    var xNode = (XElement)node;
                    if (xNode.Name == "isNotEmpty")
                    {
                        var propertyName = xNode.Attribute("property").Value;
                        var statement = xNode.Value;
                        var prepend = xNode.Attribute("prepend").Value;
                        if (ElementNotEmpty(propertyName, paras))
                        {
                            sql += prepend + statement;
                        }
                    }
                    if (xNode.Name == "isNotNull")
                    {
                        var propertyName = xNode.Attribute("property").Value;
                        var statement = xNode.Value;
                        var prepend = xNode.Attribute("prepend").Value;
                        if (ElementNotNull(propertyName, paras))
                        {
                            sql += prepend + statement;
                        }
                    }
                    if (xNode.Name == "iterate")
                    {
                        //迭代的属性名称
                        var propertyName = xNode.Attribute("property").Value;
                        //迭代的连接符
                        var conjunction = xNode.Attribute("conjunction").Value;
                        var props = paras.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        foreach (var prop in props)
                        {
                            if (prop.Name == propertyName)
                            {
                                var value = prop.GetValue(paras);
                                List<string> insql = new List<string>();
                                var countInt = value as List<int>;
                                if (countInt != null)
                                {
                                    for (var i = 0; i < countInt.Count; i++)
                                        insql.Add(xNode.Value.Replace("{{index}}", i.ToString()));
                                }
                                var countString = value as List<string>;
                                if (countString != null)
                                {
                                    for (var i = 0; i < countString.Count; i++)
                                        insql.Add(xNode.Value.Replace("{{index}}", i.ToString()));
                                }
                                sql += string.Join(conjunction, insql);
                            }
                        }
                    }
                }
            }
            return ReplaceStatements(sql, replacement);
        }
        private string ReplaceStatements(string sql, object replacement)
        {
            if (replacement == null) return sql;
            var props = replacement.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                var name = prop.Name;
                var value = prop.GetValue(replacement);
                if (value is string)
                {
                    sql = sql.Replace("#" + name + "#", (string)value);
                }
            }
            return sql;
        }
        /// <summary>
        /// 从对象中拼接 SqlParameter[] 数组
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected SqlParameter[] ParseSqlParameter(object obj)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            if (obj == null) return sqlParameters.ToArray();
            //获取对象的所有属性,拼接成SqlParameter[];
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                var name = prop.Name;
                var value = prop.GetValue(obj);
                if (value != null)
                {
                    if (value is IEnumerable<int>)
                    {
                        IEnumerable<int> values = value as IEnumerable<int>;
                        for (var i = 0; i < values.Count(); i++)
                        {
                            SqlParameter sqlParameter = new SqlParameter("@" + name + i, values.ToArray()[i]);
                            sqlParameters.Add(sqlParameter);
                        }
                    }
                    else if (value is IEnumerable<string>)
                    {
                        IEnumerable<string> values = value as IEnumerable<string>;
                        for (var i = 0; i < values.Count(); i++)
                        {
                            SqlParameter sqlParameter = new SqlParameter("@" + name + i, values.ToArray()[i]);
                            sqlParameters.Add(sqlParameter);
                        }
                    }
                    else
                    {
                        SqlParameter sqlParameter = new SqlParameter("@" + name, value);
                        sqlParameters.Add(sqlParameter);
                    }
                }
            }
            return sqlParameters.ToArray();
        }
    }
}
