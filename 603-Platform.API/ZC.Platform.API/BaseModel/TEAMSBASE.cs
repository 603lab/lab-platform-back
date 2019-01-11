using MySqlSugar;
using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.API.BaseModel
{
    [SugarMapping(TableName = "T_TEAMS")]
    public class TEAMSBASE
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int ID {get;set;}

        /// <summary>
        /// Desc:团队名称 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string teamName {get;set;}

        /// <summary>
        /// Desc:团队类型 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int teamType {get;set;}

        /// <summary>
        /// Desc:团队简介 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string teamRemark {get;set;}

        /// <summary>
        /// Desc:团队领导人编码 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string teamLeaderCode {get;set;}

        /// <summary>
        /// Desc:团队领导者名称 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string teamLeaderName {get;set;}

        /// <summary>
        /// Desc:创建人名称 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string createUserName {get;set;}

        /// <summary>
        /// Desc:创建人编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string createUserCode {get;set;}

        /// <summary>
        /// Desc:创建时间 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public DateTime createTime {get;set;}

    }
}
