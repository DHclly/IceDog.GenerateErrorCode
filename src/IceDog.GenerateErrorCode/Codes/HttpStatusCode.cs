using System;
using System.Collections.Generic;
using System.Text;

namespace IceDog.GenerateErrorCode.Codes
{

    /// <summary>
    /// 状态码类，存储状态码常量，这里的常量是http的状态码，用于反射解析
    /// </summary>
    public class HttpStatusCode
    {
        /// <summary>
        /// 客户端应当继续发送请求。这个临时响应是用来通知客户端它的部分请求已经被服务器接收，且仍未被拒绝。
        /// </summary>
        public const int Continue = 100;
        /// <summary>
        /// 代表处理将被继续执行。
        /// </summary>
        public const int Processing = 102;
        /// <summary>
        /// 请求已成功，请求所希望的响应头或数据体将随此响应返回。
        /// </summary>
        public const int Ok = 200;
        /// <summary>
        /// 请求已经被实现，而且有一个新的资源已经依据请求的需要而创建，且其URI已经随Location头信息返回。
        /// </summary>
        public const int Created = 201;
        /// <summary>
        /// 服务器已接受请求，但尚未处理。
        /// </summary>
        public const int Accepted = 202;
    }
}
