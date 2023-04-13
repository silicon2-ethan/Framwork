using System;
using System.Configuration;
using System.Linq;

namespace SL.Framework.Config
{
    /// <summary>
    /// COMMON CONFIG CLASS
    /// </summary>
    public class CommonConfig : BaseConfig
    {
        /// <summary>
        /// SECTION NAME
        /// </summary>
        private const string SectionName = "SL.Common";

        /// <summary>
        /// SECTION CONFIG
        /// </summary>
        public static readonly CommonConfig SectionConfig;

        /// <summary>
        /// 생성자
        /// </summary>
        static CommonConfig()
        {
            SectionConfig = (CommonConfig)GetSectionConfig(SectionName);
        }
    }
}