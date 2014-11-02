using System.Reflection;

namespace Redback
{
    public class TraverseRequest
    {
        public bool HasParent { get { return Parent != null; } }

        public ParentLink Parent { get; set; }

        public ObjectTarget Target { get; set; }

        public TraversePointCallback Callback { get; set; }


        public TraverseRequest CreateChild(ObjectTarget child, MemberInfo linkedBy)
        {
            var newRequest = new TraverseRequest();
            newRequest.Target = child;
            newRequest.Callback = Callback;
            //  newRequest.Settings = Settings;
            newRequest.Parent = new ParentLink { MemberInfo = linkedBy, Request = this };
            return newRequest;
        }
    }
}