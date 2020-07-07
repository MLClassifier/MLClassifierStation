namespace MLCS.Entities.Values
{
    public interface ICreateValueStrategy
    {
        IValue Create(object value);
    }
}