using MySugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZC.Platform.API.BaseModel;
using static MyCommon.EnumCommon;

namespace ZC.Platform.API
{
    public class LogWirter
    {
        public static bool Record(LogType logType, OpType opType, string title ,string opName,int relationCode, string createUserCode, string createUserName)
        {
            LOGBASE log = new LOGBASE();
            log.createTime = DateTime.Now;
            log.createUserCode = createUserCode;
            log.createUserName = createUserName;
            log.logType = logType.ToString();
            log.opType = opType.ToString();
            log.opName = opName;
            //设置关联项
            log.relationCode = relationCode;
            log.title = title;
            //是否设置回滚

            //合成最终日志
            log.remark = $"{createUserName} {opName} {title}";
            using (var db = DbContext.GetInstance("T_LOG"))
            {
                try
                {
                    db.Insert(log);
                }
                catch (Exception)
                {
                    return false;
                }
                
                
            }

            return true;
        }
    }
}
