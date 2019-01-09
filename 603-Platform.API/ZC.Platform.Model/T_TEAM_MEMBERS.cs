using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.Model
{
    public class T_TEAM_MEMBERS
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
        public string user_name {get;set;}

        /// <summary>
        /// Desc:用户编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string user_code {get;set;}

        /// <summary>
        /// Desc:团队编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int team_id {get;set;}

        /// <summary>
        /// Desc:是否为管理员 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int is_admin {get;set;}

        /// <summary>
        /// Desc:创建人编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string create_user_code {get;set;}

        /// <summary>
        /// Desc:创建人名称 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string create_user_name {get;set;}

        /// <summary>
        /// Desc:创建时间 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public DateTime create_time {get;set;}

    }
}
