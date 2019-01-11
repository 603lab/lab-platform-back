using System;
using System.Collections.Generic;
using System.Text;

namespace MyCommon
{
    /// <summary>
    /// 返回类基类
    /// </summary>
    public class ResponseModelBase
    {
        public bool isSuccess { get; set; } = true;

        /// <summary>
        /// 返回数据数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; } = "调用成功！";

        /// <summary>
        /// 状态码
        /// </summary>
        public int statusCode { get; set; } = 200;

        public void SuccessDefalut(int totalCount)
        {
            statusCode = 200;
            isSuccess = true;
            Message = "调用成功!";
            Count = totalCount;
        }
        public void SuccessDefalut(string message,int totalCount)
        {
            statusCode = 200;
            isSuccess = true;
            Message = message;
            Count = totalCount;
        }
        public void FailDefalut(Exception ex)
        {
            statusCode = -100;
            isSuccess = false;
            Message = $"返回失败失败,错误信息:{ex.Message}";
            Count = 0;
        }
        public void FailDefalut()
        {
            statusCode = -100;
            isSuccess = false;
            Message = $"不存在改信息";
            Count = 0;
        }
        public void FailDefalut(string message)
        {
            statusCode = -100;
            isSuccess = false;
            Message = message;
            Count = 0;
        }
    }

   

    /// <summary>
    /// 返回基类
    /// </summary>
    public class ResponseModelBase<T> where T : class
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool isSuccess { get; set; } = true;

        /// <summary>
        /// 返回数据数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; } = "调用成功！";
        /// <summary>
        /// 状态码
        /// </summary>
        public int statusCode { get; set; } = 200;
        /// <summary>
        /// 数据集
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 默认成功返回内容
        /// </summary>
        /// <param name="Data"></param>
        public void SuccessDefalut(T TData, int totalCount)
        {
            statusCode = 200;
            isSuccess = true;
            Message = "返回成功";
            Count = totalCount;
            Data = TData;
        }

        /// <summary>
        /// 如果为空时候需要返回错误的
        /// </summary>
        /// <param name="TData"></param>
        /// <param name="totalCount"></param>
        /// <param name="isNullReason"></param>
        public virtual void SuccessDefalut(T TData, int totalCount,string isNullReason)
        {
            if (TData != null)
            {
                statusCode = 200;
                isSuccess = true;
                Message = "返回成功";
                Count = totalCount;
            }
            else
            {
                statusCode = -100;
                isSuccess = false;
                Message = isNullReason;
                Count = 0;
            }
            Data = TData;
        }

        /// <summary>
        /// 默认失败返回方法
        /// </summary>
        /// <param name="ex"></param>
        public void FailDefalut(Exception ex)
        {
            statusCode = -100;
            isSuccess = false;
            //不一定是0
            Count = 0;
            Message = ex.Message;

        }
    }


}
