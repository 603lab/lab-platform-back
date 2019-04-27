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
using Microsoft.AspNetCore.Cors;

namespace ZC.Platform.API.Controllers
{
    [EnableCors("any")]
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
                        LogWirter.Record(LogType.Admin, OpType.Add, "", "创建用户", Convert.ToInt32(user.uCode), user.createUserCode, user.createUserName);
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
                    My my = new My();
                    ReqToDBGenericClass<USERSBASE, My>.ReqToDBInstance(userInfo, my);
                    my.techList = JsonConvert.DeserializeObject<List<string>>(my.techDirection);
                    retValue.SuccessDefalut(my, 1, "账号或者密码错误");

                    //记录登录日志
                    if (userInfo != null)
                    {
                        LogWirter.Record(LogType.Login,OpType.Login,"","登录",0, userInfo.createUserCode, userInfo.createUserName);
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
        /// （未完成）短信
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("PhoneLogin")]
        public ResPhoneLogin PhoneLogin([FromBody]ReqUsersBase user)
        {
            ResPhoneLogin retValue = new ResPhoneLogin();
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
                    db.AddDisableUpdateColumns("username", "password","is_admin","follow_num","followed_num","ID","scores","u_code","create_time");

                    bool isIDExist = db.Queryable<USERSBASE>()
                        .Any(s => s.uCode == user.uCode);
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
                            
                            user.techDirection = JsonConvert.SerializeObject(user.techList);
                            var newUser = db.Queryable<USERSBASE>().Where(s => s.uCode == user.uCode).FirstOrDefault();
                            if (newUser != null)
                            {
                                user.ID = newUser.ID;
                                ReqToDBGenericClass<ReqUsersBase, USERSBASE>.ReqToDBInstance(user, newUser);
                                db.Update(newUser);
                                retValue.SuccessDefalut("更新成功！", 1);
                                LogWirter.Record(LogType.Login, OpType.Update, "信息", "首次登录，编辑", Convert.ToInt32(user.uCode), user.createUserCode, user.createUserName);
                            }
                            
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
                bool status = true;
                if (string.IsNullOrEmpty(user.phoneNum))
                {
                    retValue.FailDefalut("请填写你的手机号");
                    status = false;
                }
                else if (string.IsNullOrEmpty(user.uCode))
                {
                    retValue.FailDefalut("填写你的学号");
                    status = false;
                }
                else if (string.IsNullOrEmpty(user.idCard))
                {
                    retValue.FailDefalut("请填写你的身份证号码");
                    status = false;
                }
                if (status)
                {
                    try
                    {
                        //设置禁止更新列
                        db.AddDisableUpdateColumns("username", "is_admin");

                        var isExist = db.Queryable<USERSBASE>()
                            .Any(s => s.uCode == user.uCode);
                        if (isExist)
                        {
                            #region 验证必填信息及其格式

                            if (string.IsNullOrEmpty(user.password))
                            {
                                retValue.FailDefalut("请填写你的新密码！");
                            }

                            #endregion

                            var student = db.Queryable<USERSBASE>().Where(u => u.uCode == user.uCode).FirstOrDefault();
                            if (student != null)
                            {
                                if (student.idCard != user.idCard || student.phoneNum != user.phoneNum)
                                {
                                    retValue.FailDefalut("手机号或者身份证与学号不对应");
                                }
                                else
                                {
                                    db.Update<USERSBASE>(new { password = user.password }, it => it.phoneNum == user.phoneNum); //只更新密码列

                                    retValue.SuccessDefalut("更新成功！", 1);
                                    var info = db.Queryable<USERSBASE>().Where(s => s.phoneNum == user.phoneNum).FirstOrDefault();
                                    LogWirter.Record(LogType.Login, OpType.Update, "手机号码", "通过手机号更新密码", Convert.ToInt32(info.uCode), info.createUserCode, info.createUserName);
                                }
                            }
                            else
                            {
                                retValue.FailDefalut("不存在该学号");
                            }
                           
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
         * 4.百科动态 - 添加日至部分
         *   - 获取百科动态
         *   - 点击链接的跳转？？
         *   
         * 5.关注动态 - 添加日志部分
         *   - 获取关注动态
         *   - 链接跳转（重复）
         * 6.项目动态 - 添加日志部分
         *   - 获取动态
         *   - 查看详情
         *   
         *  7.个人标签 - Done
         *  - 获取个人标签
         *  - 创建标签
         *  - 删除标签
         *  8.技能饼图 - Done
         *   - 更新或者上传技能图
         */

        //百科动态部分

        /// <summary>
        /// 获取百科动态内容
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("GetDocActive")]
        public ResGetDocActive GetDocActive([FromHeader]ReqGetDocActive req)
        {
            ResGetDocActive retValue = new ResGetDocActive();
            using (var db = DbContext.GetInstance("T_LOG"))
            {
                #region 判断必填项
                bool status = true;

                
                if (!Enum.GetNames(typeof(ActiveType)).Contains(req.activeType))
                {
                    retValue.FailDefalut("请填写正确的动态类型");
                    return retValue;
                }

                #endregion

                try
                {
                    if (status)
                    {
                        //百科动态
                        if (req.activeType == "Doc")
                        {
                            var resList = db.Queryable<LOGBASE>().Where(s => s.logType == "Doc").ToList();

                            retValue.SuccessDefalut(resList, resList.Count, "没有任何关注动态");
                        }
                        //关注动态
                        else if (req.activeType == "Follow")
                        {
                            var followList = db.Queryable<T_FOLLOW_USERS>().Where(s => s.create_user_code == req.uCode).ToList();
                            if (followList.Count>0)
                            {
                                List<int> list = new List<int>();
                                foreach (var item in followList)
                                {
                                    list.Add(Convert.ToInt32(item.follow_user_code));
                                }
                                var resList = db.Queryable<LOGBASE>().In("relation_code", list).ToList();

                                //注意为空时候的返回
                                retValue.SuccessDefalut(resList, resList.Count, "没有任何关注动态");
                            }
                            else
                            {
                                retValue.SuccessDefalut(null, 0, "不存在当前用户或者当前用户无关注人群");
                            }
                        }
                        //项目动态
                        else if (req.activeType == "Project")
                        {
                            retValue.SuccessDefalut(null,0, "未开发部分");
                        }
                        //论坛动态
                        else if (req.activeType == "Forum")
                        {
                            retValue.SuccessDefalut(null, 0, "未开发部分");
                        }


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
                        List<newAnnexBase> newRes = new List<newAnnexBase>();
                        string avatar = string.Empty;
                        var user = db.Queryable<USERSBASE>().Where(u => u.uCode == req.uCode).FirstOrDefault();
                        if (user != null)
                        {
                            avatar = user.avatar;
                        }
                        foreach (var item in resList)
                        {
                            newAnnexBase annex = new newAnnexBase();
                            ReqToDBGenericClass<ANNEXBASE, newAnnexBase>.ReqToDBInstance(item, annex);
                            annex.fileTagList = JsonConvert.DeserializeObject<List<string>>(item.fileTag);
                            annex.avatar = avatar;
                            newRes.Add(annex);
                        }
                        
                        
                        retValue.SuccessDefalut(newRes, pageNum);
                    }

                }
                catch (Exception ex)
                {
                    retValue.FailDefalut(ex);
                }

            }
            return retValue;
        }

        //个人标签部分

        /// <summary>
        /// 添加个人标签
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("AddTag")]
        public ResAddUserTag AddTag([FromBody]USERTAGSBASE req)
        {
            ResAddUserTag retValue = new ResAddUserTag();
            bool status = true;
            if (string.IsNullOrEmpty(req.createUserCode))
            {
                retValue.FailDefalut("请填写创建人code！");
                status = false;
            }
            else if (string.IsNullOrEmpty(req.createUserName))
            {
                retValue.FailDefalut("请填写创建人Name！");
                status = false;
            }
            else if (string.IsNullOrEmpty(req.label))
            {
                retValue.FailDefalut("标签内容不能为空！");
                status = false;
            }
            if (status)
            {
                using (var db = DbContext.GetInstance("T_USER_TAGS"))
                {
                    try
                    {
                        //设置创建时间
                        req.createTime = DateTime.Now;
                        
                        var isExist = db.Queryable<USERTAGSBASE>()
                             .Any(s => s.label == req.label && s.createUserCode == req.createUserCode);

                        if (!isExist)
                        {
                            var res = db.Insert(req);
                            retValue.SuccessDefalut("创建成功！", 1);
                            LogWirter.Record(LogType.Personal, OpType.Add, req.label + "]", "添加个人用户标签 [", Convert.ToInt32(res), req.createUserCode, req.createUserName);
                        }
                        else
                        {
                            retValue.FailDefalut("用户相同标签！");
                        }

                    }
                    catch (Exception ex)
                    {
                        retValue.FailDefalut(ex);
                    }

                }
            }
            
            return retValue;
        }

        /// <summary>
        /// 删除个人标签
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("DeleteTag")]
        public ResponseModelBase DeleteTag([FromBody]USERTAGSBASE req)
        {
            ResponseModelBase retValue = new ResponseModelBase();
            bool status = true;
            if (string.IsNullOrEmpty(req.createUserCode))
            {
                retValue.FailDefalut("请填写创建人code！");
                status = false;
            }
           
            else if (req.ID==0)
            {
                retValue.FailDefalut("请填写正确的标签ID");
                status = false;
            }
            if (status)
            {
                using (var db = DbContext.GetInstance("T_USER_TAGS"))
                {
                    try
                    {
                        //设置创建时间
                        req.createTime = DateTime.Now;

                        var isExist = db.Queryable<USERTAGSBASE>()
                             .Any(s => s.ID == req.ID && s.createUserCode == req.createUserCode);

                        if (isExist)
                        {
                            db.Delete(req);
                            retValue.SuccessDefalut("删除成功！", 1);
                            LogWirter.Record(LogType.Personal, OpType.Delete, req.label+"]", "删除个人用户标签 [", 0, req.createUserCode, req.createUserName);
                        }
                        else
                        {
                            retValue.FailDefalut("不存在改标签ID！");
                        }

                    }
                    catch (Exception ex)
                    {
                        retValue.FailDefalut(ex);
                    }

                }
            }
               
            return retValue;
        }

        /// <summary>
        /// 获取个人便签信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet("GetTags")]
        public ResUserTags GetTags([FromHeader]USERTAGSBASE req)
        {
            ResUserTags retValue = new ResUserTags();
            bool status = true;
            if (string.IsNullOrEmpty(req.createUserCode))
            {
                retValue.FailDefalut("请填写创建人code！");
                status = false;
            }
            if (status)
            {
                using (var db = DbContext.GetInstance("T_USER_TAGS"))
                {
                    try
                    {

                    var resList = db.Queryable<USERTAGSBASE>()
                        .Where(s => s.createUserCode == req.createUserCode).ToList();
                    retValue.SuccessDefalut(resList, resList.Count);
                       

                    }
                    catch (Exception ex)
                    {
                        retValue.FailDefalut(ex);
                    }

                }
            }
            
            return retValue;
        }

        //标签圆饼

        /// <summary>
        /// 获取技能圆饼图
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet("GetSkills")]
        public ResSkills GetSkills([FromHeader]USERSKILLBASE req)
        {
            ResSkills retValue = new ResSkills();
            bool status = true;
            if (string.IsNullOrEmpty(req.createUserCode))
            {
                retValue.FailDefalut("请填写创建人code！");
                status = false;
            }
            if (status)
            {
                using (var db = DbContext.GetInstance("T_USER_SKILL"))
                {
                    try
                    {

                        var resList = db.Queryable<USERSKILLBASE>()
                            .Where(s => s.createUserCode == req.createUserCode).ToList();
                        retValue.SuccessDefalut(resList, resList.Count);


                    }
                    catch (Exception ex)
                    {
                        retValue.FailDefalut(ex);
                    }

                }
            }

            return retValue;
        }

