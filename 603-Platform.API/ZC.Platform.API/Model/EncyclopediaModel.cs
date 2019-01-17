using MyCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZC.Platform.API.BaseModel;

namespace ZC.Platform.API.Model
{
    public class EncyclopediaModel
    {


        #region GetMenu

        public class ReqGetMenu : DOCCATLOGBASE
        {

        }
        public class ResGetMenu : ResponseModelBase<List<DOCCATLOGBASE>>
        {

        }
        #endregion

        #region Search

        public class likeRange
        {
            public int? max { get; set; }
            public int? min { get; set; }
        }
        public class followRange
        {
            public int? max { get; set; }
            public int? min { get; set; }
        }
        public class ReqSearch 
        {
            /// <summary>
            /// 标签
            /// </summary>
            public List<string> taps { get; set; }

            /// <summary>
            /// 作者名称
            /// </summary>
            public string authorName { get; set; }

           /// <summary>
           /// 文章点赞数
           /// </summary>
            public likeRange likeNum { get; set; }

            /// <summary>
            /// 作者关注数
            /// </summary>
            public followRange followNum { get; set; }

            public int currentPage { get; set; }
            public int pageSize { get; set; }
        }

        public class SearchResult : ANNEXBASE
        {
            /// <summary>
            /// 当前用户是否已经点赞该信息
            /// </summary>
            public int isLike { get; set; }

            /// <summary>
            /// 当前用户是否收藏当前信息
            /// </summary>
            public int isCollected { get; set; }
        }

        public class ResSearch : ResponseModelBase<List<ANNEXBASE>>
        {

        }
        #endregion

        #region AddDoc
        public class ReqAddDoc : ANNEXBASE
        {

        }
        public class ResAddDoc : ResponseModelBase
        {

        }
        #endregion

        #region Like
        public class ReqLike : LIKEBASE
        {
            public int isLike { get; set; }
        }
        public class ResLike : ResponseModelBase
        {

        }

        #endregion


        #region Collect
        public class ReqCollect : COLLECTIONBASE
        {
            public int isCollect { get; set; }
        }
        public class ResCollect : ResponseModelBase
        {

        }

        #endregion
    }
}
