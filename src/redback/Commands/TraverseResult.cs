namespace Redback
{
    public class TraverseResult
    {
        public ParentLink Parent { get; set; }

        public ObjectTarget Owner { get; set; }

        public HitTarget Target { get; set; }

        public override string ToString()
        {
            var text = string.Format("Parent=[{0}], Owner.Type='{1}'", Parent, Owner.Type.Name);
            if (Target != null)
            {
                text += string.Format(" {0}", Target);
            }
            else
            {
                text += string.Format("ToString=?");
            }
            return text;
        }
    }
}