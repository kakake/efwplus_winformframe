using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace EFWCoreLib.CoreFrame.Plugin
{


    public class PluginSectionHandler:ConfigurationSection
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public String name
        {
            get
            { return (String)this["name"]; }
            set
            { this["name"] = value; }
        }

        [ConfigurationProperty("version", IsRequired = true)]
        public String version
        {
            get
            { return (String)this["version"]; }
            set
            { this["version"] = value; }
        }

        [ConfigurationProperty("title", IsRequired = true)]
        public String title
        {
            get
            { return (String)this["title"]; }
            set
            { this["title"] = value; }
        }

        [ConfigurationProperty("author", IsRequired = true)]
        public String author
        {
            get
            { return (String)this["author"]; }
            set
            { this["author"] = value; }
        }

        [ConfigurationProperty("plugintype", IsRequired = true)]
        public String plugintype
        {
            get
            { return (String)this["plugintype"]; }
            set
            { this["plugintype"] = value; }
        }

        [ConfigurationProperty("defaultdbkey", IsRequired = true)]
        public String defaultdbkey
        {
            get
            { return (String)this["defaultdbkey"]; }
            set
            { this["defaultdbkey"] = value; }
        }

        [ConfigurationProperty("defaultcachekey", IsRequired = true)]
        public String defaultcachekey
        {
            get
            { return (String)this["defaultcachekey"]; }
            set
            { this["defaultcachekey"] = value; }
        }

        [ConfigurationProperty("isentry")]
        public string isentry
        {
            get
            { return (String)this["isentry"]; }
            set
            { this["isentry"] = value; }
        }

        [ConfigurationProperty("baseinfo", IsDefaultCollection = true)]
        public BaseInfoCollection baseinfo
        {
            get
            {
                return (BaseInfoCollection)base["baseinfo"];
            }
        }

        [ConfigurationProperty("businessinfo", IsDefaultCollection = true)]
        public BusinessInfoCollection businessinfo
        {
            get
            {
                return (BusinessInfoCollection)base["businessinfo"];
            }
        }

        [ConfigurationProperty("issue", IsDefaultCollection = true)]
        public issueCollection issue
        {
            get
            {
                return (issueCollection)base["issue"];
            }
        }

        [ConfigurationProperty("setup", IsDefaultCollection = true)]
        public setupCollection setup
        {
            get
            {
                return (setupCollection)base["setup"];
            }
        }

        [ConfigurationProperty("menus", IsDefaultCollection = true)]
        public menuCollection menus
        {
            get
            {
                return (menuCollection)base["menus"];
            }
        }
    }

    public class BaseInfoCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new baseinfoData();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((baseinfoData)element).key;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override string ElementName
        {
            get
            {
                return "data";
            }
        }

        public baseinfoData this[int index]
        {
            get
            {
                return (baseinfoData)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);

            }
        }
    }

    public class BusinessInfoCollection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new businessinfoDll();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((businessinfoDll)element).name;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override string ElementName
        {
            get
            {
                return "dll";
            }
        }

        public businessinfoDll this[int index]
        {
            get
            {
                return (businessinfoDll)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }
    }

    public class issueCollection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new issueadd();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((issueadd)element).path;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override string ElementName
        {
            get
            {
                return "add";
            }
        }

        public issueadd this[int index]
        {
            get
            {
                return (issueadd)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }
    }

    public class setupCollection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new setupadd();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((setupadd)element).path;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override string ElementName
        {
            get
            {
                return "add";
            }
        }

        public setupadd this[int index]
        {
            get
            {
                return (setupadd)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }
    }

    public class menuCollection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new menuadd();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((menuadd)element).menuname;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override string ElementName
        {
            get
            {
                return "add";
            }
        }

        public menuadd this[int index]
        {
            get
            {
                return (menuadd)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }
    }

    public class baseinfoData : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string key
        {
            get
            {
                return (string)base["key"];
            }
            set
            {
                base["key"] = value;
            }
        }
        [ConfigurationProperty("value")]
        public string value
        {
            get
            {
                return (string)base["value"];
            }
            set
            {
                base["value"] = value;
            }
        }
    }

    public class businessinfoDll : ConfigurationElement
    {
        /// <summary>
        /// 控制器名称
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string name
        {
            get
            {
                return (string)base["name"];
            }
            set
            {
                base["name"] = value;
            }
        }
        [ConfigurationProperty("version")]
        public string version
        {
            get
            {
                return (string)base["version"];
            }
            set
            {
                base["version"] = value;
            }
        }
    }

    public class issueadd : ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true)]
        public string type
        {
            get
            {
                return (string)base["type"];
            }
            set
            {
                base["type"] = value;
            }
        }
        [ConfigurationProperty("path", IsRequired = true)]
        public string path
        {
            get
            {
                return (string)base["path"];
            }
            set
            {
                base["path"] = value;
            }
        }

        [ConfigurationProperty("source")]
        public string source
        {
            get
            {
                return (string)base["source"];
            }
            set
            {
                base["source"] = value;
            }
        }
    }

    public class setupadd : ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true)]
        public string type
        {
            get
            {
                return (string)base["type"];
            }
            set
            {
                base["type"] = value;
            }
        }
        [ConfigurationProperty("path", IsRequired = true)]
        public string path
        {
            get
            {
                return (string)base["path"];
            }
            set
            {
                base["path"] = value;
            }
        }

        [ConfigurationProperty("copyto")]
        public string copyto
        {
            get
            {
                return (string)base["copyto"];
            }
            set
            {
                base["copyto"] = value;
            }
        }
    }

    public class menuadd : ConfigurationElement
    {
        [ConfigurationProperty("menuname")]
        public string menuname
        {
            get
            {
                return (string)base["menuname"];
            }
            set
            {
                base["menuname"] = value;
            }
        }
        [ConfigurationProperty("pluginname")]
        public string pluginname
        {
            get
            {
                return (string)base["pluginname"];
            }
            set
            {
                base["pluginname"] = value;
            }
        }

        [ConfigurationProperty("controllername")]
        public string controllername
        {
            get
            {
                return (string)base["controllername"];
            }
            set
            {
                base["controllername"] = value;
            }
        }

        [ConfigurationProperty("viewname")]
        public string viewname
        {
            get
            {
                return (string)base["viewname"];
            }
            set
            {
                base["viewname"] = value;
            }
        }

        [ConfigurationProperty("menupath")]
        public string menupath
        {
            get
            {
                return (string)base["menupath"];
            }
            set
            {
                base["menupath"] = value;
            }
        }

        [ConfigurationProperty("memo")]
        public string memo
        {
            get
            {
                return (string)base["memo"];
            }
            set
            {
                base["memo"] = value;
            }
        }
    }


}
