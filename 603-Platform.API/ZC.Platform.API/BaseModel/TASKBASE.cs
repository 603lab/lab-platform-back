using MySqlSugar;
using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.API.BaseModel
{
    [SugarMapping(TableName = "T_TASK")]
    public class TASKBASE
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
        public string taskTitle {get;set;}

        /// <summary>
        /// Desc:截止日期 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public DateTime endTime {get;set;}

        /// <summary>
        /// Desc:任务描述 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string taskDescription {get;set;}

        /// <summary>
        /// Desc:执行者名称 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string receivedUserName {get;set;}

        /// <summary>
        /// Desc:执行者编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int receivedUserCode {get;set;}

        /// <summary>
        /// Desc:是否完成/完成进度 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int isDone {get;set;}

        /// <summary>
        /// Desc:最后修改时间 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public DateTime lastEditTime {get;set;}

        /// <summary>
        /// Desc:创建者编号 
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
