using MySqlSugar;
using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.API.BaseModel
{
    [SugarMapping(TableName = "T_TEAM_MEMBERS")]
    public class TEAMMEMBERSBASE
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int ID {get;set;}

        /// <summary>
        /// Desc:用户名称 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string userName {get;set;}

        /// <summary>
        /// Desc:用户编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string userCode {get;set;}

        /// <summary>
        /// Desc:团队编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int teamId {get;set;}

        /// <summary>
        /// Desc:是否为管理员 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int isAdmin {get;set;}

        /// <summary>
        /// Desc:创建人编号 
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
