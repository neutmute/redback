using System.Reflection;

namespace Redback
{
    public class TraverseSettings
    {
        public int MaximumDepth { get; set; }

        public BindingFlags BindingFlags
        {
            get
            {
                BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField |
                                     BindingFlags.GetProperty;

                if (BindStatic)
                {
                    flags |= BindingFlags.Static;
                }

                return flags;
            }
        }


        public bool BindStatic { get; set; }

        public Filters ExcludeFilters { get; set; }


        public TraverseSettings()
        {
            MaximumDepth = 100;
            ExcludeFilters = new Filters();
            //ExcludeTypes = new TypeFilters();
        }
    }
}