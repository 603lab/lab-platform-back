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





    }
}
