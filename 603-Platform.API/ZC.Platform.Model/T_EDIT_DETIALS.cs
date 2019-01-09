using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.Model
{
    public class T_EDIT_DETIALS
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
        public int relation_code {get;set;}

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
        public int log_id {get;set;}

        /// <summary>
        /// Desc:创建者编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string create_user_code {get;set;}

        /// <summary>
        /// Desc:创建者姓名 
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
