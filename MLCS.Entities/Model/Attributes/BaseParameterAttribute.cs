using System;

namespace MLCS.Entities.Model.Features
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public abstract class BaseParameterAttribute : Attribute
    {
    }
}