using NVelocity.App;
using NVelocity.Context;
using Commons.Collections;
using NVelocity.Runtime;
using NVelocity;
using System.IO;

namespace EFWCoreLib.CoreFrame.Common
{
    /// <summary>
    /// 针对NVelocity封装
    /// </summary>
    public class TemplateHelper
    {
        private VelocityEngine velocity = null;
        private IContext context = null;

        public TemplateHelper(string templatePath)
        {
            velocity = new VelocityEngine();

            //使用设置初始化VelocityEngine
            ExtendedProperties props = new ExtendedProperties();

            props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, templatePath);
            props.AddProperty(RuntimeConstants.INPUT_ENCODING, "utf-8");

            velocity.Init(props);
            //RuntimeConstants.RESOURCE_MANAGER_CLASS 
            //为模板变量赋值
            context = new VelocityContext();

        }

        /// <summary>
        /// 给模板变量赋值
        /// </summary>
        /// <param name="key">模板变量</param>
        /// <param name="value">模板变量值</param>
        public void Put(string key, object value)
        {
            context.Put(key, value);
        }

        /// <summary>
        /// 生成字符
        /// </summary>
        /// <param name="templatFileName">模板文件名</param>
        public string BuildString(string templateFile)
        {
            //从文件中读取模板
            Template template = velocity.GetTemplate(templateFile);

            //合并模板
            StringWriter writer = new StringWriter();
            template.Merge(context, writer);
            return writer.ToString();
        }

        public bool ContainsKey(string keyName)
        {
            return context.ContainsKey(keyName);
        }

    }
}
 
