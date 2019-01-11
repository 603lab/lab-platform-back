using MyCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ZC.Platform.API.Model.BaseModel;

namespace ZC.Platform.API.Model
{
    public class HomeModel
    {
        public class ReqNoticeBase : NoticeBoardBase
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
        public class ReqTaskBase : TaskBase
        {
            public int currentPage { get; set; }
            public int pageSize { get; set; }
        }
        public class ResTaskBase : ResponseModelBase
        {

        }


        public class ResGetNoticese : ResponseModelBase<List<NoticeBoardBase>>
        {

        }

        public class ResGetTasks : ResponseModelBase<List<TaskBase>>
        {

        }





    }
}
