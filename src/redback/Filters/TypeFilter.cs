using System.Reflection;

namespace Redback
{
    public class TypeFilter : BaseFilter
    {
        public string Name { get; set; }
        public bool ExactMatch { get; set; }

        public TypeFilter(string pattern, bool exactMatch)
        {
            Name = pattern;
            ExactMatch = exactMatch;
        }

        public override bool IsMatch(MemberInfo memberInfo)
        {
            var queryingName = memberInfo.GetPropertyOrFieldType().Name;
            var isExact = Name == queryingName;
            var isPartialMatch = queryingName.Contains(Name) && !ExactMatch;
            return isExact || isPartialMatch;
        }

        public override string ToString()
        {
            return string.Format("Name='{0}', ExactMatch={1}", Name, ExactMatch);
        }
    }
}