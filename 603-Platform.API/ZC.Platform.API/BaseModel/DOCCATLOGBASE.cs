using MySqlSugar;
using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.API.BaseModel
{
    [SugarMapping(TableName = "T_DOC_CATLOG")]
    public class DOCCATLOGBASE
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int ID {get;set;}

        /// <summary>
        /// Desc:文件名称 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string fileName {get;set;}

        /// <summary>
        /// Desc:文件层级 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int level {get;set;}

        /// <summary>
        /// Desc:父级路径 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string parentCode {get;set;}

        /// <summary>
        /// Desc:是否为菜单 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int isMenu {get;set;}

        /// <summary>
        /// Desc:路由地址 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string url {get;set;}

        /// <summary>
        /// Desc:文件存储地址 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string fileAddress {get;set;}

        /// <summary>
        /// Desc:最后编辑时间 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? lastEditTime {get;set;}

        /// <summary>
        /// Desc:最后编辑用户编号 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string lastEditUserCode {get;set;}

        /// <summary>
        /// Desc:最后编辑用户姓名 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string lastEditUserName {get;set;}

        /// <summary>
        /// Desc:创建者编码 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string createUserCode {get;set;}

        /// <summary>
        /// Desc:创建者姓名 
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
