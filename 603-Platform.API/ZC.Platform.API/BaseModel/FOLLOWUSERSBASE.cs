using MySqlSugar;
using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.API.BaseModel
{
    [SugarMapping(TableName = "T_FOLLOW_USERS")]
    public class FOLLOWUSERSBASE
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int ID {get;set;}

        /// <summary>
        /// Desc:关注者编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string followUserCode {get;set;}

        /// <summary>
        /// Desc:关注者名称 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string followUserName {get;set;}

        /// <summary>
        /// Desc:创建者编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string createUserCode {get;set;}

        /// <summary>
        /// Desc:创建者名称 
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
