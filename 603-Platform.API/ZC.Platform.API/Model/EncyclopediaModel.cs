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

        public class DocMenu: DOCCATLOGBASE
        {
            /// <summary>
            /// 菜单是否拥有子集
            /// </summary>
            public List<DocMenu> childrenList { get; set; }
        }

        public class ReqGetMenu : DOCCATLOGBASE
        {

        }
        public class ResGetMenu : ResponseModelBase<List<DocMenu>>
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
            public List<string> tags { get; set; }

            /// <summary>
            /// 作者名称
            /// </summary>
            public string authorName { get; set; }

            /// <summary>
            /// 文章点赞数
            /// </summary>
            public int? likeMax { get; set; }
            public int? likeMin { get; set; }

            /// <summary>
            /// 作者关注数
            /// </summary>
            public int? followMax { get; set; }
            public int? followMin { get; set; }

            public int currentPage { get; set; }
            public int pageSize { get; set; }

            public string createUserCode { get; set; }
        }

        public class SearchResult : ANNEXBASE
        {
            /// <summary>
            /// 当前用户是否已经点赞该信息
            /// </summary>
            public bool isLike { get; set; }

            /// <summary>
            /// 当前用户是否收藏当前信息
            /// </summary>
            public bool isCollected { get; set; }

            public string avatar { get; set; }
        }

        public class ResSearch : ResponseModelBase<List<SearchResult>>
        {

        }
        #endregion

        #region AddDoc
        public class ReqAddDoc : ANNEXBASE
        {
            public List<string> fileTagList { get; set; }
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

        #region GetDocConetnt

        public class DocContent : ANNEXBASE
        {
            public bool isLike { get; set; }
            public bool isCollected { get; set; }
        }

        public class ReqGetDocConetnt : ANNEXBASE
        {

        }
        public class ResGetDocConetnt : ResponseModelBase<DocContent>
        {

        }

        #endregion

        #region AddBrowseNum
        public class ReqAddBrowseNum : ANNEXBASE
        {

        }
        public class ResAddBrowseNum : ResponseModelBase
        {

        }

        #endregion

        #region UpdateDoc
        public class ReqUpdateDoc : ANNEXBASE
        {
            public List<string> fileTagList { get; set; }
        }
        public class ResUpdateDoc : ResponseModelBase
        {

        }
        #endregion

        #region GetEditHistory

        public class ReqGetEditHistory
        {
            public int ID { get; set; }
            public DateTime createTime { get; set; }
        }
        public class ResGetEditHistory : ResponseModelBase<List<EDITDETIALSBASE>>
        {

        }
        #endregion

        #region DeleteDoc

        public class ReqDeleteDoc : ANNEXBASE
        {

        }
        public class ResDeleteDoc : ResponseModelBase
        {

        }
        #endregion

        #region AddComments
        public class ReqAddComments : DOCCOMMETSBASE
        {

        }
        public class ResAddComments : ResponseModelBase
        {

        }
        #endregion

        #region GetComments

        public class Comments: DOCCOMMETSBASE
        {
            public bool isLike { get; set; }
        }

        public class ReqGetComments : DOCCOMMETSBASE
        {

        }
        public class ResGetComments : ResponseModelBase<List<Comments>>
        {

        }
        #endregion

        #region Follow
        public class ReqFollow : FOLLOWUSERSBASE
        {
            public int isFollow { get; set; }
        }
        public class ResFollow : ResponseModelBase
        {

        }

        #endregion
    }
}
