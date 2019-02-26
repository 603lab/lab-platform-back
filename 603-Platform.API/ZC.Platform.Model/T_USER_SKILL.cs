using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.Model
{
    public class T_USER_SKILL
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
        public string create_user_code {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string create_user_name {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public DateTime create_time {get;set;}

    }
}
