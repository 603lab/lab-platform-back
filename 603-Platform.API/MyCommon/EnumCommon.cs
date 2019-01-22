using System;

namespace MyCommon
{
    public class EnumCommon
    {
        public enum Sex : int
        {
            /// <summary>
            /// 男士
            /// </summary>
            Male = 1,
            /// <summary>
            /// 女士
            /// </summary>
            Female = 0
        }

        public enum isTop : int
        {
            /// <summary>
            /// 置顶
            /// </summary>
            Yes = 1,
            /// <summary>
            /// 非置顶
            /// </summary>
            No = 0
        }

        public enum isDone : int
        {
            /// <summary>
            /// 完成
            /// </summary>
            Yes = 1,
            /// <summary>
            /// 未完成
            /// </summary>
            No = 0
        }

        /// <summary>
        /// 点赞
        /// </summary>
        public enum isLike : int
        {
            /// <summary>
            /// 点赞
            /// </summary>
            Yes = 1,
            /// <summary>
            /// 取消赞
            /// </summary>
            No = 0
        }

        /// <summary>
        /// 是否为回答
        /// </summary>
        public enum isReply : int
        {
            /// <summary>
            /// 回复项
            /// </summary>
            Yes = 1,
            /// <summary>
            /// 非回复项
            /// </summary>
            No = 0
        }

        /// <summary>
        /// 是否关注
        /// </summary>
        public enum isFollow : int
        {
            /// <summary>
            /// 关注
            /// </summary>
            Yes = 1,
            /// <summary>
            /// 非取消关注
            /// </summary>
            No = 0
        }

        /// <summary>
        /// 收藏
        /// </summary>
        public enum isCollected : int
        {
            /// <summary>
            /// 收藏
            /// </summary>
            Yes = 1,
            /// <summary>
            /// 取消收藏
            /// </summary>
            No = 0
        }

        /// <summary>
        /// 提交报文类型
        /// </summary>
        public enum JsonDetailType 
        {
            /// <summary>
            /// 新增
            /// </summary>
            Add,
            /// <summary>
            /// 更新
            /// </summary>
            Update

        }

        /// <summary>
        /// 点赞类型
        /// </summary>
        public enum LikeType
        {
            /// <summary>
            /// doc类型的点赞
            /// </summary>
            Doc,
            /// <summary>
            /// 评论类型的点赞
            /// </summary>
            Comment
        }

    }
}
