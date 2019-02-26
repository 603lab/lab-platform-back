using MySqlSugar;
using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.API.BaseModel
{
    [SugarMapping(TableName = "T_USER_SKILL")]
    public class USERSKILLBASE
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int ID {get;set;}

        /// <summary>
        /// Desc:所占比例 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int percent {get;set;}

        /// <summary>
        /// Desc:技能 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string item {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string createUserCode {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string createUserName {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public DateTime createTime {get;set;}

    }
}
