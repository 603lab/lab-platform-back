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

        public enum isMenu : int
        {
            /// <summary>
            /// 是菜单
            /// </summary>
            Yes = 1,
            /// <summary>
            /// 非菜单
            /// </summary>
            No = 0
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

        /// <summary>
        /// 日志记录类型
        /// </summary>
        public enum LogType
        {
            /// <summary>
            /// 登录信息
            /// </summary>
            Login,
            /// <summary>
            /// 百科系统
            /// </summary>
            Doc,
            /// <summary>
            /// 项目相关
            /// </summary>
            Project,
            /// <summary>
            /// 管理员
            /// </summary>
            Admin,
            /// <summary>
            /// 个人
            /// </summary>
            Personal,
            /// <summary>
            /// 创建菜单
            /// </summary>
            Menu
        }

        /// <summary>
        /// 操作类型
        /// </summary>
        public enum OpType
        {
            /// <summary>
            /// 登录
            /// </summary>
            Login,
            /// <summary>
            /// 登出
            /// </summary>
            Out,
            /// <summary>
            /// 新建
            /// </summary>
            Add,
            /// <summary>
            /// 删除
            /// </summary>
            Delete,
            /// <summary>
            /// 编辑
            /// </summary>
            Update



            
        }

        public enum ActiveType
        {

            /// <summary>
            /// 百科动态
            /// </summary>
            Doc,
            /// <summary>
            /// 项目动态
            /// </summary>
            Project,
            /// <summary>
            /// 论坛动态
            /// </summary>
            Forum,
            /// <summary>
            /// 关注动态
            /// </summary>
            Follow

        }

        public enum AnnexType
        {
            /// <summary>
            /// 头像文件
            /// </summary>
            Avatar,
            /// <summary>
            /// pdf文件
            /// </summary>
            PDF,
            /// <summary>
            /// 文档
            /// </summary>
            Doc

        }
    }
}
