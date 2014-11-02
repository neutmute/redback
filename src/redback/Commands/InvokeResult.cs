using System.Reflection;

namespace Redback
{
    public class InvokeResult : ObjectTarget
    {
        public MemberInfo MemberInfo { get; set; }

        public bool WasExceptionSwallowed { get; set; }

        public HitTarget ToHitTarget(string name)
        {
            var hit = new HitTarget();
            hit.Name = name;
            hit.Type = Type;
            hit.Value = Value;
            return hit;
        }
    }
}