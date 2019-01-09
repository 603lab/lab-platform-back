using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.Model
{
    public class T_DOC_COMMETS
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int ID {get;set;}

        /// <summary>
        /// Desc:文档编号 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? doc_id {get;set;}

        /// <summary>
        /// Desc:是否是回答 1是 0否 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int is_reply {get;set;}

        /// <summary>
        /// Desc:父级编号 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? parent_code {get;set;}

        /// <summary>
        /// Desc:内容 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string content {get;set;}

        /// <summary>
        /// Desc:回复用户编号 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? reply_user_code {get;set;}

        /// <summary>
        /// Desc:回复的用户名称 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string reply_user_name {get;set;}

        /// <summary>
        /// Desc:创建用户编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string create_user_code {get;set;}

        /// <summary>
        /// Desc:创建用户姓名 
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
