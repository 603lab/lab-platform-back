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

        }
        public class ResUsersBase:ResponseModelBase
        {

        }

        public class ResLogin : ResponseModelBase<USERSBASE>
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

    }
}
