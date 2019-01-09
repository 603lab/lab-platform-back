using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.Model
{
    public class T_LOG
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
        public string log_type {get;set;}

        /// <summary>
        /// Desc:操作类型 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string op_type {get;set;}

        /// <summary>
        /// Desc:操作名称 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string op_name {get;set;}

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
        public int? relation_code {get;set;}

        /// <summary>
        /// Desc:创建者编号 
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
