using System;

namespace MetalMynds.FlatGlass
{
    /// <summary>
    /// Marks the element so that lookups to the UI window are cached. This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class CacheLookupAttribute : Attribute
    {
    }
}