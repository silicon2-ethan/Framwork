
namespace SL.Framework.Config
{
    /// <summary>
    /// BIZ CONFIG CLASS
    /// </summary>
    public class BizConfig : BaseConfig
    {
        /// <summary>
        /// SECTION NAME
        /// </summary>
        private const string SectionName = "SL.Biz";

        /// <summary>
        /// SECTION CONFIG
        /// </summary>
        public static readonly BizConfig SectionConfig;

        /// <summary>
        /// 생성자
        /// </summary>
        static BizConfig()
        {
            SectionConfig = (BizConfig) GetSectionConfig(SectionName);
        }
    }
}