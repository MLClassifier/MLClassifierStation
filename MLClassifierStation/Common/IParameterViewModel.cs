namespace MLClassifierStation.Common
{
    public interface IParameterViewModel : IViewModel
    {
        string Name { get; }
        object UntypedParameter { get; set; }
    }
}