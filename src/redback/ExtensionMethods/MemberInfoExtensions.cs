using System;
using System.Reflection;

namespace Redback
{
    public static class MemberInfoExtensions
    {
        public static Type GetPropertyOrFieldType(this MemberInfo memberInfo)
        {

            var fieldInfo = memberInfo as FieldInfo;
            var propertyInfo = memberInfo as PropertyInfo;



            var theType = fieldInfo == null ? propertyInfo.PropertyType : fieldInfo.FieldType;
            return theType;
        }
    }
}