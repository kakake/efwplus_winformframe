using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace EFWCoreLib.CoreFrame.Init.AttributeManager
{
    public abstract class AbstractAttributeManager
    {
        //public abstract static void LoadAttribute(List<string> BusinessDll, ICacheManager cache);

        public static Object GetAttributeInfo(ICacheManager cache)
        {
            return cache.GetData(GetCacheKey());
        }

        public static void ClearAttributeData(ICacheManager cache)
        {
            cache.Remove(GetCacheKey());
        }

        public abstract static string GetCacheKey();
    }
}
