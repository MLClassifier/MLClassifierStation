using MLCS.Entities.Values.Numerical;
using System;
using System.Collections.Generic;

namespace MLCS.Entities.Values.Default
{
    public class ValueFactory : IValueFactory
    {
        private IDictionary<Type, ICreateValueStrategy> valueTypes;

        public ValueFactory()
        {
            valueTypes = new Dictionary<Type, ICreateValueStrategy>();
            valueTypes.Add(typeof(string), new CreateCategorialValueStrategy());
            valueTypes.Add(typeof(int), new CreateNumericalValueStrategy());
            valueTypes.Add(typeof(double), new CreateNumericalValueStrategy());
        }

        public IValue Create(object value)
        {
            return valueTypes[value.GetType()].Create(value);
        }
    }
}