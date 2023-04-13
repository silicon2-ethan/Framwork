using System;

namespace SL.Framework.Result
{
    /// <summary>
    /// OBJECT RESULT CLASS
    /// </summary>
    public class ObjectResult : BaseResult
    {
        /// <summary>
        /// 생성자
        /// </summary>
        public ObjectResult() : base()
        {
            ResultData = null;
        }

        /// <summary>
        /// 결과객체
        /// </summary>
        public new Object ResultData { get; set; }
    }
}