using MySqlSugar;
using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.API.BaseModel
{
    [SugarMapping(TableName = "T_DOC_COMMETS")]
    public class DOCCOMMETSBASE
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
        public int? docId {get;set;}

        /// <summary>
        /// Desc:是否是回答 1是 0否 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int isReply {get;set;}

        /// <summary>
        /// Desc:父级编号 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? parentCode {get;set;}

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
        public int? replyUserCode {get;set;}

        /// <summary>
        /// Desc:回复的用户名称 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string replyUserName {get;set;}

        /// <summary>
        /// Desc:创建用户编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string createUserCode {get;set;}

        /// <summary>
        /// Desc:创建用户姓名 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string createUserName {get;set;}

        /// <summary>
        /// Desc:创建时间 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public DateTime createTime {get;set;}

    }
}
