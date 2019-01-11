using MySqlSugar;
using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.API.BaseModel
{
    [SugarMapping(TableName = "T_NOTICE_BOARD")]
    public class NOTICEBOARDBASE
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
        public string mainTitle {get;set;}

        /// <summary>
        /// Desc:副标题 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string subHead {get;set;}

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
        public int? formatType {get;set;}

        /// <summary>
        /// Desc:内容类型 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? contentType {get;set;}

        /// <summary>
        /// Desc:是否置顶 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int isTop {get;set;}

        /// <summary>
        /// Desc:排列序号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int sort {get;set;}

        /// <summary>
        /// Desc:最近一次更新时间 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public DateTime lastEditTime {get;set;}

        /// <summary>
        /// Desc:创建人姓名 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string createUserName {get;set;}

        /// <summary>
        /// Desc:创建人编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string createUserCode {get;set;}

        /// <summary>
        /// Desc:创建时间 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public DateTime createTime {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string lastEditUserCode {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string lastEditUserName {get;set;}

    }
}
