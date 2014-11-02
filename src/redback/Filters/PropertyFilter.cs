using System.Reflection;

namespace Redback
{
    public class PropertyFilter : BaseFilter
    {
        public string Name { get; set; }

        public PropertyFilter()
        {

        }
        public PropertyFilter(string name)
        {
            Name = name;
        }

        public override bool IsMatch(MemberInfo memberInfo)
        {
            var queryingName = memberInfo.Name;
            var isExact = Name == queryingName;
            return isExact;
        }

        public override string ToString()
        {
            return string.Format("Name='{0}'", Name);
        }
    }
}