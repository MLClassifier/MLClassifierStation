namespace MLCS.Entities.Values
{
    public interface IValueFactory
    {
        IValue Create(object value);
    }
}