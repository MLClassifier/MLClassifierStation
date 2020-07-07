namespace MLCS.Entities.Values.Default
{
    public class CreateCategorialValueStrategy : ICreateValueStrategy
    {
        public IValue Create(object value)
        {
            return new CategorialValue()
            {
                UntypedValue = value
            };
        }
    }
}