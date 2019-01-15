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
using ZC.Platform.API.BaseModel;

namespace ZC.Platform.API.Controllers
{
    [Route("Base-Module/Users")]
    public class UsersController : Controller
    {
        /// <summary>
        /// 管理员注册新用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public ResUsersBase Add([FromBody]ReqUsersBase user)
        {
            ResUsersBase retValue = new ResUsersBase();
            using (var db = DbContext.GetInstance("T_USERS"))
            {
                try
                {
                    //设置创建时间
                    user.createTime = DateTime.Now;

                    var isExist = db.Queryable<USERSBASE>()
                         .Any(s => s.username == user.username);

                    if (!isExist)
                    {
                        db.Insert(user);
                        retValue.SuccessDefalut("创建新用户成功！", 1);
                    }
                    else
                    {
                        retValue.FailDefalut("用户已存在，请更新账号！");
                    }

                }
                catch (Exception ex)
                {
                    retValue.FailDefalut(ex);
                }

            }
            return retValue;
        }

        /// <summary>
        /// 用户登录 
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public ResLogin Login([FromBody]ReqUsersBase user)
        {
            ResLogin retValue = new ResLogin();
            using (var db = DbContext.GetInstance("T_USERS"))
            {
                try
                {
                    var userInfo = db.Queryable<USERSBASE>()
                        .Where(s => s.username == user.username)
                        .Where(s => s.password == user.password)
                        .FirstOrDefault();

                    retValue.SuccessDefalut(userInfo, 1, "账号或者密码错误");
                }
                catch (Exception ex)
                {
                    retValue.FailDefalut(ex);
                }

            }
            return retValue;
        }


        /// <summary>
        /// （未完成）短信
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("PhoneLogin")]
        public ResLogin PhoneLogin([FromBody]ReqUsersBase user)
        {
            ResLogin retValue = new ResLogin();
            using (var db = DbContext.GetInstance("T_USERS"))
            {
                try
                {
                    var userInfo = db.Queryable<USERSBASE>()
                        .Where(s => s.phoneNum == user.phoneNum)
                        .FirstOrDefault();

                    retValue.SuccessDefalut(userInfo, 1, "手机号或者验证码错误");
                }
                catch (Exception ex)
                {
                    retValue.FailDefalut(ex);
                }

            }
            return retValue;
        }


        /// <summary>
        /// 首次登录请填写详情
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public ResUsersBase Update([FromBody]ReqUsersBase user)
        {
            ResUsersBase retValue = new ResUsersBase();
            using (var db = DbContext.GetInstance("T_USERS"))
            {
                try
                {
                    //设置禁止更新列
                    db.AddDisableUpdateColumns("username", "password","is_admin");

                    bool isIDExist = db.Queryable<USERSBASE>()
                        .Any(s => s.ID == user.ID);
                    if (isIDExist)
                    {
                        #region 验证必填信息及其格式

                        bool status = true;

                        //登录后将获取的信息存在本地 然后用于请求
                        if (string.IsNullOrEmpty(user.phoneNum))
                        {
                            retValue.FailDefalut("请填写正确的手机号！");
                            status = false;
                        }
                        else if (string.IsNullOrEmpty(user.realName))
                        {
                            retValue.FailDefalut("请填写真实姓名！");
                            status = false;
                        }
                        else if (string.IsNullOrEmpty(user.uCode))
                        {
                            retValue.FailDefalut("请填写你的学号！");
                            status = false;
                        }
                        else if (string.IsNullOrEmpty(user.idCard))
                        {
                            retValue.FailDefalut("请填写你的身份证信息！");
                            status = false;
                        }
                        #endregion
                        if (status)
                        {
                            db.Update(user);
                            retValue.SuccessDefalut("更新成功！", 1);
                        }
                        
                    }
                    else
                    {
                        retValue.FailDefalut("不存在该用户ID");
                    }
                }
                catch (Exception ex)
                {
                    retValue.FailDefalut(ex);
                }

            }
            return retValue;
        }


        /// <summary>
        /// 根据手机号修改密码
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("UpdatePassword")]
        public ResUsersBase UpdatePassword([FromBody]ReqUsersBase user)
        {
            ResUsersBase retValue = new ResUsersBase();
            using (var db = DbContext.GetInstance("T_USERS"))
            {
                try
                {
                    //设置禁止更新列
                    db.AddDisableUpdateColumns("username", "is_admin");

                    var isExist = db.Queryable<USERSBASE>()
                        .Any(s => s.phoneNum == user.phoneNum);
                    if (isExist)
                    {
                        #region 验证必填信息及其格式

                        if (string.IsNullOrEmpty(user. password))
                        {
                            retValue.FailDefalut("请填写你的新密码！");
                        }

                        #endregion
                        db.Update<USERSBASE>(new { password = user.password }, it => it.phoneNum == user.phoneNum); //只更新密码列
                        
                        retValue.SuccessDefalut("更新成功！", 1);
                    }
                    else
                    {
                        retValue.FailDefalut("不存在该用户手机号");
                    }
                }
                catch (Exception ex)
                {
                    retValue.FailDefalut(ex);
                }

            }
            return retValue;
        }


       //以下示例

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
                    USERSBASE users = new USERSBASE();
                    users = ModelConvert.FromTo<T_USERS, USERSBASE>(my, users);
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

                    USERSBASE users = new USERSBASE();
                    //转化成前端友好的数据
                    users = ModelConvert.FromTo<T_USERS, USERSBASE>(my, users);
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
                    USERSBASE users = new USERSBASE();
                    users = ModelConvert.FromTo<T_USERS, USERSBASE>(my, users);
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
        [HttpGet("GetUserList")]
        public ResGetUserList GetUserList([FromHeader]ReqGetUser user)
        {
            ResGetUserList retValue = new ResGetUserList();
            try
            {
                //获取转化表格
                using (var db = DbContext.GetInstance("T_USERS"))
                {
                    var userList = db.Queryable<USERSBASE>()
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