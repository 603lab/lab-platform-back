using MyCommon;
using MySqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZC.Platform.Model;
using static ZC.Platform.API.Model.BaseModel;

namespace ZC.Platform.API.Model
{
    public class UsersModel
    {


        /// <summary>
        ///通用请求类和返回类
        /// </summary>
        public class ReqUsersBase : UsersBase
        {

        }
        public class ResUsersBase:ResponseModelBase
        {

        }

        public class ResLogin : ResponseModelBase<UsersBase>
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
