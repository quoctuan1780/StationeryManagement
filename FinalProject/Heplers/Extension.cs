using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FinalProject.Heplers
{
    public static class Extension
    {
        public static Dictionary<string, string> AsParams(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name.ToLower(),
                propInfo => propInfo.GetValue(source, null)?.ToString()
            );
        }

            public static object GetOrDefault(this Dictionary<string, object> source, string key, object defaultValue)
            {
                return source.ContainsKey(key) ? source[key] : defaultValue;
            }
        
    }
}
