using System;
using System.Linq;
using System.Text;

namespace ZC.Platform.Model
{
    public class T_USERS
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int ID {get;set;}

        /// <summary>
        /// Desc:账号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string username {get;set;}

        /// <summary>
        /// Desc:密码 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string password {get;set;}

        /// <summary>
        /// Desc:手机号码 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string phone_num {get;set;}

        /// <summary>
        /// Desc:真实姓名 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string real_name {get;set;}

        /// <summary>
        /// Desc:昵称 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string nick_name {get;set;}

        /// <summary>
        /// Desc:微信号码 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string wechat {get;set;}

        /// <summary>
        /// Desc:qq号码 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string qq {get;set;}

        /// <summary>
        /// Desc:头像地址 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string head_img {get;set;}

        /// <summary>
        /// Desc:班级名称 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string class_name {get;set;}

        /// <summary>
        /// Desc:身份证号 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string idCard {get;set;}

        /// <summary>
        /// Desc:技术方向 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string tech_direction {get;set;}

        /// <summary>
        /// Desc:积分 
        /// Default:0 
        /// Nullable:False 
        /// </summary>
        public UInt32 scores {get;set;}

        /// <summary>
        /// Desc:担当职责 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string job {get;set;}

        /// <summary>
        /// Desc:入学年 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? entrance_year {get;set;}

        /// <summary>
        /// Desc:专业方向 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string u_major {get;set;}

        /// <summary>
        /// Desc:学号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string u_code {get;set;}

        /// <summary>
        /// Desc:邮箱 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string email {get;set;}

        /// <summary>
        /// Desc:性别 0 女 1男 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int is_male {get;set;}

        /// <summary>
        /// Desc:是否为管理员 0否 1是 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int is_admin {get;set;}

        /// <summary>
        /// Desc:用户最后登录标识 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string last_login_token {get;set;}

        /// <summary>
        /// Desc:用户最后登录时间 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? last_login_time {get;set;}

        /// <summary>
        /// Desc:创建人名称 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string create_user_name {get;set;}

        /// <summary>
        /// Desc:创建人编码 
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
