using MyCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZC.Platform.Model;

namespace ZC.Platform.API.Model
{
    public class UsersModel
    {
        public class Base
        {

        }
        #region GetUser

        public class ReqGetUser:T_USERS
        {
            
        }

        /// <summary>
        /// 返回类
        /// </summary>
        public class ResGetUserList:ResponseModelBase<List<T_USERS>>
        {

        }
        public class ResGetUser : ResponseModelBase<T_USERS>
        {

        }
        #endregion
    }
}
