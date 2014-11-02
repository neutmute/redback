using System.Reflection;

namespace Redback
{
    public class ParentLink
    {
        public TraverseRequest Request { get; set; }

        public MemberInfo MemberInfo { get; set; }

        public bool IsEnumerable { get; set; }
        public bool IsListEnumerable { get; set; }
        public int ListIndex { get; set; }

        public bool IsLinkedByMember { get { return MemberInfo != null; } }

        public override string ToString()
        {

            var text = string.Empty;

            if (Request.HasParent)
            {
                text = Request.Parent.ToString();
            }

            var MemberInfoText = IsLinkedByMember ? MemberInfo.Name : "[?]";


            if (IsEnumerable)
            {
                if (IsListEnumerable)
                {
                    text += string.Format("{0}[{1}]", Request.Target.Type.Name, ListIndex);
                }
                else
                {
                    text += string.Format("{0}[??]", Request.Target.Type.Name);
                }
            }
            else
            {
                text += string.Format("{0}->{1}", Request.Target.Type.Name, MemberInfoText);
            }


            return text;
        }
    }
}