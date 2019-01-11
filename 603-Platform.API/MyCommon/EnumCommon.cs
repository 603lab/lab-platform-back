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
    }
}
