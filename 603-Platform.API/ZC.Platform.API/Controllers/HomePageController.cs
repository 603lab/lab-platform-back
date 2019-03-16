using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySugar;
using MySqlSugar;
using static ZC.Platform.API.Model.HomeModel;
using static MyCommon.EnumCommon;
using static MyCommon.UpdateCommon;
using ZC.Platform.API.BaseModel;
using Microsoft.AspNetCore.Cors;

namespace ZC.Platform.API.Controllers
{
    [EnableCors("any")]
    [Route("Base-Module/Home")]
    public class HomePageController : Controller
    {
        /// <summary>
        ///首页公告栏信息加载
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet("GetNotices")]
        public ResGetNoticese GetNotices([FromHeader]ReqNoticeBase notice)
        {
            ResGetNoticese retValue = new ResGetNoticese();
            using (var db = DbContext.GetInstance("T_NOTICE_BOARD"))
            {
                try
                {
                    //如果有信息被设置为置顶并且根据创建时间降序
                    var topNoticeList = db.Queryable<NOTICEBOARDBASE>()
                        .Where(s => s.isTop == (int)isTop.Yes)
                        .OrderBy(s => s.lastEditTime, OrderByType.desc)
                        .ToList();

                    var noticeList = db.Queryable<NOTICEBOARDBASE>()
                     .Where(s => s.isTop == (int)isTop.No)
                    .OrderBy(s => s.createTime, OrderByType.desc)
                    .ToList();

                    //合并
                    var reList = topNoticeList.Union(noticeList)
                        .ToList();
                    //0是第一页
                    var resultList = reList.Skip((notice.currentPage - 1) * notice.pageSize).Take(notice.pageSize).ToList();

                    retValue.SuccessDefalut(resultList, reList.Count);

                }
                catch (Exception ex)
                {
                    retValue.FailDefalut(ex);
                }

            }
            return retValue;
        }

        /// <summary>
        ///添加公告信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("AddNotice")]
        public ResNoticeBase AddNotice([FromBody]NOTICEBOARDBASE notice)
        {
            ResNoticeBase retValue = new ResNoticeBase();
            using (var db = DbContext.GetInstance("T_NOTICE_BOARD"))
            {
                try
                {
                    //设置时间
                    notice.createTime = DateTime.Now;
                    notice.lastEditTime = DateTime.Now;
                    //设置最近编辑人信息
                    notice.lastEditUserCode = notice.createUserCode;
                    notice.lastEditUserName = notice.createUserName;

                    var res = db.Insert(notice);
                    retValue.SuccessDefalut("发布成功", 1);
                    LogWirter.Record(LogType.Admin, OpType.Add, notice.mainTitle + "]", "添加公告 [", Convert.ToInt32(res), notice.createUserCode, notice.createUserName);
                }
                catch (Exception ex)
                {
                    retValue.FailDefalut(ex);
                }

            }
            return retValue;
        }

