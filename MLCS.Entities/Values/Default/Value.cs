using MLCS.Entities.Features;

namespace MLCS.Entities.Values.Default
{
    public class Value : IValue
    {
        public object UntypedValue { get; set; }

        public virtual int CompareTo(object other)
        {
            return 0;
        }

        public virtual bool Covers(ICoverable other)
        {
            return false;
        }

        public virtual ICoverable FindCovering(ICoverable other)
        {
            return null;
        }

        public virtual ICoverable FindCovering(ICoverable other, IFeature feature)
        {
            return null;
        }

        public virtual ICoverable FindCommon(ICoverable other)
        {
            return null;
        }
    }
}