using System.Configuration;

namespace SL.Framework.Config
{
    /// <inheritdoc />
    /// <summary>
    /// BASE CONFIG CLASS
    /// </summary>
    public class BaseConfig : ConfigurationSection
    {
        protected static object GetSectionConfig(string sectionName)
        {
            return ConfigurationManager.GetSection(sectionName);
        }

        public DefaultElementCollection GetElementCollection(string sectionName)
        {
            return (DefaultElementCollection) this[sectionName];
        }

        public DefaultElement GetElement(string sectionName)
        {
            return (DefaultElement) this[sectionName];
        }
    }

    /// <summary>
    /// DEFAULT ELEMENT COLLECTION CLASS
    /// </summary>
    [ConfigurationCollection(typeof (DefaultElement))]
    public class DefaultElementCollection : ConfigurationElementCollection
    {
        public DefaultElement this[int index]
        {
            get { return (DefaultElement) BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }

        public new DefaultElement this[string name]
        {
            get { return (DefaultElement) BaseGet(name); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new DefaultElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DefaultElement) element).Name;
        }
    }

    /// <summary>
    /// DEFAULT ELEMENT CLASS
    /// </summary>
    public class DefaultElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "", IsRequired = false)]
        public string Name
        {
            get { return (string) this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("value", DefaultValue = "", IsRequired = false)]
        public string Value
        {
            get { return (string) this["value"]; }
            set { this["value"] = value; }
        }

        [ConfigurationProperty("default", DefaultValue = "", IsRequired = false)]
        public string Default
        {
            get { return (string) this["default"]; }
            set { this["default"] = value; }
        }

        [ConfigurationProperty("defaultpath", DefaultValue = "", IsRequired = false)]
        public string DefaultPath
        {
            get { return (string) this["defaultpath"]; }
            set { this["defaultpath"] = value; }
        }

        [ConfigurationProperty("temppath", DefaultValue = "", IsRequired = false)]
        public string TempPath
        {
            get { return (string) this["temppath"]; }
            set { this["temppath"] = value; }
        }

        [ConfigurationProperty("tempurl", DefaultValue = "", IsRequired = false)]
        public string TempUrl
        {
            get { return (string) this["tempurl"]; }
            set { this["tempurl"] = value; }
        }

        [ConfigurationProperty("path", DefaultValue = "", IsRequired = false)]
        public string Path
        {
            get { return (string) this["path"]; }
            set { this["path"] = value; }
        }

        [ConfigurationProperty("url", DefaultValue = "", IsRequired = false)]
        public string Url
        {
            get { return (string) this["url"]; }
            set { this["url"] = value; }
        }

        [ConfigurationProperty("src", DefaultValue = "", IsRequired = false)]
        public string Src
        {
            get { return (string) this["src"]; }
            set { this["src"] = value; }
        }

        [ConfigurationProperty("extension", DefaultValue = "", IsRequired = false)]
        public string Extension
        {
            get { return (string) this["extension"]; }
            set { this["extension"] = value; }
        }

        [ConfigurationProperty("id", DefaultValue = "", IsRequired = false)]
        public string Id
        {
            get { return (string) this["id"]; }
            set { this["id"] = value; }
        }

        [ConfigurationProperty("password", DefaultValue = "", IsRequired = false)]
        public string Password
        {
            get { return (string) this["password"]; }
            set { this["password"] = value; }
        }

        [ConfigurationProperty("ip", DefaultValue = "", IsRequired = false)]
        public string Ip
        {
            get { return (string) this["ip"]; }
            set { this["ip"] = value; }
        }

        [ConfigurationProperty("server", DefaultValue = "", IsRequired = false)]
        public string Server
        {
            get { return (string) this["server"]; }
            set { this["server"] = value; }
        }

        [ConfigurationProperty("host", DefaultValue = "", IsRequired = false)]
        public string Host
        {
            get { return (string) this["host"]; }
            set { this["host"] = value; }
        }

        [ConfigurationProperty("port", DefaultValue = "", IsRequired = false)]
        public string Port
        {
            get { return (string) this["port"]; }
            set { this["port"] = value; }
        }

        [ConfigurationProperty("username", DefaultValue = "", IsRequired = false)]
        public string UserName
        {
            get { return (string) this["username"]; }
            set { this["username"] = value; }
        }

        [ConfigurationProperty("adds")]
        public DefaultElementCollection Adds
        {
            get { return (DefaultElementCollection) this["adds"]; }
        }


        [ConfigurationProperty("encript", DefaultValue = "", IsRequired = false)]
        public string Encript
        {
            get { return (string)this["encript"]; }
            set { this["encript"] = value; }
        }
    }
}