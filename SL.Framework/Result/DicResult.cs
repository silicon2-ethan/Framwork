using System.Collections.Generic;

namespace SL.Framework.Result
{
    /// <summary>
    /// DICTIONARY RESULT CLASS
    /// </summary>
    public class DicResult : BaseResult
    {
        /// <summary>
        /// 생성자
        /// </summary>
        public DicResult()
            : base()
        {
            ResultData = null;
        }

        /// <summary>
        /// 결과객체
        /// </summary>
        public new Dictionary<string, object> ResultData { get; set; }
    }
}