        /// <summary>
        ///  更新圆饼图
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("UpdateSkill")]
        public ResUpdateForm UpdateSkill([FromBody]ReqUpdateForm req)
        {
            ResUpdateForm retValue = new ResUpdateForm();
            bool status = true;
            if (string.IsNullOrEmpty(req.createUserCode))
            {
                retValue.FailDefalut("请填写创建人Code！");
                status = false;
            }
            else if (string.IsNullOrEmpty(req.createUserName))
            {
                retValue.FailDefalut("请填写创建人Name！");
                status = false;
            }
            if (status)
            {
                using (var db = DbContext.GetInstance("T_USER_SKILL"))
                {
                    try
                    {
                        //删除全部在插入
                        db.Delete<USERSKILLBASE>(s=>s.createUserCode==req.createUserCode);
                        if (req.skillList.Count > 0)
                        {
                            foreach (var item in req.skillList)
                            {
                                item.createUserCode = req.createUserCode;
                                item.createUserName = req.createUserName;
                                db.Insert(item);
                            }
                        }
                        
                        retValue.SuccessDefalut("更新成功！", req.skillList.Count);
                        LogWirter.Record(LogType.Personal, OpType.Update, "", "更新了自己的个人技能图", 0, req.createUserCode, req.createUserName);

                    }
                    catch (Exception ex)
                    {
                        retValue.FailDefalut(ex);
                    }

                }
            }

            return retValue;
        }
    }

}