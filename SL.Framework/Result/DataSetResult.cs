using System.Data;

namespace SL.Framework.Result
{
    /// <summary>
    /// DATASET RESULT CLASS
    /// </summary>
    public class DataSetResult : BaseResult
    {
        /// <summary>
        /// 생성자
        /// </summary>
        public DataSetResult() : base()
        {
            ResultData = null;
        }

        /// <summary>
        /// 결과객체
        /// </summary>
        public new DataSet ResultData { get; set; }
    }
}