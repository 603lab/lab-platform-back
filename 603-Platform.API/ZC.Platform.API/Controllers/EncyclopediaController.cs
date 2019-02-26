using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlSugar;
using MySugar;
using Newtonsoft.Json;
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
                    if (req.likeMin != null)
                    {
                        annexList.Where(it => it.likeNum >=req.likeMin);
                    }
                    if (req.likeMax != null)
                    {
                        annexList.Where(it => it.likeNum <= req.likeMax);
                    }
                    //标签
                    if (req.tags != null)
                    {

                        
                        //annexList.Where(it => it.fileTag.Contains(req.tags[0]) || it.fileTag.Contains(req.tags[1]));
                        string sql = "( (file_tag  LIKE \"%" + req.tags[0] + "%\") ";
                        int i = 0;
                        foreach (var item in req.tags)
                        {
                            if (i == 0)
                            {

                            }
                            else
                            {
                                sql += "OR  (file_tag  LIKE \"%" + item + "%\" ) ";
                            }
                            i++;
                        }
                        sql += ")";
                        annexList.Where(sql);
                       

                    }

                    //作者关注数
                    if (req.followMin != null || req.followMax != null)
                    {
                        

                        //annexList.Where<ANNEXBASE, T_USERS>((it, it2) => it.createUserCode.Equals(it2.u_code));
                        if (req.followMin != null)
                        {
                            /*annexList.Where<T_USERS>((it, it2) => it2.followed_num >= req.followMin);*/
                            annexList.JoinTable<T_USERS>((it, it2) => it.createUserCode == it2.u_code)
                            .Where<T_USERS>((it, it2) => it2.followed_num >= req.followMin);
                        }

                        if (req.followMax != null)
                        {
                            //annexList.Where<T_USERS>((it, it2) => it2.followed_num <= req.followMax);
                            annexList.JoinTable<T_USERS>((it, it2) => it.createUserCode == it2.u_code)
                          .Where<T_USERS>((it, it2) => it2.followed_num <= req.followMin);
                        }
                    }
                    //在取分页总数的时候节省性能

                    var totalList = annexList.ToList();
                    var finalSQL = annexList.ToSql();
                    var resultList = totalList.Skip((req.currentPage - 1) * req.pageSize).Take(req.pageSize).ToList();

                    var list = resultList.Select(it =>
                    {
                        var model = new SearchResult();
                        ReqToDBGenericClass<ANNEXBASE, SearchResult>.ReqToDBInstance(it, model);

                        model.isLike = db.Queryable<T_LIKE>()
                        .Any(s => s.item_id == s.ID && s.create_user_code == req.createUserCode && s.type == LikeType.Doc.ToString());

                        model.isCollected = db.Queryable<T_COLLECTION>()
                           .Any(s => s.item_id == s.ID && s.create_user_code == req.createUserCode);

                        return model;
                    }).ToList();

                    retValue.SuccessDefalut(list, totalList.Count);

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
                        //事务
                        try
                        {
                            db.BeginTran();//开启事务

                            var id = db.Insert(req);

                            //将报文上传 t_edit_detail 表
                            string json = JsonConvert.SerializeObject(req);

                            #region detail的处理

                            T_EDIT_DETIALS detail = new T_EDIT_DETIALS();
                            detail.create_time = DateTime.Now;
                            detail.create_user_code = req.createUserCode;
                            detail.create_user_name = req.createUserName;
                            detail.detail = json;
                            detail.relation_code = id.ObjToInt();
                            detail.type = JsonDetailType.Add.ToString();
                            var res = db.Insert(detail);

                            #endregion


                            db.CommitTran();//提交事务
                            retValue.SuccessDefalut("创建成功", 1);
                            LogWirter.Record(LogType.Doc, OpType.Add, req.fileName+"]", "创建文章 [", Convert.ToInt32(req.createUserCode), req.createUserCode, req.createUserName);
                        }
                        catch (Exception ex)
                        {
                            db.RollbackTran();//回滚事务
                            retValue.FailDefalut($"意外错误,错误信息：{ex.Message}");
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
                else if (!Enum.GetNames(typeof(LikeType)).Contains(req.type))
                {
                    retValue.FailDefalut("不存在该点赞类型，请修改");
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
                            bool isExist = db.Queryable<LIKEBASE>().Any(s => s.itemId == req.itemId && s.createUserCode == req.createUserCode &&s.type==req.type);
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
                                    LogWirter.Record(LogType.Personal, OpType.Add, annex.file_name+"]", "点赞了 [", Convert.ToInt32(annex.create_user_code), req.createUserCode, req.createUserName);
                                }
                                catch (Exception ex)
                                {
                                    db.RollbackTran();//回滚事务
                                    retValue.FailDefalut("异常错误：" + ex.Message);
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
                                LogWirter.Record(LogType.Personal, OpType.Delete, annex.file_name + "]", "取消点赞了 [", Convert.ToInt32(annex.create_user_code), req.createUserCode, req.createUserName);
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
        /// 收藏取消收藏
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
                                retValue.FailDefalut("该文档已经被收藏！");
                            }
                            else
                            {

                                COLLECTIONBASE collection = new COLLECTIONBASE();
                                //转化
                                ReqToDBGenericClass<ReqCollect, COLLECTIONBASE>.ReqToDBInstance(req, collection);
                                //获取原收藏数+1
                                var annex = db.Queryable<T_ANNEX>().Where(s => s.ID == req.itemId).First();
                                int collectNum = annex.collect_num + 1;

                                try
                                {
                                    db.BeginTran();//开启事务
                                    db.Insert(collection);
                                    db.Update<T_ANNEX>(
                                        new
                                        {
                                            collect_num = collectNum
                                        },
                                        it => it.ID == req.itemId
                                        );
                                    db.CommitTran();//提交事务
                                    retValue.SuccessDefalut("收藏成功!", 1);
                                    LogWirter.Record(LogType.Personal, OpType.Add, annex.file_name + "]", "收藏了 [", Convert.ToInt32(annex.create_user_code), req.createUserCode, req.createUserName);
                                }
                                catch (Exception ex)
                                {
                                    db.RollbackTran();//回滚事务
                                    retValue.FailDefalut("异常错误：" + ex.Message);
                                }

                            }
                        }
                        //删除收藏
                        else if (req.isCollect == (int)isCollected.No)
                        {
                            var item = db.Queryable<COLLECTIONBASE>().Where(s => s.itemId == req.itemId && s.createUserCode == req.createUserCode).First();

                            var annex = db.Queryable<T_ANNEX>().Where(s => s.ID == req.itemId).First();
                            int collectNum = annex.collect_num - 1;

                            try
                            {
                                db.BeginTran();//开启事务
                                db.Delete(item);
                                db.Update<T_ANNEX>(
                                    new
                                    {
                                        collect_num = collectNum
                                    },
                                    it => it.ID == req.itemId
                                    );
                                db.CommitTran();//提交事务
                                retValue.SuccessDefalut("取消收藏成功!", 1);
                                LogWirter.Record(LogType.Personal, OpType.Delete, annex.file_name + "]", "取消了 [", Convert.ToInt32(annex.create_user_code), req.createUserCode, req.createUserName);
                            }
                            catch (Exception ex)
                            {
                                db.RollbackTran();//回滚事务
                                retValue.FailDefalut("异常错误：" + ex.Message);
                            }
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

        /// <summary>
        ///获取文章具体内容
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet("GetDocConetnt")]
        public ResGetDocConetnt GetDocConetnt([FromHeader]ReqGetDocConetnt req)
        {
            ResGetDocConetnt retValue = new ResGetDocConetnt();
            using (var db = DbContext.GetInstance("T_ANNEX"))
            {
                try
                {
                    var res = db.Queryable<ANNEXBASE>()
                        .Where(s => s.ID == req.ID)
                       .FirstOrDefault();

                    var model = new DocContent();
                    ReqToDBGenericClass<ANNEXBASE, DocContent>.ReqToDBInstance(res, model);

                    model.isLike = db.Queryable<T_LIKE>()
                        .Any(s => s.item_id == req.ID && s.create_user_code == req.createUserCode && s.type == LikeType.Doc.ToString());

                    model.isCollected = db.Queryable<T_COLLECTION>()
                       .Any(s => s.item_id == req.ID && s.create_user_code == req.createUserCode);

                    retValue.SuccessDefalut(model, 1, "不存在该Doc");
                }
                catch (Exception ex)
                {
                    retValue.FailDefalut(ex);
                }

            }
            return retValue;
        }

        /// <summary>
        ///增加浏览数 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("AddBrowseNum")]
        public ResAddBrowseNum AddBrowseNum([FromBody] ReqAddBrowseNum req)
        {
            ResAddBrowseNum retValue = new ResAddBrowseNum();

            using (var db = DbContext.GetInstance("T_ANNEX"))
            {

                #region 判断必填项
                bool status = true;
                if (req.ID == 0)
                {
                    retValue.FailDefalut("请填写Doc编号");
                    status = false;
                }
                if (string.IsNullOrEmpty(req.createUserCode))
                {
                    retValue.FailDefalut("请填写用户编号");
                    status = false;
                }
                if (string.IsNullOrEmpty(req.createUserName))
                {
                    retValue.FailDefalut("请填写用户姓名");
                    status = false;
                }
                #endregion
                try
                {
                    if (status)
                    {
                        //设置创建时间
                        req.createTime = DateTime.Now;
                        var annex = db.Queryable<ANNEXBASE>().Where(s => s.ID == req.ID).FirstOrDefault();
                        if (annex != null)
                        {
                            db.Update<ANNEXBASE>(
                           new
                           {
                               browseNum = annex.browseNum + 1
                           },
                        it => it.ID == req.ID
                           );
                            retValue.SuccessDefalut("增加浏览数成功", 1);
                            LogWirter.Record(LogType.Doc, OpType.Add, annex.fileName + "]", "查看了 [", Convert.ToInt32(annex.createUserCode), req.createUserCode, req.createUserName);
                        }
                        else
                        {
                            retValue.FailDefalut("不存在改Doc编号");
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
        /// 编辑文章  - 暂时不考虑权限
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("UpdateDoc")]
        public ResUpdateDoc UpdateDoc([FromBody] ReqUpdateDoc req)
        {
            ResUpdateDoc retValue = new ResUpdateDoc();

            using (var db = DbContext.GetInstance("T_ANNEX"))
            {

                #region 判断必填项
                bool status = true;

                if (string.IsNullOrEmpty(req.createUserCode))
                {
                    retValue.FailDefalut("必填参数修改人编号");
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

                        try
                        {
                            db.BeginTran();
                            var annex = db.Queryable<ANNEXBASE>().Where(s => s.ID == req.ID).FirstOrDefault();
                            db.Update<ANNEXBASE>(
                                new
                                {
                                    fileTag = req.fileTag,
                                    content = req.content
                                },
                                it => it.ID == req.ID
                                );

                            //将报文上传 t_edit_detail 表
                            string json = JsonConvert.SerializeObject(req);

                            #region detail的处理

                            T_EDIT_DETIALS detail = new T_EDIT_DETIALS();
                            detail.create_time = DateTime.Now;
                            detail.create_user_code = req.createUserCode;
                            detail.create_user_name = req.createUserName;
                            detail.detail = json;
                            detail.relation_code = req.ID;
                            detail.type = JsonDetailType.Update.ToString();
                            db.Insert(detail);

                            #endregion

                            db.CommitTran();//提交事务
                            retValue.SuccessDefalut("更新成功", 1);
                            LogWirter.Record(LogType.Doc, OpType.Update, annex.fileName + "]", "编辑了 [", Convert.ToInt32(annex.createUserCode), req.createUserCode, req.createUserName);
                        }
                        catch (Exception ex)
                        {
                            db.RollbackTran();//回滚事务
                            retValue.FailDefalut($"意外错误,错误信息：{ex.Message}");
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
        ///获取当前文章所有的编辑历史
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet("GetEditHistory")]
        public ResGetEditHistory GetEditHistory([FromHeader]ReqGetEditHistory req)
        {
            ResGetEditHistory retValue = new ResGetEditHistory();
            using (var db = DbContext.GetInstance("T_EDIT_DETIALS"))
            {
                try
                {

                    var resultList = db.Queryable<EDITDETIALSBASE>()
                        .Where(s => s.relationCode == req.ID)
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
        /// 删除文章 - 需要考虑权限
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("DeleteDoc")]
        public ResDeleteDoc DeleteDoc([FromBody] ReqDeleteDoc req)
        {
            ResDeleteDoc retValue = new ResDeleteDoc();

            using (var db = DbContext.GetInstance("T_ANNEX"))
            {

                #region 判断必填项
                bool status = true;
                var annex = db.Queryable<ANNEXBASE>().Where(s => s.ID == req.ID).FirstOrDefault();
                if (string.IsNullOrEmpty(req.createUserCode))
                {
                    retValue.FailDefalut("必填参数修改人编号");
                    status = false;
                }
                else if (string.IsNullOrEmpty(req.createUserName))
                {
                    retValue.FailDefalut("必填参数创建人姓名");
                    status = false;
                }
                else if (annex==null)
                {
                    retValue.FailDefalut("不存在当前ID的文件");
                    status = false;
                }
                #endregion

                try
                {
                    if (status)
                    {
                        //设置创建时间
                        req.createTime = DateTime.Now;

                        try
                        {
                            db.BeginTran();
                            
                            db.Delete<ANNEXBASE>(s => s.ID == req.ID);

                            //这里需要加入删除的日志

                            db.CommitTran();//提交事务
                            retValue.SuccessDefalut("删除成功", 1);
                            LogWirter.Record(LogType.Doc, OpType.Delete, annex.fileName + "]", "删除 [", Convert.ToInt32(annex.createUserCode), req.createUserCode, req.createUserName);
                        }
                        catch (Exception ex)
                        {
                            db.RollbackTran();//回滚事务
                            retValue.FailDefalut($"意外错误,错误信息：{ex.Message}");
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

        /*聊天部分：
         1.发表评论,回复
         2.获取所有评论
         */

        /// <summary>
        /// 发表评论 或者 回复
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("AddComments")]
        public ResAddComments AddComments([FromBody] ReqAddComments req)
        {
            ResAddComments retValue = new ResAddComments();

            using (var db = DbContext.GetInstance("T_DOC_COMMETS"))
            {

                #region 判断必填项
                bool status = true;
                if (req.docId == 0)
                {
                    retValue.FailDefalut("请填写文件编号");
                    status = false;
                }
                else if (req.isReply == 1)
                {
                    if (req.parentCode == null)
                    {
                        retValue.FailDefalut("请填写回复项 关联编号");
                        status = false;
                    }
                    else if (string.IsNullOrEmpty(req.replyUserCode))
                    {
                        retValue.FailDefalut("请填写回复人编号");
                        status = false;
                    }
                    else if (string.IsNullOrEmpty(req.replyUserName))
                    {
                        retValue.FailDefalut("请填写回复人姓名");
                        status = false;
                    }
                    else if (req.replyUserCode == req.createUserCode)
                    {
                        retValue.FailDefalut("大哥，你自己回复自己，闹着玩呢？");
                        status = false;
                    }
                    else
                    {
                        //判断parentId
                        status = db.Queryable<DOCCOMMETSBASE>()
                        .Any(s => s.ID == req.parentCode);
                        retValue.FailDefalut("不存在该父类编号");
                    }
                }
                else if (req.isReply == 0)
                {
                    //非回复项 放置前端请求错误信息 清空
                    req.replyUserCode = null;
                    req.replyUserName = null;
                    req.parentCode = null;
                }
                else if (string.IsNullOrEmpty(req.content))
                {
                    retValue.FailDefalut("请填写回复内容");
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
                else
                {
                    //判断是否存在
                    status = db.Queryable<T_ANNEX>()
                        .Any(s => s.ID == req.docId);

                    retValue.FailDefalut("不存在该doc编号");

                }
                #endregion

                try
                {
                    if (status)
                    {
                        //设置创建时间
                        req.createTime = DateTime.Now;
                        //事务
                        try
                        {
                            db.BeginTran();//开启事务
                            var annex = db.Queryable<ANNEXBASE>().Where(s => s.ID == req.docId).FirstOrDefault();
                            db.Insert(req);

                            db.CommitTran();//提交事务
                            retValue.SuccessDefalut("评论成功", 1);
                            if (req.isReply == 0)
                            {
                                LogWirter.Record(LogType.Doc, OpType.Delete, annex.fileName + "]", "评论 [", Convert.ToInt32(annex.createUserCode), req.createUserCode, req.createUserName);
                            }
                            else
                            {
                                LogWirter.Record(LogType.Doc, OpType.Delete, annex.fileName + "]", "回复 [", Convert.ToInt32(annex.createUserCode), req.createUserCode, req.createUserName);
                            }
                        }
                        catch (Exception ex)
                        {
                            db.RollbackTran();//回滚事务
                            retValue.FailDefalut($"意外错误,错误信息：{ex.Message}");
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
        ///获取当前文章所有评论
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet("GetComments")]
        public ResGetComments GetComments([FromHeader]ReqGetComments req)
        {
            ResGetComments retValue = new ResGetComments();
            using (var db = DbContext.GetInstance("T_DOC_COMMETS"))
            {
                #region 判断必填项
                bool status = true;

                if (string.IsNullOrEmpty(req.createUserCode))
                {
                    retValue.FailDefalut("必填参数修改人编号");
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
                        var resultList = db.Queryable<DOCCOMMETSBASE>()
                       .Where(d => d.docId == req.ID)
                       .JoinTable<T_LIKE>((d, l) => d.ID == l.item_id && l.type == LikeType.Comment.ToString() &&l.create_user_code==req.createUserCode)
                       .Select<Comments>("d.*,l.item_id as isLike")
                       .ToList();

                        retValue.SuccessDefalut(resultList, resultList.Count);
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
        /// 关注与取消关注
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("Follow")]
        public ResFollow Follow([FromBody] ReqFollow req)
        {
            ResFollow retValue = new ResFollow();

            using (var db = DbContext.GetInstance("T_FOLLOW_USERS"))
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
                else if (string.IsNullOrEmpty(req.followUserCode))
                {
                    retValue.FailDefalut("请填写关注对象编号");
                    status = false;
                }
                else if (string.IsNullOrEmpty(req.followUserName))
                {
                    retValue.FailDefalut("请填写关注对象姓名");
                    status = false;
                }
                else if (req.followUserCode==req.createUserCode)
                {
                    retValue.FailDefalut("emmm...请不要自己关注自己");
                    status = false;
                }
                else
                {
                    status = db.Queryable<T_USERS>().Any(s => s.u_code == req.followUserCode);
                    retValue.FailDefalut("不存在该关注用户ID");
                }

                try
                {
                    if (status)
                    {
                        //设置创建时间
                        req.createTime = DateTime.Now;
                        //设置超时时间
                        db.CommandTimeOut = 30000;
                        //创建关注记录
                        if (req.isFollow == (int)isFollow.Yes)
                        {
                            //判断是否重复创建点赞
                            bool isExist = db.Queryable<FOLLOWUSERSBASE>().Any(s => s.followUserCode == req.followUserCode && s.createUserCode == req.createUserCode);
                            if (isExist)
                            {
                                retValue.FailDefalut("您已关注该用户！");
                            }
                            else
                            {
                                FOLLOWUSERSBASE follow = new FOLLOWUSERSBASE();

                                ReqToDBGenericClass<ReqFollow,FOLLOWUSERSBASE >.ReqToDBInstance(req, follow);
                                //事务 同步文档点赞数

                                //获取被关注用户数量
                                var followUser = db.Queryable<T_USERS>().Where(s => s.u_code == req.followUserCode).First();
                                //当前用户关注数
                                var my = db.Queryable<T_USERS>().Where(s => s.u_code == req.createUserCode).First();

                                int followedNum = followUser.followed_num + 1;
                                int followNum = my.follow_num + 1;

                                try
                                {
                                    db.BeginTran();//开启事务
                                    db.Insert(follow);
                                    //增加关注人的关注数量
                                    db.Update<T_USERS>(
                                        new
                                        {
                                            followed_num = followedNum
                                        },
                                        it => it.u_code == req.followUserCode
                                        );
                                    //增加本用户的关注数量
                                    db.Update<T_USERS>(
                                       new
                                       {
                                           follow_num = followNum
                                       },
                                       it => it.u_code == req.createUserCode
                                       );
                                    db.CommitTran();//提交事务
                                    retValue.SuccessDefalut("关注成功!", 1);
                                    LogWirter.Record(LogType.Personal, OpType.Add, "了你", "关注", Convert.ToInt32(followUser.u_code), req.createUserCode, req.createUserName);
                                }
                                catch (Exception ex)
                                {
                                    db.RollbackTran();//回滚事务
                                    retValue.FailDefalut("异常错误：" + ex.Message);
                                }


                            }
                        }
                        //取消关注
                        else if (req.isFollow == (int)isFollow.No)
                        {
                            //判断是否重复创建点赞
                            bool isExist = db.Queryable<FOLLOWUSERSBASE>().Any(s => s.followUserCode == req.followUserCode && s.createUserCode == req.createUserCode);
                            if (!isExist)
                            {
                                retValue.FailDefalut("请勿重复取消关注！");
                            }
                            else
                            {
                                FOLLOWUSERSBASE follow = new FOLLOWUSERSBASE();

                                ReqToDBGenericClass<ReqFollow, FOLLOWUSERSBASE>.ReqToDBInstance(req, follow);
                                //事务 同步文档点赞数

                                //获取被关注用户数量
                                var followUser = db.Queryable<T_USERS>().Where(s => s.u_code == req.followUserCode).First();
                                //当前用户关注数
                                var my = db.Queryable<T_USERS>().Where(s => s.u_code == req.createUserCode).First();

                                int followedNum = followUser.followed_num - 1;
                                int followNum = my.follow_num - 1;

                                try
                                {
                                    db.BeginTran();//开启事务
                                    db.Insert(follow);
                                    //减少关注人的关注数量
                                    db.Update<T_USERS>(
                                        new
                                        {
                                            followed_num = followedNum
                                        },
                                        it => it.u_code == req.followUserCode
                                        );
                                    //减少本用户的关注数量
                                    db.Update<T_USERS>(
                                       new
                                       {
                                           follow_num = followNum
                                       },
                                       it => it.u_code == req.createUserCode
                                       );
                                    db.CommitTran();//提交事务
                                    retValue.SuccessDefalut("取消关注成功!", 1);
                                }
                                catch (Exception ex)
                                {
                                    db.RollbackTran();//回滚事务
                                    retValue.FailDefalut("异常错误：" + ex.Message);
                                }


                            }
                        }
                        else
                        {
                            retValue.FailDefalut("非正常关注参数，请修改！");
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
        /// 上传文件
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            var newFile = Request.Form.Files;
            long size = newFile.Sum(f => f.Length);
            //这里可以用来限制文件大小 这里先不做限制

            //在头部获取文件类型
            var newQuery = Request.Headers.Where(it => it.Key == "annexType").FirstOrDefault();
            var val = newQuery.Value.ToString();
            //判断传参是否符合规定
            if (!Enum.GetNames(typeof(AnnexType)).Contains(val))
            {
               return Ok(new { isSuccess = false,Reason="非法文件类型" });
            }
            //利用AnnexType限制文件类型


            #region 创建路径

            string webRootPath = "/Annex/"+ val + "/" + DateTime.Now.ToString("yyyyMMdd")
                + "/";
            //当前路径为 C:\Users\luren\Desktop\603\lab-platform-back\603-Platform.API\ZC.Platform.API\bin\Debug\netcoreapp2.0\
            string baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            int index = baseDirectory.LastIndexOf('I');
            string downloadPath = baseDirectory.Substring(0, index + 1) + webRootPath;
            //存储相对路径
            string resPath = string.Empty;

            #endregion


            foreach (var formFile in newFile)
            {
                if (formFile.Length > 0)
                {
                    if (!Directory.Exists(downloadPath))
                    {
                        Directory.CreateDirectory(downloadPath);
                    }
                    string fileExt = Path.GetExtension(formFile.FileName); //文件扩展名，含有“.”
                    long fileSize = formFile.Length; //获得文件大小，以字节为单位
                    string newFileName = System.Guid.NewGuid().ToString() + fileExt; //随机生成新的文件名
                    var filePath = downloadPath + newFileName;
                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                        resPath = webRootPath + newFileName;
                    }
                    catch (Exception ex)
                    {

                        return Ok(new { isSuccess = false, Reason = ex.Message });
                    }
                   
                    
                }
            }
            return Ok(new { isSuccess = true, path = resPath });
        }

        //导出

        //分享部分

    }
}