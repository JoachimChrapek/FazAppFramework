using System;

namespace FazAppFramework.Attributes
{
    /// <summary>
    /// Fields and properties decorated with this attribute will be injected with ResolveDependencies function.
    /// It is called in Awake function of MainBehaviour by default.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DependencyInjectAttribute : Attribute
    {
    }
}
