using MySqlSugar;
using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.API.BaseModel
{
    [SugarMapping(TableName = "T_LOG")]
    public class LOGBASE
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int ID {get;set;}

        /// <summary>
        /// Desc:日志类型 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string logType {get;set;}

        /// <summary>
        /// Desc:操作类型 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string opType {get;set;}

        /// <summary>
        /// Desc:操作名称 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string opName {get;set;}

        /// <summary>
        /// Desc:标题 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string title {get;set;}

        /// <summary>
        /// Desc:主要内容 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string remark {get;set;}

        /// <summary>
        /// Desc:关联编号 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? relationCode {get;set;}

        /// <summary>
        /// Desc:创建者编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string createUserCode {get;set;}

        /// <summary>
        /// Desc:创建人名称 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string createUserName {get;set;}

        /// <summary>
        /// Desc:创建时间 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public DateTime createTime {get;set;}

    }
}
