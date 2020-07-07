using System;
using System.Collections.Generic;
using System.Reflection;

namespace MLClassifierStation.Common
{
    public class SettingsViewModelBase : WizardElementViewModelBase
    {
        public void SetParameterValues(object instance, IEnumerable<IParameterViewModel> parameters)
        {
            Type type = instance.GetType();
            foreach (IParameterViewModel parameter in parameters)
            {
                int parameterValue;
                bool isInt = int.TryParse(parameter.UntypedParameter.ToString(), out parameterValue);
                if (!isInt)
                    throw new Exception(
                        string.Format("Please enter an integer value for setting parameter {0}.", parameter.Name));

                PropertyInfo propertyInfo = type.GetProperty(parameter.Name);
                propertyInfo.SetValue(instance, parameterValue);
            }
        }
    }
}