        /// <summary>
        /// 修改公告并更新时间
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        [HttpPost("UpdateNotice")]
        public ResNoticeBase UpdateNotice([FromBody]ReqNoticeBase notice)
        {
            ResNoticeBase retValue = new ResNoticeBase();
            using (var db = DbContext.GetInstance("T_NOTICE_BOARD"))
            {
                try
                {
                    bool isIDExist = db.Queryable<NOTICEBOARDBASE>()
                      .Any(s => s.ID == notice.ID);
                    if (isIDExist)
                    {
                        //设置禁止更新列
                        db.AddDisableUpdateColumns("create_time", "create_user_name", "create_user_name");

                        //设置时间
                        notice.lastEditTime = DateTime.Now;
                        //设置最近编辑人信息
                        notice.lastEditUserCode = notice.createUserCode;
                        notice.lastEditUserName = notice.createUserName;

                        var noticeModel = new NOTICEBOARDBASE();

                        db.Update<NOTICEBOARDBASE>(
                            new
                            {
                                mainTitle = notice.mainTitle,
                                subHead = notice.subHead,
                                content = notice.content,
                                formatType = notice.formatType,
                                contentType = notice.contentType,
                                isTop = notice.isTop,
                                lastEditUserName = notice.lastEditUserName,
                                lastEditUserCode = notice.lastEditUserCode
                            },
                            it => it.ID == notice.ID
                            );


                        retValue.SuccessDefalut("修改公告成功！", 1);
                        LogWirter.Record(LogType.Admin, OpType.Update, notice.mainTitle + "]", "修改公告 [", notice.ID, notice.createUserCode, notice.createUserName);
                    }
                    else
                    {
                        retValue.FailDefalut("不存在该公告ID");
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
        ///首页获取我的任务 -分页吧
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet("GetTasks")]
        public ResGetTasks GetTasks([FromHeader]ReqTaskBase task)
        {
            ResGetTasks retValue = new ResGetTasks();
            using (var db = DbContext.GetInstance("T_TASK"))
            {
                try
                {
                    //如果有信息被设置为置顶并且根据创建时间降序
                    var TaskList = db.Queryable<TASKBASE>()
                        .Where(s => s.isDone == task.isDone)
                        .OrderBy(s => s.lastEditTime, OrderByType.desc)
                        .ToList();
                    //分页 0是第一页
                    var reList = TaskList.Skip((task.currentPage-1) * task.pageSize)
                         .Take(task.pageSize).ToList();
                  
                    retValue.SuccessDefalut(reList, TaskList.Count);

                }
                catch (Exception ex)
                {
                    retValue.FailDefalut(ex);
                }

            }
            return retValue;
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [HttpPost("AddTask")]
        public ResTaskBase AddTask([FromBody]TASKBASE task)
        {
            ResTaskBase retValue = new ResTaskBase();
            using (var db = DbContext.GetInstance("T_TASK"))
            {
                try
                {
                    //规范日期格式
                    db.SerializerDateFormat = "yyyy-mm-dd";
                    //设置时间
                    task.createTime = DateTime.Now;
                    task.lastEditTime = DateTime.Now;
                    //设置最近编辑人信息
                    task.lastEditUserCode = task.createUserCode;
                    task.lastEditUserName = task.createUserName;

                    var res = db.Insert(task);
                    retValue.SuccessDefalut("添加任务成功", 1);
                    LogWirter.Record(LogType.Personal, OpType.Add, task.taskTitle + "]", "创建任务 [", Convert.ToInt32(res), task.createUserCode, task.createUserName);
                }
                catch (Exception ex)
                {
                    retValue.FailDefalut(ex);
                }

            }
            return retValue;
        }

        /// <summary>
        /// 编辑任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [HttpPost("UpdateTask")]
        public ResTaskBase UpdateTask([FromBody]ReqTaskBase task)
        {
            ResTaskBase retValue = new ResTaskBase();
            using (var db = DbContext.GetInstance("T_TASK"))
            {
                try
                {
                    bool isIDExist = db.Queryable<TASKBASE>()
                      .Any(s => s.ID == task.ID);
                    if (isIDExist)
                    {
                        // 设置禁止更新列
                        db.AddDisableUpdateColumns("create_time");

                        //规范日期格式
                        db.SerializerDateFormat = "yyyy-mm-dd";

                        //设置时间
                        task.lastEditTime = DateTime.Now;

                        //设置最近编辑人
                        task.lastEditUserCode = task.createUserCode;
                        task.lastEditUserName = task.createUserName;
                        
                        var taskModel = new TASKBASE();
                        
                        //只更新需要更新的部分
                        db.Update<TASKBASE>(
                            new
                            {
                                taskTitle = task.taskTitle,
                                endTime = task.endTime,
                                taskDescription = task.taskDescription,
                                isDone = task.isDone,
                                lastEditUserName = task.lastEditUserName,
                                lastEditUserCode = task.lastEditUserCode

                            },
                            it => it.ID == task.ID
                            );

                        retValue.SuccessDefalut("编辑任务成功", 1);
                        LogWirter.Record(LogType.Personal, OpType.Update, task.taskTitle + "]", "编辑任务 [", task.ID, task.createUserCode, task.createUserName);
                    }
                    else
                    {
                        retValue.FailDefalut("不存在该任务ID");
                    }

                }
                catch (Exception ex)
                {
                    retValue.FailDefalut(ex);
                }

            }
            return retValue;
        }

        //大佬推荐和文章推荐部分
        //暂时设定为 (被关注)人气排行前十
        // 文章推荐为 收藏点赞总数排行前十

        /// <summary>
        ///大佬推荐 - 前端后端
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet("GetLeaders")]
        public ResGetLeaders GetLeaders([FromHeader]ReqGetLeaders req)
        {
            ResGetLeaders retValue = new ResGetLeaders();
            using (var db = DbContext.GetInstance("T_USERS"))
            {
                try
                {
                    //如果有信息被设置为置顶并且根据创建时间降序
                    var userList = db.Queryable<USERSBASE>()
                        .Where(s => s.techDirection.Contains(req.leaderType))
                        .OrderBy(s => s.followedNum, OrderByType.desc)
                        .ToList();
                    //分页 0是第一页
                    var reList = userList.Skip((req.currentPage - 1) * req.pageSize)
                         .Take(req.pageSize).ToList();

                    retValue.SuccessDefalut(reList, userList.Count);

                }
                catch (Exception ex)
                {
                    retValue.FailDefalut(ex);
                }

            }
            return retValue;
        }

        [HttpGet("GetGoods")]
        public ResGetGoods GetGoods([FromHeader]ReqGetGoods req)
        {
            ResGetGoods retValue = new ResGetGoods();
            using (var db = DbContext.GetInstance("T_ANNEX"))
            {
                try
                {
                    //如果有信息被设置为置顶并且根据创建时间降序
                    var annexList = db.Queryable<ANNEXBASE>()
                        .Where(s => s.fileTag.Contains(req.articleType))
                        .OrderBy(s => s.likeNum, OrderByType.desc)
                        .ToList();
                    //分页 0是第一页
                    var reList = annexList.Skip((req.currentPage - 1) * req.pageSize)
                         .Take(req.pageSize).ToList();

                    retValue.SuccessDefalut(reList, annexList.Count);

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