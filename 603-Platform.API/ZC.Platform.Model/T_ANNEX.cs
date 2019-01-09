using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.Model
{
    public class T_ANNEX
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int ID {get;set;}

        /// <summary>
        /// Desc:父级编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int parent_id {get;set;}

        /// <summary>
        /// Desc:文件地址 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string file_address {get;set;}

        /// <summary>
        /// Desc:文件名称 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string file_name {get;set;}

        /// <summary>
        /// Desc:文件标签 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string file_tag {get;set;}

        /// <summary>
        /// Desc:文件路由地址 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string url {get;set;}

        /// <summary>
        /// Desc:文件内容 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string content {get;set;}

        /// <summary>
        /// Desc:喜欢者数量 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int like_num {get;set;}

        /// <summary>
        /// Desc:浏览数 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public Double browse_num {get;set;}

        /// <summary>
        /// Desc:评论数 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public Double comment_num {get;set;}

        /// <summary>
        /// Desc:文件类型 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string type {get;set;}

        /// <summary>
        /// Desc:文件备注 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string remark {get;set;}

        /// <summary>
        /// Desc:创建者编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string create_user_code {get;set;}

        /// <summary>
        /// Desc:创建者名称 
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
