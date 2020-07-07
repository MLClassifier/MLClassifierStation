using MLCS.Entities;
using MLCS.Entities.Vectors;
using MLCS.Entities.Model;
using MLCS.Entities.Model.Features;
using MLCS.Entities.Model.Default;
using MLCS.LearningStatistics.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MLCS.LearningAlgorithmPlugins.Default
{
    public abstract class BaseLearningAlgorithm : ILearningAlgorithm
    {
        public virtual IModel GenerateModel(IEnumerable<IVector> trainingExamples,
            IEnumerable<IVector> validationExamples, IMetric metric = null)
        {
            IEnumerable<IVector> clearedTrainingExamples = trainingExamples.ClearSkipFeatures();
            IEnumerable<IVector> clearedValidationExamples = validationExamples.ClearSkipFeatures();

            return OnGenerateModel(clearedTrainingExamples, clearedValidationExamples, metric);
        }

        protected abstract IModel OnGenerateModel(IEnumerable<IVector> trainingExamples,
            IEnumerable<IVector> validationExamples, IMetric metric = null);

        protected IEnumerable<ILearningParameter> GetParameters()
        {
            IEnumerable<PropertyInfo> learningParameters = GetType().GetProperties().Where(
                    prop => Attribute.IsDefined(prop, typeof(LearningParameterAttribute)));

            return learningParameters.Select(lp => new LearningParameter()
            {
                Name = lp.Name,
                Type = lp.GetType(),
                Value = lp.GetValue(this)
            });
        }
    }
}