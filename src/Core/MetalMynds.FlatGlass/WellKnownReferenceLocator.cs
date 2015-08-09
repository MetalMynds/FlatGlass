// -----------------------------------------------------------------------
// <copyright file="WellKnownLocator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MetalMynds.FlatGlass
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WellKnownReferenceLocator : Locator
    {
        private readonly String _wellKnownName;
        private readonly String _description;

        public WellKnownReferenceLocator(String wellKnownName)
            : base(wellKnownName, -1, Scope.None)
        {
            _wellKnownName = wellKnownName;
            _description = String.Format("Place Holder Reference: Well Known Name: [{0}]", wellKnownName); 
        }

        public String WellKnownName
        {
            get
            {
                return _wellKnownName;
            }
        }

        public override System.Windows.Automation.Condition Condition
        {
            get { throw new NotImplementedException("WellKnownReferenceLocator Condition Property is Never Implemented!"); }
        }

        public override string Description
        {
            get { return _description; }
        }
    }
}
