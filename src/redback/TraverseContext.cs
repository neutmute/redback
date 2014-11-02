using System.Collections;
using System.Collections.Generic;

namespace Redback
{
    public class TraverseContext
    {
        public int Depth { get; set; }

        public HashSet<object> ExploredObjects { get; set; }

        public MemberInfoList TargetMembers { get; set; }

        public IEnumerable TargetAsEnumerable { get; set; }

        public bool IsContainedInEnumerable { get; set; }

        public bool IsContainedInList { get; set; }

        public int ListIndex { get; set; }

        public TraverseContext()
        {
            ExploredObjects = new HashSet<object>();
        }

        public TraverseContext Clone()
        {
            var point = new TraverseContext();
            point.Depth = Depth;
            ExploredObjects = ExploredObjects;
            return point;
        }

        public void RecordExplored(TraverseRequest request)
        {
            ExploredObjects.Add(request.Target);
            //if (ExploredObjects.Contains(targetObject))
            //{
            //    Log.Trace(m => m("Have already seen {0}", targetObject));
            //    return;
            //}
        }

        public bool ShouldTerminate(TraverseSettings settings)
        {
            return Depth >= settings.MaximumDepth;
        }
    }
}