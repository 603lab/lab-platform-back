using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.Model
{
    public class T_TEAMS
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
        public string team_name {get;set;}

        /// <summary>
        /// Desc:团队类型 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string team_type {get;set;}

        /// <summary>
        /// Desc:团队简介 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string team_remark {get;set;}

        /// <summary>
        /// Desc:团队领导人编码 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string team_leader_code {get;set;}

        /// <summary>
        /// Desc:团队领导者名称 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string team_leader_name {get;set;}

        /// <summary>
        /// Desc:创建人名称 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string create_user_name {get;set;}

        /// <summary>
        /// Desc:创建人编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string create_user_code {get;set;}

        /// <summary>
        /// Desc:创建时间 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public DateTime create_time {get;set;}

    }
}
