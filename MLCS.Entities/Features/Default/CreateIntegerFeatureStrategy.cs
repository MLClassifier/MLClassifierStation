using MLCS.Entities.Features.Numerical;

namespace MLCS.Entities.Features.Default
{
    public class CreateIntegerFeatureStrategy : ICreateFeatureStrategy
    {
        private INumericalFeatureFactory<int> integerFeatureFactory;

        public CreateIntegerFeatureStrategy(INumericalFeatureFactory<int> integerFeatureFactory)
        {
            this.integerFeatureFactory = integerFeatureFactory;
        }

        public IFeature Create(int index, string name, string line, bool isClass = false, bool isSkipFeature = false)
        {
            int from, to;
            (from, to) = NumericalUtils.GetIntegerInterval(line);

            return integerFeatureFactory.Create(index, name, from, to, isSkipFeature);
        }
    }
}