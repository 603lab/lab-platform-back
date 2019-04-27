using MyCommon;
using MySqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZC.Platform.API.BaseModel;
using ZC.Platform.Model;

namespace ZC.Platform.API.Model
{
    public class UsersModel
    {


        /// <summary>
        ///通用请求类和返回类
        /// </summary>
        public class ReqUsersBase : USERSBASE
        {
            public List<string> techList { get; set; }
           
        }
        public class ResUsersBase:ResponseModelBase
        {

        }
        public class My:USERSBASE
        {
            public List<string> techList { get; set; }
        }
        public class ResLogin : ResponseModelBase<My>
        {

        }

        public class ResPhoneLogin : ResponseModelBase<USERSBASE>
        {

        }

        #region GetUser

        public class ReqGetUser : USERSBASE
        {

        }

        /// <summary>
        /// 返回类
        /// </summary>
        public class ResGetUserList : ResponseModelBase<List<USERSBASE>>
        {

        }

        public class ResGetUser : ResponseModelBase<USERSBASE>
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

        #region GetUsersInfo
        public class UserInfo: USERSBASE
        {
            /// <summary>
            /// 团队编号
            /// </summary>
            public int teamId { get; set; }

            /// <summary>
            /// 团队名称
            /// </summary>
            public string teamName { get; set; }

            /// <summary>
            /// 文章点赞数
            /// </summary>
            public int likeNum { get; set; }


            /// <summary>
            /// 是否为该团队管理员
            /// </summary>
            public int isTeamAdmin { get; set; }

        }


        public class ReqGetUsersInfo : USERSBASE
        {

        }
        public class ResGetUsersInfo : ResponseModelBase<UserInfo>
        {

        }

        #endregion

        #region GetFollowUsers

        /// <summary>
        /// 返回类
        /// </summary>
        public class ReqGetFollowUsers : USERSBASE
        {

        }

        public class ResGetFollowUsers : ResponseModelBase<List<USERSBASE>>
        {

        }
        #endregion
        
        #region GetMyDocs

        /// <summary>
        /// 返回类
        /// </summary>
        public class ReqGetMyDocs 
        {
            public string uCode { get; set; }

            /// <summary>
            /// 搜索条件
            /// </summary>
            public string content { get; set; }

            /// <summary>
            /// 当前页码
            /// </summary>
            public int currentPage { get; set; }

            /// <summary>
            /// 页码大小
            /// </summary>
            public int pageSize { get; set; }
        }

        public class newAnnexBase : ANNEXBASE
        {
            public List<string> fileTagList { get; set; }

            public string avatar { get; set; }
        }
        public class ResGetMyDocs : ResponseModelBase<List<newAnnexBase>>
        {

        }
        #endregion

        #region GetDocActive

        /// <summary>
        /// 返回类
        /// </summary>
        public class ReqGetDocActive
        {
            public string activeType { get; set; }
            public string uCode { get; set; }
        }

        public class ResGetDocActive : ResponseModelBase<List<LOGBASE>>
        {

        }
        #endregion

        #region 个人标签部分

        public class ResAddUserTag : ResponseModelBase
        {

        }

        public class ResUserTags : ResponseModelBase<List<USERTAGSBASE>>
        {

        }

        #endregion

        #region 更新圆饼图

       
        public class ReqUpdateForm 
        {
            public List<USERSKILLBASE> skillList { get; set; }
            public string createUserCode { get; set; }
            public string createUserName{ get; set; }
        }

        public class ResUpdateForm:ResponseModelBase
        {
                
        }

        public class ResSkills: ResponseModelBase<List<USERSKILLBASE>>
        {

        }
        #endregion
    }
}
