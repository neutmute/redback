using System.Reflection;

namespace Redback
{
    public class InvokeRequest
    {
        public ObjectTarget Target { get; set; }

        public MemberInfo MemberInfo { get; set; }

        //public TraverseSettings Settings { get; set; }

        public bool ThrowExceptions { get; set; }

    }
}