namespace MLCS.Entities.Features.Default
{
    public class CreateCategorialFeatureStrategy : ICreateFeatureStrategy
    {
        private ICategorialFeatureFactory<string> categorialFeatureFactory;

        public CreateCategorialFeatureStrategy(ICategorialFeatureFactory<string> categorialFeatureFactory)
        {
            this.categorialFeatureFactory = categorialFeatureFactory;
        }

        public IFeature Create(int index, string name, string line, bool isClass, bool isSkipFeature = false)
        {
            string[] values = line.Split(',');
            return categorialFeatureFactory.Create(index, name, isClass, values, isSkipFeature);
        }
    }
}