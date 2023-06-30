using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;


namespace SL.Framework.Config
{
    /// <summary>
    /// WEB CONFIG CLASS
    /// </summary>
    public class WebConfig : BaseConfig
    {
        /// <summary>
        /// SECTION NAME
        /// </summary>
        private const string SectionName = "SL.Web";

        /// <summary>
        /// SECTION CONFIG
        /// </summary>
        public static readonly WebConfig SectionConfig;

        /// <summary>
        /// 생성자
        /// </summary>
        static WebConfig()
        {
            SectionConfig = (WebConfig)GetSectionConfig(SectionName);
        }

        /// <summary>
        /// SECURITYKEYINFO ELEMENT COLLECTION
        /// </summary>
        [ConfigurationProperty("SiteSetting")]
        public DefaultElementCollection SiteSettingInfo
        {
            get { return (DefaultElementCollection)this["SiteSetting"]; }
        }

        /// <summary>
        /// SECURITYKEYINFO ELEMENT COLLECTION
        /// </summary>
        [ConfigurationProperty("ApprSetting")]
        public DefaultElementCollection ApprSettingInfo
        {
            get { return (DefaultElementCollection)this["ApprSetting"]; }
        }


        /// <summary>
        /// 웹사이트의 타입을 불러온다.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Test,
        /// Stagging,
        /// Service
        /// </remarks>
        public static string WebServerType
        {
            get
            {
                var result = string.Empty;
#if(DEBUG)
                if (SectionConfig == null) return "Test"; //웹환경이 아닌 경우 처리
#else
                if (SectionConfig == null) return "Real"; //웹환경이 아닌 경우 처리
#endif
                var item = SectionConfig.SiteSettingInfo.Cast<DefaultElement>().FirstOrDefault(x => x.Name.Equals("WebServerType", StringComparison.OrdinalIgnoreCase));
                if (item != null)
                    result = item.Value;
                return result;
            }
        }


        #region [ApprSetting]
        public static string RDA_CA
        {
            get
            {
                var result = string.Empty;
                var item = SectionConfig.ApprSettingInfo.Cast<DefaultElement>().FirstOrDefault(x => x.Name.Equals("RDA_CA", StringComparison.OrdinalIgnoreCase));
                if (item != null)
                    result = item.Value;
                return result;
            }
        }
        public static string RDA_PA
        {
            get
            {
                var result = string.Empty;
                var item = SectionConfig.ApprSettingInfo.Cast<DefaultElement>().FirstOrDefault(x => x.Name.Equals("RDA_PA", StringComparison.OrdinalIgnoreCase));
                if (item != null)
                    result = item.Value;
                return result;
            }
        }
        public static string RDA_DS
        {
            get
            {
                var result = string.Empty;
                var item = SectionConfig.ApprSettingInfo.Cast<DefaultElement>().FirstOrDefault(x => x.Name.Equals("RDA_DS", StringComparison.OrdinalIgnoreCase));
                if (item != null)
                    result = item.Value;
                return result;
            }
        }
        public static string RDA_ETC
        {
            get
            {
                var result = string.Empty;
                var item = SectionConfig.ApprSettingInfo.Cast<DefaultElement>().FirstOrDefault(x => x.Name.Equals("RDA_ETC", StringComparison.OrdinalIgnoreCase));
                if (item != null)
                    result = item.Value;
                return result;
            }
        }
        public static string RDA_ST
        {
            get
            {
                var result = string.Empty;
                var item = SectionConfig.ApprSettingInfo.Cast<DefaultElement>().FirstOrDefault(x => x.Name.Equals("RDA_ST", StringComparison.OrdinalIgnoreCase));
                if (item != null)
                    result = item.Value;
                return result;
            }
        }
        public static string RDA_IT
        {
            get
            {
                var result = string.Empty;
                var item = SectionConfig.ApprSettingInfo.Cast<DefaultElement>().FirstOrDefault(x => x.Name.Equals("RDA_IT", StringComparison.OrdinalIgnoreCase));
                if (item != null)
                    result = item.Value;
                return result;
            }
        }

        public static string RDA_CU
        {
            get
            {
                var result = string.Empty;
                var item = SectionConfig.ApprSettingInfo.Cast<DefaultElement>().FirstOrDefault(x => x.Name.Equals("RDA_CU", StringComparison.OrdinalIgnoreCase));
                if (item != null)
                    result = item.Value;
                return result;
            }
        }

        public static string IP_GREATER
        {
            get
            {
                var result = string.Empty;
                var item = SectionConfig.ApprSettingInfo.Cast<DefaultElement>().FirstOrDefault(x => x.Name.Equals("IP_GREATER", StringComparison.OrdinalIgnoreCase));
                if (item != null)
                    result = item.Value;
                return result;
            }
        }

        public static string IP_GREATER_2
        {
            get
            {
                var result = string.Empty;
                var item = SectionConfig.ApprSettingInfo.Cast<DefaultElement>().FirstOrDefault(x => x.Name.Equals("IP_GREATER_2", StringComparison.OrdinalIgnoreCase));
                if (item != null)
                    result = item.Value;
                return result;
            }
        }

        public static string IP_RESS
        {
            get
            {
                var result = string.Empty;
                var item = SectionConfig.ApprSettingInfo.Cast<DefaultElement>().FirstOrDefault(x => x.Name.Equals("IP_RESS", StringComparison.OrdinalIgnoreCase));
                if (item != null)
                    result = item.Value;
                return result;
            }
        }

        public static string IAP_GREATER
        {
            get
            {
                var result = string.Empty;
                var item = SectionConfig.ApprSettingInfo.Cast<DefaultElement>().FirstOrDefault(x => x.Name.Equals("IAP_GREATER", StringComparison.OrdinalIgnoreCase));
                if (item != null)
                    result = item.Value;
                return result;
            }
        }

        public static string IAP_GREATER_2
        {
            get
            {
                var result = string.Empty;
                var item = SectionConfig.ApprSettingInfo.Cast<DefaultElement>().FirstOrDefault(x => x.Name.Equals("IAP_GREATER_2", StringComparison.OrdinalIgnoreCase));
                if (item != null)
                    result = item.Value;
                return result;
            }
        }

        public static string IAP_RESS
        {
            get
            {
                var result = string.Empty;
                var item = SectionConfig.ApprSettingInfo.Cast<DefaultElement>().FirstOrDefault(x => x.Name.Equals("IAP_RESS", StringComparison.OrdinalIgnoreCase));
                if (item != null)
                    result = item.Value;
                return result;
            }
        }

        public static string PLB_1
        {
            get
            {
                var result = string.Empty;
                var item = SectionConfig.ApprSettingInfo.Cast<DefaultElement>().FirstOrDefault(x => x.Name.Equals("PLB_1", StringComparison.OrdinalIgnoreCase));
                if (item != null)
                    result = item.Value;
                return result;
            }
        }

        public static string PLB_2
        {
            get
            {
                var result = string.Empty;
                var item = SectionConfig.ApprSettingInfo.Cast<DefaultElement>().FirstOrDefault(x => x.Name.Equals("PLB_2", StringComparison.OrdinalIgnoreCase));
                if (item != null)
                    result = item.Value;
                return result;
            }
        }
        #endregion

    }
}