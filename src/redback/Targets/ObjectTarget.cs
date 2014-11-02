using System;

namespace Redback
{
    public class ObjectTarget
    {
        public object Value { get; set; }

        public Type Type { get; set; }

        public ObjectTarget()
        {

        }

        public ObjectTarget(object value)
        {
            Value = value;
            Type = value.GetType();
        }

        public override string ToString()
        {
            return string.Format("{{{1}}} {0}", Value, Type.Name);
        }
    }
}