using System.Reflection;

namespace Redback
{
    public abstract class BaseFilter
    {
        public abstract bool IsMatch(MemberInfo memberInfo);


    }
}