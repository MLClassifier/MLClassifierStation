namespace MLCS.Entities.Values.Numerical
{
    public class CreateNumericalValueStrategy : ICreateValueStrategy
    {
        public IValue Create(object value)
        {
            return new NumericalValue()
            {
                UntypedValue = new Interval()
                {
                    UntypedFrom = value,
                    UntypedTo = value
                }
            };
        }
    }
}