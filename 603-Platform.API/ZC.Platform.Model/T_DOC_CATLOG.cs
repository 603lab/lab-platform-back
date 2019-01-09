using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.Model
{
    public class T_DOC_CATLOG
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
        public string file_name {get;set;}

        /// <summary>
        /// Desc:文件层级 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int level {get;set;}

        /// <summary>
        /// Desc:是否为菜单 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int is_menu {get;set;}

        /// <summary>
        /// Desc:路由地址 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string url {get;set;}

        /// <summary>
        /// Desc:文件存储地址 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string file_address {get;set;}

        /// <summary>
        /// Desc:最后编辑时间 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? last_edit_time {get;set;}

        /// <summary>
        /// Desc:最后编辑用户编号 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string last_edit_user_code {get;set;}

        /// <summary>
        /// Desc:最后编辑用户姓名 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string last_edit_user_name {get;set;}

        /// <summary>
        /// Desc:创建者编码 
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
