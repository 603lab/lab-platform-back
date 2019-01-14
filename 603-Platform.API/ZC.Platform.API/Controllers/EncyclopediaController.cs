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
                    var resultList = annexList.ToList();

                    retValue.SuccessDefalut(resultList, resultList.Count);

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