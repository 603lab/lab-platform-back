using MyCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZC.Platform.API.BaseModel;

namespace ZC.Platform.API.Model
{
    public class HomeModel
    {
        public class ReqNoticeBase : NOTICEBOARDBASE
        {
            public int currentPage{ get; set; }
            public int pageSize { get; set; }
        }
        public class ResNoticeBase : ResponseModelBase
        {

        }

        /// <summary>
        /// Task基类
        /// </summary>
        public class ReqTaskBase : TASKBASE
        {
            public int currentPage { get; set; }
            public int pageSize { get; set; }
        }
        public class ResTaskBase : ResponseModelBase
        {

        }


        public class ResGetNoticese : ResponseModelBase<List<NOTICEBOARDBASE>>
        {

        }

        public class ResGetTasks : ResponseModelBase<List<TASKBASE>>
        {

        }

        #region 获取推荐大佬
        public class ReqGetLeaders
        {
            public string leaderType { get; set; }
            public string uCode { get; set; }
            public int currentPage { get; set; }
            public int pageSize { get; set; }
        }
        public class leader : USERSBASE
        {
            public string tag { get; set; }
            public bool isFollowed { get; set; }
        }
        public class ResGetLeaders : ResponseModelBase<List<leader>>
        {

        }

        #endregion

        #region 获取推荐文章

        public class ReqGetGoods
        {
            public string articleType { get; set; }
            public int currentPage { get; set; }
            public int pageSize { get; set; }
        }
        public class goods : ANNEXBASE
        {
            public string avatar { get; set; }
        }

        public class ResGetGoods : ResponseModelBase<List<goods>>
        {

        }
        #endregion

    }
}
