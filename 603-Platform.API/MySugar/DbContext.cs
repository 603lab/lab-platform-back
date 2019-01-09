using MySqlSugar;
using System;

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

    }
}
