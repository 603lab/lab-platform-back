using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlSugar;
using MySugar;
using ZC.Platform.API.BaseModel;
using ZC.Platform.Model;
using static MyCommon.EnumCommon;
using static MyCommon.UpdateCommon;
using static ZC.Platform.API.Model.EncyclopediaModel;

namespace ZC.Platform.API.Controllers
{
    [Route("Base-Module/Encyclopedia")]
    public class EncyclopediaController : Controller
    {
        /// <summary>
        ///获取百科基本菜单
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet("GetMenu")]
        public ResGetMenu GetMenu([FromHeader]ReqGetMenu req)
        {
            ResGetMenu retValue = new ResGetMenu();
            using (var db = DbContext.GetInstance("T_DOC_CATLOG"))
            {
                try
                {
                    var resultList = db.Queryable<DOCCATLOGBASE>()
                        .ToList();

                    retValue.SuccessDefalut(resultList, resultList.Count);

                }
                catch (Exception ex)
                {
                    retValue.FailDefalut(ex);
                }

            }
            return retValue;
        }

        /// <summary>
        ///获取百科条件查询
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet("Search")]
        public ResSearch Search([FromHeader]ReqSearch req)
        {
            ResSearch retValue = new ResSearch();
            using (var db = DbContext.GetInstance("T_ANNEX"))
            {
                try
                {
                    var annexList = db.Queryable<ANNEXBASE>();
                    //作者
                    if (!string.IsNullOrEmpty(req.authorName))
                    {
                        annexList.Where(it => it.createUserName.Contains(req.authorName));
                    }
                    //点赞数
                    if (req.likeNum.min != null)
                    {
                        annexList.Where(it => it.likeNum > req.likeNum.min);
                    }
                    if (req.likeNum.max != null)
                    {
                        annexList.Where(it => it.likeNum < req.likeNum.max);
                    }
                    //标签
                    if (req.taps != null)
                    {
                        annexList.In("tap", req.taps);
                    }

                    //作者关注数
                    if (req.followNum.min != null || req.followNum.max != null)
                    {
                        //annexList.JoinTable<T_USERS>((it, it2) => it.createUserCode == it2.u_code)
                        //    .Where<T_USERS>((it, it2) => it2.followed_num >= req.followNum.min);

                        annexList.Where<ANNEXBASE, T_USERS>((it, it2) => it.createUserCode.Equals(it2.u_code));
                        if (req.followNum.min != null)
                        {
                            annexList.Where<T_USERS>((it, it2) => it2.followed_num >= req.followNum.min);
                        }

                        if (req.followNum.max != null)
                        {
                            annexList.Where<T_USERS>((it, it2) => it2.followed_num <= req.followNum.max);
                        }
                    }
                    //在取分页总数的时候节省性能
                    var totalList = annexList.ToList();
                    var resultList = totalList.Skip((req.currentPage - 1) * req.pageSize).Take(req.pageSize).ToList();

                    retValue.SuccessDefalut(resultList, totalList.Count);

                }
                catch (Exception ex)
                {
                    retValue.FailDefalut(ex);
                }

            }
            return retValue;
        }

        /// <summary>
        /// 创建文章
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("AddDoc")]
        public ResAddDoc AddDoc([FromBody] ReqAddDoc req)
        {
            ResAddDoc retValue = new ResAddDoc();

            using (var db = DbContext.GetInstance("T_ANNEX"))
            {

                #region 判断必填项
                bool status = true;
                if (req.parentId == 0)
                {
                    retValue.FailDefalut("请填写文件父类ID");
                    status = false;
                }
                else if (string.IsNullOrEmpty(req.fileAddress))
                {
                    retValue.FailDefalut("请填写文件地址");
                    status = false;
                }
                else if (string.IsNullOrEmpty(req.fileName))
                {
                    retValue.FailDefalut("请填写文件地址");
                    status = false;
                }
                //这里需要维护一个字典表 转化type code
                else if (string.IsNullOrEmpty(req.type))
                {
                    retValue.FailDefalut("请填写文件类型");
                    status = false;
                }
                else if (string.IsNullOrEmpty(req.createUserCode))
                {
                    retValue.FailDefalut("必填参数创建人编号");
                    status = false;
                }
                else if (string.IsNullOrEmpty(req.createUserName))
                {
                    retValue.FailDefalut("必填参数创建人姓名");
                    status = false;
                }

                #endregion
                try
                {
                    if (status)
                    {
                        //设置创建时间
                        req.createTime = DateTime.Now;
                        db.Insert(req);
                        retValue.SuccessDefalut("创建成功", 1);
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
        /// 点赞/或取消点赞
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Like")]
        public ResLike Like([FromBody] ReqLike req)
        {
            ResLike retValue = new ResLike();

            using (var db = DbContext.GetInstance("T_LIKE"))
            {
                bool status = true;
                if (string.IsNullOrEmpty(req.createUserCode))
                {
                    retValue.FailDefalut("必填参数创建人编号");
                    status = false;
                }
                else if (string.IsNullOrEmpty(req.createUserName))
                {
                    retValue.FailDefalut("必填参数创建人姓名");
                    status = false;
                }
                else
                {
                    status = db.Queryable<T_ANNEX>().Any(s => s.ID == req.itemId);
                    retValue.FailDefalut("不存在该项目ID");
                }

                try
                {
                    if (status)
                    {
                        //设置创建时间
                        req.createTime = DateTime.Now;
                        //设置超时时间
                        db.CommandTimeOut = 30000;
                        //创建点赞记录
                        if (req.isLike == (int)isLike.Yes)
                        {
                            //判断是否重复创建点赞
                            bool isExist = db.Queryable<LIKEBASE>().Any(s => s.itemId == req.itemId && s.createUserCode == req.createUserCode);
                            if (isExist)
                            {
                                retValue.FailDefalut("请勿重复点赞！");
                            }
                            else
                            {
                                LIKEBASE like = new LIKEBASE();
                                //转化
                                ReqToDBGenericClass<ReqLike, LIKEBASE>.ReqToDBInstance(req, like);
                                //事务 同步文档点赞数

                                //获取原点赞数+1
                                var annex = db.Queryable<T_ANNEX>().Where(s => s.ID == req.itemId).First();
                                int likeNum = annex.like_num + 1;

                                try
                                {
                                    db.BeginTran();//开启事务
                                    db.Insert(like);
                                    db.Update<T_ANNEX>(
                                        new
                                        {
                                            like_num = likeNum
                                        },
                                        it => it.ID == req.itemId
                                        );
                                    db.CommitTran();//提交事务
                                    retValue.SuccessDefalut("点赞成功!", 1);
                                }
                                catch (Exception ex)
                                {
                                    db.RollbackTran();//回滚事务
                                    retValue.FailDefalut("异常错误："+ex.Message);
                                }
                                

                            }  
                        }
                        //删除点赞
                        else if (req.isLike == (int)isLike.No)
                        {
                            var item = db.Queryable<LIKEBASE>().Where(s => s.itemId == req.itemId && s.createUserCode == req.createUserCode).First();
                            var annex = db.Queryable<T_ANNEX>().Where(s => s.ID == req.itemId).First();
                            int likeNum = annex.like_num - 1;
                            try
                            {
                                db.BeginTran();//开启事务
                                db.Delete(item);
                                db.Update<T_ANNEX>(
                                    new
                                    {
                                        like_num = likeNum
                                    },
                                    it => it.ID == req.itemId
                                    );
                                db.CommitTran();//提交事务
                                retValue.SuccessDefalut("取消点赞成功！", 1);
                            }
                            catch (Exception ex)
                            {
                                db.RollbackTran();//回滚事务
                                retValue.FailDefalut("异常错误：" + ex.Message);
                            }
                        }
                        else
                        {
                            retValue.FailDefalut("非正常点赞参数，请修改！");
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
        /// 点赞/或取消点赞
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Collect")]
        public ResCollect Collect([FromBody] ReqCollect req)
        {
            ResCollect retValue = new ResCollect();

            using (var db = DbContext.GetInstance("T_COLLECTION"))
            {
                bool status = true;
                if (string.IsNullOrEmpty(req.createUserCode))
                {
                    retValue.FailDefalut("必填参数创建人编号");
                    status = false;
                }
                else if (string.IsNullOrEmpty(req.createUserName))
                {
                    retValue.FailDefalut("必填参数创建人姓名");
                    status = false;
                }
                else
                {
                    status = db.Queryable<T_ANNEX>().Any(s => s.ID == req.itemId);
                    retValue.FailDefalut("不存在该项目ID");
                }

                try
                {
                    if (status)
                    {
                        //设置创建时间
                        req.createTime = DateTime.Now;

                        //创建记录
                        if (req.isCollect == (int)isCollected.Yes)
                        {
                            //判断是否重复创建收藏
                            bool isExist = db.Queryable<COLLECTIONBASE>().Any(s => s.itemId == req.itemId && s.createUserCode == req.createUserCode);
                            if (isExist)
                            {
                                retValue.FailDefalut("改文档已经被收藏！");
                            }
                            else
                            {
                                COLLECTIONBASE collection = new COLLECTIONBASE();
                                //转化
                                ReqToDBGenericClass<ReqCollect, COLLECTIONBASE>.ReqToDBInstance(req, collection);
                                db.Insert(collection);
                                retValue.SuccessDefalut("收藏成功!", 1);
                            }
                        }
                        //删除收藏
                        else if (req.isCollect == (int)isCollected.No)
                        {
                            var item = db.Queryable<COLLECTIONBASE>().Where(s => s.itemId == req.itemId && s.createUserCode == req.createUserCode).First();
                            db.Delete(item);
                            retValue.SuccessDefalut("取消收藏成功！", 1);
                        }
                        else
                        {
                            retValue.FailDefalut("非正常收藏参数，请修改！");
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
    }
}