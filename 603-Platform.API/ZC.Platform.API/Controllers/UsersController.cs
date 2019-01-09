using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySugar;
using MySqlSugar;
using MyCommon;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.IO;
using ZC.Platform.Model;
using static ZC.Platform.API.Model.UsersModel;

namespace ZC.Platform.API.Controllers
{
    [Route("MySugarCore/Users")]
    public class UsersController : Controller
    {

        /// <summary>
        /// Get单个查询 - url
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet("GetUrl")]
        public ResGetUser GetUrl([FromForm]ReqGetUser user)
        {
            ResGetUser retValue = new ResGetUser();
            string logContent = string.Empty;
            try
            {

                using (var db = DbContext.GetInstance(ref logContent))
                {
                    var my = db.Queryable<T_USERS>()
                        .Where(s => s.ID == user.ID)
                        .FirstOrDefault();
                    retValue.SuccessDefalut(my, 1, "不存在该用户");

                    //记录日志
                    Startup.log.Info(LogHelper.LogDetails(logContent, user, retValue));
                }
            }
            catch (Exception ex)
            {
                retValue.FailDefalut(ex);
                Startup.log.Error(logContent, ex);
            }

            return retValue;
        }

        /// <summary>
        /// Get单个查询 - form-data
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet("Get")]
        public ResGetUser Get([FromForm]ReqGetUser user)
        {
            ResGetUser retValue = new ResGetUser();
            try
            {
                using (var db = DbContext.GetInstance())
                {
                    var my = db.Queryable<T_USERS>()
                        .Where(s => s.ID == user.ID)
                        .FirstOrDefault();
                    retValue.SuccessDefalut(my, 1, "不存在该用户");
                }
            }
            catch (Exception ex)
            {
                retValue.FailDefalut(ex);
            }

            return retValue;
        }

        /// <summary>
        /// Post单个查询
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("GetUser")]
        public ResGetUser GetUser([FromBody]ReqGetUser user)
        {
            ResGetUser retValue = new ResGetUser();
            try
            {
                using (var db = DbContext.GetInstance())
                {
                    var my = db.Queryable<T_USERS>()
                        .Where(s => s.ID == user.ID)
                        .FirstOrDefault();
                    retValue.SuccessDefalut(my, 1, "不存在该用户");
                }
            }
            catch (Exception ex)
            {
                retValue.FailDefalut(ex);
            }

            return retValue;
        }

        /// <summary>
        /// Post查询列表
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("GetUserList")]
        public ResGetUserList GetUserList([FromBody]ReqGetUser user)
        {
            ResGetUserList retValue = new ResGetUserList();
            try
            {
                using (var db = DbContext.GetInstance())
                {
                    var userList = db.Queryable<T_USERS>()
                        .Where(s => s.is_male == user.is_male)
                        .ToList();
                    retValue.SuccessDefalut(userList, userList.Count);
                }
            }
            catch (Exception ex)
            {
                retValue.FailDefalut(ex);
            }

            return retValue;
        }


    }

}