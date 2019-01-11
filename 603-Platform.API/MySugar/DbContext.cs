using MySqlSugar;
using System;
using System.Collections.Generic;

namespace MySugar
{
    public class DbContext
    {
        //禁止实例化
        private DbContext()
        {

        }
        public static string ConnectionString { get; set; }
        public static string logContent { get; set; } = string.Empty;

        /// <summary>
        /// 无日志型
        /// </summary>
        /// <returns></returns>
        public static SqlSugarClient GetInstance()
        {
            string logContent = string.Empty;
            var db = new SqlSugarClient(ConnectionString);
            return db;
        }

        /// <summary>
        /// 通过表名直接处理 无需加特性
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static SqlSugarClient GetInstance(string tableName)
        {
            string logContent = string.Empty;
            var db = new SqlSugarClient(ConnectionString);

            #region 设置表转化

            List<KeyValue> kvList = new List<KeyValue>();
            //注意：只有启动属性映射才可以使用SetMappingColumns
            db.IsEnableAttributeMapping = true;

            var columsList = db.ClassGenerating.GetTableColumns(db, tableName);
            foreach (var item in columsList)
            {
                kvList.Add(GetNewKV(item.COLUMN_NAME.ToString(), item.COLUMN_NAME.ToString()));
            }
            db.SetMappingColumns(kvList);
            //db.AddMappingColumn()
            #endregion

            return db;
        }

        /// <summary>
        /// 记录日志型
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public static SqlSugarClient GetInstance(ref string log)
        {
            var db = new SqlSugarClient(ConnectionString);
            db.IsEnableLogEvent = true;//启用日志事件
            db.LogEventStarting = SugarConfigs.LogEventStarting;
            db.LogEventCompleted = SugarConfigs.LogEventCompleted;
            log = logContent;
            return db;
        }

        public class SugarConfigs
        {
            public static Action<string, string> LogEventStarting = (sql, pars) =>
            {
                logContent = $"starting: {sql} {pars}";

                using (var db = DbContext.GetInstance())
                {
                    //日志记录件事件里面用到数据库操作 IsEnableLogEvent一定要为false否则将引起死循环，并且要新开一个数据实例 像我这样写就没问题。
                    db.IsEnableLogEvent = false;
                    //db.ExecuteCommand("select 1");
                }
            };
            public static Action<string, string> LogEventCompleted = (sql, pars) =>
            {
                logContent += $"compeleting: {sql} {pars}";
            };
        }

        public static KeyValue GetNewKV(string Key, string Value)
        {
            Key = NewKey(Key);
            KeyValue kv = new KeyValue();
            kv.Key = Key;
            kv.Value = Value;
            return kv;
        }

        /// <summary>
        /// 获取驼峰式的命名
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string NewKey(string key)
        {
            int index = key.IndexOf('_');
            if (index != -1)
            {
                string ch = key[index + 1].ToString().ToUpper();

                key = key.Remove(index, 2);
                key = key.Insert(index, ch);
                while (key.IndexOf('_') != -1)
                {
                    key = NewKey(key);
                }
            }

            return key;
        }


    }
    /// <summary>
    /// 全局配置别名列（不区分表）
    /// </summary>

}
