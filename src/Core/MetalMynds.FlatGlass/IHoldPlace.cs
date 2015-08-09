// -----------------------------------------------------------------------
// <copyright file="IHoldPlace.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MetalMynds.FlatGlass
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Automation;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IHoldPlace
    {
        String WellKnownAs { get; }
        Boolean IsWellKnown { get; }
        List<Locator> LocatorList { get; }
        Type HeldType { get; }
        Object GetControl();
        AutomationElement Element { get; }
        void Forget();
        Object Window { get; }
    }
}
