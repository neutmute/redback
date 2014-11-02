namespace Redback
{
    public class HitTarget : ObjectTarget
    {
        public Enumerations HitType { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            if (HitType == Enumerations.Member)
            {
                return string.Format(".{0}={1}", Name, base.ToString());
            }
            return string.Format("Class.ToString()={0}", base.ToString());
        }
    }
}