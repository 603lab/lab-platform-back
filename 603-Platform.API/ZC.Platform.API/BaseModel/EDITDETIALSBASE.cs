using MySqlSugar;
using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.API.BaseModel
{
    [SugarMapping(TableName = "T_EDIT_DETIALS")]
    public class EDITDETIALSBASE
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int ID {get;set;}

        /// <summary>
        /// Desc:关联编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int relationCode {get;set;}

        /// <summary>
        /// Desc:修改报文 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string detail {get;set;}

        /// <summary>
        /// Desc:日志相关编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int logId {get;set;}

        /// <summary>
        /// Desc:创建者编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string createUserCode {get;set;}

        /// <summary>
        /// Desc:创建者姓名 
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
