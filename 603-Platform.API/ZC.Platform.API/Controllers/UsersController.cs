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
using static MyCommon.UpdateCommon;
using static MyCommon.EnumCommon;

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

        /*个人中心部分
         * 1.初始化页面 - Done
         * 2.显示关注人头像信息以及姓名 
         *  - 返回信息 - Done
         *  - 头像上传
         * 3.显示我的文章 - 包括搜索条件 - Done
         * 
         * 4.百科动态
         *   - 获取百科动态
         *   - 点击链接的跳转？？
         * 5.关注动态
         *   - 获取关注动态
         *   - 链接跳转（重复）
         * 6.项目动态
         *   - 获取动态
         *   - 查看详情
         */

        /// <summary>
        /// 个人中心初始化页面
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("GetUsersInfo")]
        public ResGetUsersInfo GetComments([FromHeader]ReqGetUsersInfo req)
        {
            ResGetUsersInfo retValue = new ResGetUsersInfo();
            using (var db = DbContext.GetInstance("T_USERS"))
            {
                #region 判断必填项
                bool status = true;

                if (string.IsNullOrEmpty(req.uCode))
                {
                    retValue.FailDefalut("必填参数用户编号");
                    status = false;
                }


                #endregion

                try
                {
                    if (status)
                    {
                        var res= db.Queryable<USERSBASE>()
                       .Where(s => s.uCode == req.uCode)
                       .FirstOrDefault();

                        UserInfo userInfo = new UserInfo();
                        ReqToDBGenericClass<USERSBASE, UserInfo>.ReqToDBInstance(res, userInfo);

                        //获取用户所在团队
                        var team = db.Queryable<T_TEAM_MEMBERS>().Where(s => s.user_code == req.uCode).FirstOrDefault();
                        if (team != null)
                        {
                            userInfo.teamId = team.team_id;
                            userInfo.teamName = team.team_name;
                            userInfo.isTeamAdmin = team.is_admin;
                        }


                        //获取文章点赞数 
                        //获取文章点赞总数
                        userInfo.likeNum = db.Queryable<T_LIKE>().Where(s => s.type == LikeType.Doc.ToString()).Where("item_id in (select ID from T_ANNEX where create_user_code = " + req.uCode + ")").Count();


                        retValue.SuccessDefalut(userInfo, 1,"不存在该用户");
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
        /// 获取用户关注人列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("GetFollowUsers")]
        public ResGetFollowUsers GetFollowUsers([FromHeader]ReqGetFollowUsers req)
        {
            ResGetFollowUsers retValue = new ResGetFollowUsers();
            using (var db = DbContext.GetInstance("T_USERS"))
            {
                #region 判断必填项
                bool status = true;

                if (string.IsNullOrEmpty(req.uCode))
                {
                    retValue.FailDefalut("必填参数用户编号");
                    status = false;
                }


                #endregion

                try
                {
                    if (status)
                    {

                        var list = db.Queryable<USERSBASE>()
                            .Where("u_code in (select follow_user_code from T_FOLLOW_USERS where create_user_code = " + req.uCode + ")").ToList();

                        retValue.SuccessDefalut(list, list.Count, "不存在该用户");
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
        /// 搜索条件+获取我的文章 - 分页
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("GetMyDocs")]
        public ResGetMyDocs GetMyDocs([FromHeader]ReqGetMyDocs req)
        {
            ResGetMyDocs retValue = new ResGetMyDocs();
            using (var db = DbContext.GetInstance("T_ANNEX"))
            {
                #region 判断必填项
                bool status = true;

                if (string.IsNullOrEmpty(req.uCode))
                {
                    retValue.FailDefalut("必填参数用户编号");
                    status = false;
                }


                #endregion

                try
                {
                    if (status)
                    {

                        var list = db.Queryable<ANNEXBASE>()
                            .Where(s => s.createUserCode == req.uCode);

                        //内容或者标签标题包含 搜索条件则输出
                        if (!string.IsNullOrEmpty(req.content))
                        {
                            list.Where(s => s.content.Contains(req.content) || s.fileTag.Contains(req.content) || s.fileName.Contains(req.content));
                               
                        }
                        int pageNum = 0;
                        var resList = list.OrderBy(s => s.createTime, OrderByType.desc).ToPageList(req.currentPage,req.pageSize, ref pageNum);

                        retValue.SuccessDefalut(resList, pageNum);
                    }

                }
                catch (Exception ex)
                {
                    retValue.FailDefalut(ex);
                }

            }
            return retValue;
        }
    }

}