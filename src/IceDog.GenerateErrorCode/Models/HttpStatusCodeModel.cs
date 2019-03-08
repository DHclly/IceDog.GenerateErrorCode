using System;
using System.Collections.Generic;
using System.Text;

namespace IceDog.GenerateErrorCode.Models
{
    /// <summary>
    /// http状态码模型
    /// </summary>
    class HttpStatusCodeModel
    {
        /// <summary>
        /// 状态码的全名
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 状态码的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 状态码的值
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// 状态码的注释
        /// </summary>
        public string Note { get; set; }
    }
}
