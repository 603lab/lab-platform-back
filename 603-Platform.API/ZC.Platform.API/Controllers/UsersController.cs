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
    [Route("Base-Module/Users")]
    public class UsersController : Controller
    {
        /// <summary>
        /// 管理员注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("InsertOrUpdate")]
        public ResUsersBase InsertOrUpdate([FromBody]ReqUsersBase user)
        {
            ResUsersBase retValue = new ResUsersBase();
            using (var db = DbContext.GetInstance("T_USERS"))
            {
                try
                {
                    //设置创建时间
                    user.createTime = DateTime.Now;

                    db.InsertOrUpdate(user);
                    retValue.SuccessDefalut(1);
                }
                catch (Exception ex)
                {
                    retValue.FailDefalut(ex);
                }
                
            }
            return retValue;
        }


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
                    //转化成前端友好的数据
                    UsersBase users = new UsersBase();
                    users = ModelConvert.FromTo<T_USERS, UsersBase>(my, users);
                    retValue.SuccessDefalut(users, 1, "不存在该用户");

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

                    UsersBase users = new UsersBase();
                    //转化成前端友好的数据
                    users = ModelConvert.FromTo<T_USERS, UsersBase>(my, users);
                    retValue.SuccessDefalut(users, 1, "不存在该用户");

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
                    //转化成前端友好的数据
                    UsersBase users = new UsersBase();
                    users = ModelConvert.FromTo<T_USERS, UsersBase>(my, users);
                    retValue.SuccessDefalut(users, 1, "不存在该用户");

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
                //获取转化表格
                using (var db = DbContext.GetInstance())
                {
                    var userList = db.Queryable<UsersBase>()
                        .Where(s => s.isMale == user.isMale)
                        .ToList();
                    
                    //转化成前端友好的数据
                    //var list = userList.Select(s =>
                    //{
                    //    var model = new UsersBase();
                    //    model = ModelConvert.FromTo<T_USERS, UsersBase>(s, model);
                    //    return model;
                    //}).ToList();
                    
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