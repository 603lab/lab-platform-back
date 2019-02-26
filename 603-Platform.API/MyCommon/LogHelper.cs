using MySugar;
using MySqlSugar;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static MyCommon.EnumCommon;

namespace MyCommon
{
   public class LogHelper
    {
        public static string LogDetails(string sql, object Req, object Res)
        {
            string details = "SQL:" + sql;
            //组合报文
            details += "ReqJson:" + JsonConvert.SerializeObject(Req);
            details += "Res:Json" + JsonConvert.SerializeObject(Res);
            return details;
        }

    }
}
