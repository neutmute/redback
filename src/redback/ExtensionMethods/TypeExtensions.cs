using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redback.ExtensionMethods
{

    /// <summary>
    /// Returns 'System.Nullable&lt;int&gt;' say instead of 'System.Nullable`'
    /// </summary>
    /// <remarks>http://stackoverflow.com/questions/2448800/given-a-type-instance-how-to-get-generic-type-name-in-c/2448918#2448918</remarks>
    public static class TypeExtensions
    {
        public static string ToGenericTypeString(this Type t)
        {
            if (!t.IsGenericType)
                return t.Name;
            string genericTypeName = t.GetGenericTypeDefinition().Name;
            genericTypeName = genericTypeName.Substring(0,
                genericTypeName.IndexOf('`'));
            string genericArgs = string.Join(",",
                t.GetGenericArguments()
                    .Select(ta => ToGenericTypeString(ta)).ToArray());
            return genericTypeName + "<" + genericArgs + ">";
        }
    }
}
