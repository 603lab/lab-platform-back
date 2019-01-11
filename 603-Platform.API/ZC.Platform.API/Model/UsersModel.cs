using MyCommon;
using MySqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZC.Platform.Model;

namespace ZC.Platform.API.Model
{
    public class UsersModel
    {
        [SugarMapping(TableName = "T_USERS")]
        /// <summary>
        /// 自动生成的Base类/用于请求
        /// </summary>
        public class UsersBase
        {
            [DataField("ID")]
            public int ID { get; set; }

            [DataField("username")]
            public string username { get; set; }

            [DataField("password")]
            public string password { get; set; }

            [DataField("phone_num")]
            public string phoneNum { get; set; }

            [DataField("real_name")]
            public string realName { get; set; }
            [DataField("nick_name")]
            public string nickName { get; set; }
            [DataField("wechat")]
            public string wechat { get; set; }
            [DataField("qq")]
            public string qq { get; set; }
            [DataField("head_img")]
            public string headImg { get; set; }
            [DataField("class_name")]
            public string className { get; set; }
            [DataField("tech_direction")]
            public string techDirection { get; set; }
            [DataField("scores")]
            public int scores { get; set; }
            [DataField("job")]
            public string job { get; set; }
            [DataField("entrance_year")]
            public int? entranceYear { get; set; }
            [DataField("u_major")]
            public string uMajor { get; set; }
            [DataField("u_code")]
            public string uCode { get; set; }
            [DataField("email")]
            public string email { get; set; }
            [DataField("is_male")]
            public int isMale { get; set; }
            [DataField("is_admin")]
            public int isAdmin { get; set; }
            [DataField("last_login_token")]
            public string lastLoginToken { get; set; }
            [DataField("last_login_time")]
            public DateTime? lastLoginTime { get; set; }
            [DataField("create_user_name")]
            public string createUserName { get; set; }
            [DataField("create_user_code")]
            public string createUserCode { get; set; }
            [DataField("create_time")]
            public DateTime createTime { get; set; }

        }

        public class ReqUsersBase : UsersBase
        {

        }
        public class ResUsersBase:ResponseModelBase
        {

        }


        #region GetUser

        public class ReqGetUser : UsersBase
        {

        }

        /// <summary>
        /// 返回类
        /// </summary>
        public class ResGetUserList : ResponseModelBase<List<UsersBase>>
        {

        }

        public class ResGetUser : ResponseModelBase<UsersBase>
        {

        }
        #endregion

        #region AddUser
        /// <summary>
        /// 返回类
        /// </summary>
        public class ResAddUser : ResponseModelBase<List<T_USERS>>
        {
            public string userID { get; set; }
        }
        public class ReqAddUser : ResponseModelBase<T_USERS>
        {

        }
        #endregion

    }
}
