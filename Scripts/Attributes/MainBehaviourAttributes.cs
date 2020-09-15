using System;

namespace FazAppFramework.Attributes
{
    /// <summary>
    /// Fields and properties decorated with this attribute will be assigned in Awake with GetComponent() function
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class GetComponentAttribute : Attribute
    { }

    /// <summary>
    /// Fields and properties decorated with this attribute will be assigned in Awake with GetComponentInChildren() function
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class GetComponentInChildrenAttribute : Attribute
    { }

    /// <summary>
    /// Fields and properties decorated with this attribute will be assigned in Awake with GetComponentInParent() function
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class GetComponentInParentAttribute : Attribute
    { }
}
