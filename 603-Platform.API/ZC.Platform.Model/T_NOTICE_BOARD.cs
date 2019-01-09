using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.Model
{
    public class T_NOTICE_BOARD
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int ID {get;set;}

        /// <summary>
        /// Desc:主标题 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string main_title {get;set;}

        /// <summary>
        /// Desc:副标题 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string sub_head {get;set;}

        /// <summary>
        /// Desc:内容 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string content {get;set;}

        /// <summary>
        /// Desc:格式类型 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? format_type {get;set;}

        /// <summary>
        /// Desc:内容类型 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? content_type {get;set;}

        /// <summary>
        /// Desc:是否置顶 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? is_top {get;set;}

        /// <summary>
        /// Desc:排列序号 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? sort {get;set;}

        /// <summary>
        /// Desc:创建人姓名 
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
