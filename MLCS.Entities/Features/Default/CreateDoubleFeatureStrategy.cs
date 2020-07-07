using MLCS.Entities.Features.Numerical;

namespace MLCS.Entities.Features.Default
{
    public class CreateDoubleFeatureStrategy : ICreateFeatureStrategy
    {
        private INumericalFeatureFactory<double> doubleFeatureFactory;

        public CreateDoubleFeatureStrategy(INumericalFeatureFactory<double> doubleFeatureFactory)
        {
            this.doubleFeatureFactory = doubleFeatureFactory;
        }

        public IFeature Create(int index, string name, string line, bool isClass = false, bool isSkipFeature = false)
        {
            double from, to;
            (from, to) = NumericalUtils.GetDoubleInterval(line);

            return doubleFeatureFactory.Create(index, name, from, to, isSkipFeature);
        }
    }
}