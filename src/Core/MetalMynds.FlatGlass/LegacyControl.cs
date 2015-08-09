// -----------------------------------------------------------------------
// <copyright file="LegacyControl.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MetalMynds.FlatGlass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Automation;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LegacyControl
    {

        private AutomationElement _element;
        

        public LegacyControl(AutomationElement Element, String Name)
        {
            _element = Element;
        }

        public void Select()
        {
            
        }

    }
}
