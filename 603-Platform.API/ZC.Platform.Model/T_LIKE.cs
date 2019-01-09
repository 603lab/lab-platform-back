using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.Model
{
    public class T_LIKE
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int ID {get;set;}

        /// <summary>
        /// Desc:项目编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int item_id {get;set;}

        /// <summary>
        /// Desc:项目类型 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string type {get;set;}

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
