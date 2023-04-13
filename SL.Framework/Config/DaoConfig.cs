using SL.Framework.Utility;
using System;
using System.Configuration;
using System.Linq;

namespace SL.Framework.Config
{
    /// <summary>
    /// DAO CONFIG CLASS
    /// </summary>
    public class DaoConfig : BaseConfig
    {
        /// <summary>
        /// SECTION NAME
        /// </summary>
        private const string SectionName = "SL.Dao";

        /// <summary>
        /// SECTION CONFIG
        /// </summary>
        public static readonly DaoConfig SectionConfig;

        /// <summary>
        /// 생성자
        /// </summary>
        static DaoConfig()
        {
            SectionConfig = (DaoConfig) GetSectionConfig(SectionName);
        }

        /// <summary>
        /// CONNECTION STRING
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string DbConnectionString(string name)
        {
            var result = string.Empty;
            foreach (
                var item in
                    SectionConfig.Connections.Cast<DefaultElement>()
                        .Where(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                if (item.Encript.vIsEmpty())
                    result = item.Value;
                else
                {
                    SL.Framework.Extension.AESEncapsulationExtension testAes = new SL.Framework.Extension.AESEncapsulationExtension();
                    result = testAes.Decrypt(item.Encript);
                }
            }
            return result;
        }

        /// <summary>
        /// CONNECTION ELEMENT COLLECTION
        /// </summary>
        [ConfigurationProperty("connectioninfo")]
        public DefaultElementCollection Connections
        {
            get { return (DefaultElementCollection) this["connectioninfo"]; }
        }


        /// <summary>
        /// CONNECTION ELEMENT COLLECTION
        /// </summary>
        [ConfigurationProperty("WrapperInfo")]
        public DefaultElementCollection Mapper
        {
            get { return (DefaultElementCollection)this["WrapperInfo"]; }
        }

        /// <summary>
        /// service mapper
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string MapXML(string name)
        {
            var result = string.Empty;
            foreach (var item in SectionConfig.Mapper.Cast<DefaultElement>().Where(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                result = item.Value;
            }
            return result;
        }
    }
}