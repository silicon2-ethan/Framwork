
namespace SL.Framework.Result
{
    /// <summary>
    /// BASE RESULT CLASS
    /// BASE RESULT 를 쓰시려면 DEFAULT RESULT 를 써주세요.
    /// </summary>
    public class BaseResult
    {
        /// <summary>
        /// 생성자
        /// </summary>
        public BaseResult()
        {
            ResultFlag = false;
            ResultState = -1;
            ResultMessage = null;
            ResultUrl = null;
            ResultData = null;
            ResultExtensionData = null;
        }

        /// <summary>
        /// 결과 유무
        /// </summary>
        public bool ResultFlag { get; set; }

        /// <summary>
        /// 결과 상태 / 코드
        /// </summary>
        public int ResultState { get; set; }

        /// <summary>
        /// 결과 메세지 / 결과 STRING
        /// </summary>
        public string ResultMessage { get; set; }

        /// <summary>
        /// 결과 URL
        /// </summary>
        public string ResultUrl { get; set; }

        /// <summary>
        /// 결과 객체
        /// </summary>
        public string ResultData { get; set; }

        /// <summary>
        /// 확장 결과 객체
        /// </summary>
        public object ResultExtensionData { get; set; }
    }
}