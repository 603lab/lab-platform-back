using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.Model
{
    public class T_TASK
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int ID {get;set;}

        /// <summary>
        /// Desc:任务标题 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string task_title {get;set;}

        /// <summary>
        /// Desc:截止日期 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public DateTime end_time {get;set;}

        /// <summary>
        /// Desc:任务描述 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string task_description {get;set;}

        /// <summary>
        /// Desc:执行者名称 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string received_user_name {get;set;}

        /// <summary>
        /// Desc:执行者编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int received_user_code {get;set;}

        /// <summary>
        /// Desc:是否完成/完成进度 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int is_done {get;set;}

        /// <summary>
        /// Desc:最后修改时间 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public DateTime last_edit_time {get;set;}

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

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string last_edit_user_code {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string last_edit_user_name {get;set;}

    }
}
