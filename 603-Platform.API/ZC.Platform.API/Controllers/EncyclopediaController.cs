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
                    var  resultList = totalList.Skip((req.currentPage - 1) * req.pageSize).Take(req.pageSize).ToList();

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
                if (string.IsNullOrEmpty(req.fileAddress))
                {
                    retValue.FailDefalut("请填写文件地址");
                    status = false;
                }
                if (string.IsNullOrEmpty(req.fileName))
                {
                    retValue.FailDefalut("请填写文件地址");
                    status = false;
                }
                //这里需要维护一个字典表 转化type code
                if (string.IsNullOrEmpty(req.type))
                {
                    retValue.FailDefalut("请填写文件类型");
                    status = false;
                }
                if (string.IsNullOrEmpty(req.createUserCode))
                {
                    retValue.FailDefalut("必填参数创建人编号");
                    status = false;
                }
                if (string.IsNullOrEmpty(req.createUserName))
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
                        retValue.SuccessDefalut("创建成功",1);
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