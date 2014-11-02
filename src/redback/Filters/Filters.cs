using System.Collections.Generic;
using System.Reflection;

namespace Redback
{
    public class Filters : List<BaseFilter>
    {
        public bool IsExcluded(MemberInfo memberInfo)
        {
            return Exists(f => f.IsMatch(memberInfo));
        }
    }
}