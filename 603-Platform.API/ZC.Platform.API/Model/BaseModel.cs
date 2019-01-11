using MyCommon;
using MySqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZC.Platform.API.Model
{
    public class BaseModel
    {
        [SugarMapping(TableName = "T_USERS")]
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

            public string idCard { get; set; }
        }

        [SugarMapping(TableName = "T_NOTICE_BOARD")]
        public class NoticeBoardBase
        {

            public int ID { get; set; }

            public string mainTitle { get; set; }

            public string subHead { get; set; }

            public string content { get; set; }

            public int? formatType { get; set; }

            public int? contentType { get; set; }

            public int? isTop { get; set; }

            public int sort { get; set; }

            public string createUserName { get; set; }

            public string createUserCode { get; set; }

            public DateTime createTime { get; set; }

            public DateTime lastEditTime { get; set; }
            public string lastEditUserName { get; set; }
            public string lastEditUserCode { get; set; }
        }

        [SugarMapping(TableName = "T_TASK")]
        public class TaskBase
        {

            public int ID { get; set; }

            public string taskTitle { get; set; }


            public DateTime endTime { get; set; }

            public string taskDescription { get; set; }

            public string receivedUserName { get; set; }

            public int receivedUserCode { get; set; }

            public int isDone { get; set; }

            public string createUserCode { get; set; }

            public string createUserName { get; set; }

            public DateTime createTime { get; set; }
            public DateTime lastEditTime { get; set; }

            public string lastEditUserName { get; set; }
            public string lastEditUserCode { get; set; }
        }

        //暂时都是手动书写的的看一下是不是也能够自动生成！
    }
}
