// -----------------------------------------------------------------------
// <copyright file="WellKnownAttribute.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MetalMynds.FlatGlass
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    /// 
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Field)]
    public class WellKnownAsAttribute : Attribute
    {
        private String _name;

        public WellKnownAsAttribute(String name) 
        {
            _name = name;
        }

        public virtual String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
    }
}
