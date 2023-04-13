using SL.Framework.Extension;

namespace SL.Framework.Result
{
    /// <summary>
    /// EXCEPTION RESULT CLASS
    /// </summary>
    public class ExceptionResult : BaseResult
    {
        /// <summary>
        /// 생성자
        /// </summary>
        public ExceptionResult() : base()
        {
            ResultData = null;
        }

        /// <summary>
        /// 결과객체
        /// </summary>
        public new ExceptionExtension ResultData { get; set; }
    }
}